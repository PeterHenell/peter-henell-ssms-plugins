using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest
{
    public class BufferReader<T> where T : class
    {
        ConcurrentQueue<T> collection;
        private object _lock = new object();
        private bool _allIsRead;

        public BufferReader()
        {

        }

        public void BeginRead(IEnumerable<T> items)
        {
            collection = new ConcurrentQueue<T>();
            AllIsRead = false;

            Action task = new Action(() =>
            {
                foreach (var item in items)
                {
                    collection.Enqueue(item);
                }
                this.AllIsRead = true;
            });

            task.BeginInvoke(null, null);
        }

        public T Pop()
        {
            if (!AllIsRead)
            {
                T r = null;
                bool readOne = false;
                while (readOne == false && !AllIsRead)
                {
                    readOne = collection.TryDequeue(out r);
                }
                return r;
            }
            return null;
        }

        public bool AllIsRead
        {
            get
            {
                lock (_lock)
                {
                    return _allIsRead;
                }
            }
            set
            {
                lock (_lock)
                {
                    _allIsRead = value;
                }
            }
        }
    }
}
