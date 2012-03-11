using System;
using System.Collections.Generic;
using System.Data;

namespace TinyORM.Utils
{
    public static class DbUtils
    {
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != typeof (object))
            {
                if (toCheck == null)
                    break;

                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static bool IsGenericACollection<T>()
        {
            if (IsValueType<T>())
                return false;

            //Is it a List?
            if (IsList<T>())
                return true;

            //Is it a Datatable?
            if (IsDataTable<T>())
                return true;

            //Is it a Dataset?
            return IsDataSet<T>();
        }

        public static bool IsList<T>()
        {
            return IsSubclassOfRawGeneric(typeof(List<>), typeof(T));
        }

        public static bool IsDataTable<T>()
        {
            return IsSubclassOfRawGeneric(typeof(DataTable), typeof(T));
        }

        public static bool IsDataSet<T>()
        {
            return IsSubclassOfRawGeneric(typeof(DataSet), typeof(T));
        }

        public static bool IsValueType<T>()
        {
            return IsValueType(typeof(T));
        }

        public static bool IsValueType(Type T)
        {
            if (T.IsValueType)
                return true;
            if (T == typeof(string))
                return true;

            return T == typeof(String);
        }

        public static DataTable GetDataTable(object rtnObj)
        {
            if (rtnObj != null)
            {
                if (rtnObj.GetType() == typeof (DataSet))
                {
                    var tmpDs = (DataSet) rtnObj;
                    return tmpDs.Tables.Count > 0 ? tmpDs.Tables[0] : new DataTable();
                }

                throw new Exception("Input object is not of type DataSet. Unable to obstract a datatable."); //TODO: Trans
            }

            return new DataTable();
        }


        //TODO: Work this out in a standard way
        private const string StoredProcedurePrefix = "usp_";
        public static CommandType SqlCommandOrUsp(string strSql)
        {
            //It's a stored proc with no parameters (separated by a space from the sp name) added on.
            if (strSql.StartsWith(StoredProcedurePrefix, StringComparison.CurrentCultureIgnoreCase) && (!strSql.Contains(" ")))
                return CommandType.StoredProcedure;

            return CommandType.Text;
        }

        public static object ProcessStringAsBool(object value)
        {
            var strVal = (string)value;
            int intParse;

            if (Int32.TryParse(strVal, out intParse))
                return ProcessIntAsBool(intParse);

            return false;
        }

        public static object ProcessIntAsBool(object value)
        {
            return (int)value == 1;
        }
    }
}