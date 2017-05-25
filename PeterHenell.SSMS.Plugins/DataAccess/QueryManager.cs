using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.Collections.Generic;
using System.Threading;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class QueryManager : IDisposable
    {

        private string _connectionString;
        private SqlCommand _currentCommand;
        private CancellationToken _cancellationToken;
        private bool _completed = false;

        private QueryManager(string connectionString, CancellationToken token)
        {
            this._connectionString = connectionString;
            this._cancellationToken = token;
        }

        public static void Run(string connectionString, CancellationToken token, Action<QueryManager> action)
        {
            using (var manager = new QueryManager(connectionString, token))
            {
                action(manager);
                manager._completed = true;
            }
            //Run<object>(connectionString, token, (qm) => action);
        }
        public static T Run<T>(string connectionString, CancellationToken token, Func<QueryManager, T> action)
        {
            using (var manager = new QueryManager(connectionString, token))
            {
                return action(manager);
            }
        }

        /// <summary>
        /// Fills a dataset by executing the supplied command in the current window connection
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public void Fill(string query, DataSet ds)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = GetCommand(query, con))
            using (var ad = new SqlDataAdapter(cmd))
            {
                ad.Fill(ds);
            }
            _completed = true;
        }

        public void ExecuteQuery(string sql, Action<SqlDataReader> streamReaderCallback)
        {
            using (var con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = GetCommand(sql, con))
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        streamReaderCallback(reader);
                    }
                    _completed = true;
                }
            }
        }

        private SqlCommand GetCommand(string sql, SqlConnection con)
        {
            // caller need to take care of command and dispose it
            SqlCommand cmd = new SqlCommand(sql, con);
            this._currentCommand = cmd;

            if (_cancellationToken != null)
            {
                StartCancellationMonitoring();
                this._currentCommand.Disposed += _currentCommand_Disposed;
            }

            return cmd;
        }

        private void _currentCommand_Disposed(object sender, EventArgs e)
        {
            _completed = true;
        }

        private void StartCancellationMonitoring()
        {
            _completed = false;
            var a = new Action(() =>
            {
                while (!_completed)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        Cancel();
                        return;
                    }
                    Thread.Sleep(10);
                }
            });
            a.BeginInvoke(null, null);
        }

        private void Cancel()
        {
            if (_currentCommand != null)
            {
                _currentCommand.Cancel();
                _currentCommand.Dispose();
            }
        }

        public void Dispose()
        {
            if (_currentCommand != null)
            {
                _currentCommand.Dispose();
            }
        }

        public void Fill(string query, DataTable dt, Dictionary<string, string> parameters)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = GetCommand(query, con))
            using (var ad = new SqlDataAdapter(cmd))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                ad.Fill(dt);
            }
            _completed = true;
        }

        public T1 ExecuteScalar<T1>(string query)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = GetCommand(query, con))
            {
                //Console.WriteLine(query);
                con.Open();
                var result = cmd.ExecuteScalar();
                _completed = true;
                return (T1)result;
            }
        }

        internal void Fill(string query, DataTable dt)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = GetCommand(query, con))
            using (var ad = new SqlDataAdapter(cmd))
            {
                ad.Fill(dt);
            }
            _completed = true;
        }
    }
}
