using System.Data;
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

            for (var i = 1; i < rows + 1; i++)
            {
                var row = rtnVal.NewRow();
                row[0] = i;
                row[1] = i % 2;
                row[2] = i;
                row[3] = i.ToString(CultureInfo.InvariantCulture);

                rtnVal.Rows.Add(row);
            }

            return rtnVal;
        }
    }
}
