//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer;

//namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPluginTests.DataSetCompareCommandTests
//{
//    [TestFixture]
//    public class DataSetComparerTest
//    {
//        DataSetComparer comparer = new DataSetComparer();

//        [Test]
//        public void DatasetShouldBeEqualToItself()
//        {
//            var dt = new DataTable();
//            Assert.That(comparer.SetsAreEqual(dt, dt));
//        }

//        [Test]
//        public void DifferentDatasetsShouldBeEqualIfTheyHaveTheSameRows()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID" },  
//                new DataColumn { ColumnName = "Name" },  
//            });
//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID" },  
//                new DataColumn { ColumnName = "Name" },  
//            });

//            AddRowTo(a, ID: 555, Name: "Anders");
//            AddRowTo(b, ID: 555, Name: "Anders");

//            Assert.That(comparer.SetsAreEqual(a, b));
//            Assert.That(comparer.SetsAreEqual(b, a));
//        }

//        [Test]
//        public void DifferentDatasetsShouldNotBeEqualIfTheyHaveDifferentRows()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });
//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            AddRowTo(a, ID: 555, Name: "Anders");
//            AddRowTo(b, ID: 444, Name: "Peter");

//            Assert.That(comparer.SetsAreEqual(a, b), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a), Is.False);
//        }

//        [Test]
//        public void EmptyDatasetsShouldNotBeEqualToDatasetWithData()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });
//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            AddRowTo(a, ID: 555, Name: "Anders");

//            Assert.That(comparer.SetsAreEqual(a, b), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a), Is.False);
//        }

//        [Test]
//        public void NullDataSetShouldNotBeEqualToAnything()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });
//            DataTable b = null;

//            AddRowTo(a, ID: 555, Name: "Anders");

//            Assert.That(comparer.SetsAreEqual(a, b), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, b), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a), Is.False);
//        }

//        [Test]
//        public void EmptySetsShouldBeEqual()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            Assert.That(comparer.SetsAreEqual(a, b));
//            Assert.That(comparer.SetsAreEqual(b, a));
//        }

//        [Test]
//        public void DataSetsWithDifferentColumnsShouldNotbeEqual()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "Identifier", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "CustomerName", DataType = typeof(string) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            var arow = a.NewRow();
//            arow["Identifier"] = 555;
//            arow["CustomerName"] = "Peter";

//            AddRowTo(b, ID: 555, Name: "Peter");

//            a.Rows.Add(arow);

//            Assert.That(comparer.SetsAreEqual(a, b), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a), Is.False);
//        }

//        [Test]
//        public void ShouldCompareWithOnlyTheSelectedColumns()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//            });

//            AddRowTo(a, ID: 555, Name: "Peter");
//            AddRowTo(b, ID: 555);

//            Assert.That(comparer.SetsAreEqual(a, b, "ID"), Is.True);
//            Assert.That(comparer.SetsAreEqual(b, a, "ID"), Is.True);
//            Assert.That(comparer.SetsAreEqual(a, b, "Name"), Is.False);
//        }

//        [Test]
//        public void GivenManyColumnsShouldCompareOnlyFew()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//                new DataColumn { ColumnName = "CreatedOn", DataType = typeof(DateTime) },  
//                new DataColumn { ColumnName = "IsActive", DataType = typeof(bool) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//                new DataColumn { ColumnName = "CreatedOn", DataType = typeof(DateTime) },  
//                new DataColumn { ColumnName = "IsActive", DataType = typeof(bool) },  
//            });

//            for (int i = 0; i < 100; i++)
//            {
//                AddRowTo(a, ID: i, IsActive: i == 50);
//                AddRowTo(b, ID: i, IsActive: i == 50);
//            }

//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "IsActive"), Is.True);
//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "IsActive", "CreatedOn", "Name"), Is.True);
//            Assert.That(comparer.SetsAreEqual(b, a, "ID"), Is.True);

//            AddRowTo(a, ID: 9999, IsActive: true);
//            AddRowTo(b, ID: 9999, IsActive: false);

//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "IsActive"), Is.False);
//        }

//        [Test]
//        public void ShouldNotBeEqualWhenThereIsUnevenAmountOfRows()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//                new DataColumn { ColumnName = "CreatedOn", DataType = typeof(DateTime) },  
//                new DataColumn { ColumnName = "IsActive", DataType = typeof(bool) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },  
//                new DataColumn { ColumnName = "CreatedOn", DataType = typeof(DateTime) },  
//                new DataColumn { ColumnName = "IsActive", DataType = typeof(bool) },  
//            });

//            for (int i = 0; i < 10; i++)
//            {
//                AddRowTo(a, ID: 77777, IsActive: true);
//            }

//            AddRowTo(b, ID: 77777, IsActive: true);

//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "IsActive"), Is.False);
//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "IsActive", "CreatedOn", "Name"), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a, "ID"), Is.False);
//        }

//        [Test]
//        public void ShouldNotBeEqualWhenTheyContainTheSameValuesButInUnequalAmount()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//            });

//            AddRowTo(a, ID: 10);
//            AddRowTo(a, ID: 20);
//            AddRowTo(a, ID: 30);
//            AddRowTo(a, ID: 10);

//            AddRowTo(b, ID: 10);
//            AddRowTo(b, ID: 20);
//            AddRowTo(b, ID: 30);
//            AddRowTo(b, ID: 20);

//            Assert.That(comparer.SetsAreEqual(a, b, "ID"), Is.False);
//            Assert.That(comparer.SetsAreEqual(a, b, "ID"), Is.False);
//        }

//        [Test]
//        public void ShouldNotBeEqualWhenTheyAreNotBagEqual()
//        {
//            var a = new DataTable();
//            a.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },
//            });

//            var b = new DataTable();
//            b.Columns.AddRange(new DataColumn[] {
//                new DataColumn { ColumnName = "ID", DataType = typeof(int) },  
//                new DataColumn { ColumnName = "Name", DataType = typeof(string) },
//            });

//            AddRowTo(a, ID: 10, Name: "Peter");
//            AddRowTo(a, ID: 10, Name: "Peter");
//            AddRowTo(a, ID: 10, Name: "Peter");
//            AddRowTo(a, ID: 10, Name: "Anders");

//            AddRowTo(b, ID: 10, Name: "Peter");
//            AddRowTo(b, ID: 10, Name: "Peter");
//            AddRowTo(b, ID: 10, Name: "Anders");
//            AddRowTo(b, ID: 10, Name: "Anders");

//            Assert.That(comparer.SetsAreEqual(a, b, "ID", "Name"), Is.False);
//            Assert.That(comparer.SetsAreEqual(b, a, "ID", "Name"), Is.False);
//        }


//        private static void AddRowTo(DataTable table, int ID)
//        {
//            var row = table.NewRow();
//            row["ID"] = ID;
//            table.Rows.Add(row);
//        }
//        private static void AddRowTo(DataTable table, int ID, string Name)
//        {
//            var row = table.NewRow();
//            row["ID"] = ID;
//            row["Name"] = Name;
//            table.Rows.Add(row);
//        }
//        private static void AddRowTo(DataTable table, int ID, bool IsActive)
//        {
//            var row = table.NewRow();
//            row["ID"] = ID;
//            row["IsActive"] = IsActive;
//            table.Rows.Add(row);
//        }
//    }
//}
