using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace TinyORM.Tests.Mapping
{
    public static class Common
    {
        public static DataSet GenerateDataSet(int dataTables, int rows)
        {
            var rtnVal = new DataSet("Temp Dataset");

            for (var i = 0; i < dataTables; i++)
                rtnVal.Tables.Add(GenerateDataTable(rows));

            return rtnVal;
        }

        public static DataTable GenerateDataTable(int rows)
        {
            var rtnVal = new DataTable("Temp Datatable");

            rtnVal.Columns.Add("PropertyInt", typeof(int));
            rtnVal.Columns.Add("PropertyBool", typeof(bool));
            rtnVal.Columns.Add("PropertyLong", typeof(long));
            rtnVal.Columns.Add("PropertyString", typeof(string));
            rtnVal.Columns.Add("PropertyDouble", typeof(double));
            rtnVal.Columns.Add("PropertyFloat", typeof(float));

            for (var i = 1; i < rows + 1; i++)
            {
                var row = rtnVal.NewRow();
                row[0] = i;
                row[1] = i % 2;
                row[2] = i;
                row[3] = i.ToString(CultureInfo.InvariantCulture);
                row[4] = i + 0.01;
                row[5] = i + 0.01;

                rtnVal.Rows.Add(row);
            }

            return rtnVal;
        }

        #region ObjectGeneration
        public class DummyClass
        {
            public int PropertyInt { get; set; }
            public bool PropertyBool { get; set; }
            public long PropertyLong { get; set; }
            public string PropertyString { get; set; }
            public double PropertyDouble { get; set; }
            public float PropertyFloat { get; set; }
        }

        /// <summary>
        /// A class with funny un-matching names (in upper or lowercase)
        /// This tests the matching engine
        /// </summary>
        public class DummyClassWrong
        {
            public int propertyint { get; set; }
            public bool PROPERTYBool { get; set; }
            public long PropertyLONG { get; set; }
            public string PROPERTYSTRING { get; set; }
            public double propertyDouble { get; set; }
            public float ProPertyFloaT { get; set; }
        }

        public static List<DummyClass> GenerateExpectedDummyObject(int rows)
        {
            var rtnVal = new List<DummyClass>();

            for (var i = 1; i < rows + 1; i++)
            {
                var row = new DummyClass
                {
                    PropertyInt = i,
                    PropertyBool = (i % 2) == 1,
                    PropertyLong = i,
                    PropertyString = i.ToString(CultureInfo.InvariantCulture),
                    PropertyFloat = i + 0.01F,
                    PropertyDouble = i + 0.01D
                };

                rtnVal.Add(row);
            }

            return rtnVal;
        }

        public static List<DummyClassWrong> GenerateExpectedWrongDummyObject(int rows)
        {
            var rtnVal = new List<DummyClassWrong>();

            for (var i = 1; i < rows + 1; i++)
            {
                var row = new DummyClassWrong
                {
                    propertyint = i,
                    PROPERTYBool = (i % 2) == 1,
                    PropertyLONG = i,
                    PROPERTYSTRING = i.ToString(CultureInfo.InvariantCulture),
                    ProPertyFloaT = i + 0.01F,
                    propertyDouble = i + 0.01D
                };

                rtnVal.Add(row);
            }

            return rtnVal;
        }
        #endregion

        public static void PrintTime(DateTime timeStamp, DateTime now)
        {
            var different = now - timeStamp;
            Debug.WriteLine("Total Milliseconds:" + different.TotalMilliseconds);
            Debug.WriteLine("Total Seconds:" + different.TotalSeconds);
            Debug.WriteLine("Total Minutes:" + different.TotalMinutes);
            Debug.WriteLine("");
        }
    }
}
