using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using TinyORM.Utils;

namespace TinyORM.Mapping
{
    public class TinyMapper : IMapper
    {
        public T Map<T>(object dbValue)
        {
            T rtnVal;

            //Just return a blank object if we have nothing to work from
            if (dbValue == null)
                rtnVal = default(T);
            //If the object is already converted, then use it
            else if (dbValue.GetType() == typeof(T))
                rtnVal = (T)dbValue;
            else
                //Otherwise, lets map it the best we can...
                rtnVal = DbUtils.IsGenericACollection<T>()
                                 ? MapCollection<T>(dbValue)
                                 : MapSingleObject<T>(dbValue);

            return (T)ProcessSqlValue<T>(rtnVal);
        }

        #region MapSingleObject
        /// <summary>
        /// Maps a single object to an output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbValue"></param>
        /// <returns></returns>
        private T MapSingleObject<T>(object dbValue)
        {
            //Here, we assume we want to try and use reflection to setup a new object with the parameters set
            //from the 1st datatable. Assumed to be a single object, otherwise it would be encapsulated in a list

            if (dbValue.GetType() == typeof(DataSet))
                dbValue = DbUtils.GetDataTable(dbValue);

            if (dbValue.GetType() == typeof(DataTable))
            {
                var dt = (DataTable)dbValue;
                return dt.Rows.Count > 0 ? ConvertDataRowToObject<T>(dt.Columns, dt.Rows[0]) : default(T);
            }

            //If we are here, then we have a single object trying to be converted into another object.
            //All we can do is try and cast it... TODO: Find a better way?
            return (T)dbValue;
        }

        #endregion

        #region MapCollection

        private T MapCollection<T>(object dbValue)
        {
            if (DbUtils.IsDataTable<T>())
                return (T)MapDataTable(dbValue);

            if (DbUtils.IsList<T>())
                return MapList<T>(dbValue);

            return default(T);
        }

        #endregion

        #region MapList
        /// <summary>
        /// Convert the input into a list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbValue"></param>
        /// <returns></returns>
        private T MapList<T>(object dbValue)
        {
            DataTable dt = null;

            if (dbValue.GetType() == typeof(DataSet))
                dt = DbUtils.GetDataTable(dbValue);
            else if (dbValue.GetType() == typeof(DataTable))
                dt = (DataTable)dbValue;

            if (dt == null)
                return default(T);

            var objType = typeof(T).GetGenericArguments()[0];
            var generatedList = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(objType)));
            var convertDataRowToObjectHandler = GetType().GetMethod("ConvertDataRowToObject", BindingFlags.NonPublic | BindingFlags.Instance);
            var convertDataRowToObjectMethod = convertDataRowToObjectHandler.MakeGenericMethod(new[] { objType });
            
            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                    generatedList.Add(convertDataRowToObjectMethod.Invoke(this, new object[] { dt.Columns, dt.Rows[i] }));
            }

            return (T)generatedList;
        }


        #endregion

        #region MapDatatable

        private object MapDataTable(object dbValue)
        {
            return DbUtils.GetDataTable(dbValue);
        }

        #endregion

        #region MiscFunctions
        private T ConvertDataRowToObject<T>(DataColumnCollection columnDefs, DataRow dr)
        {
            if (columnDefs.Count < 1 || dr == null)
                throw new Exception("Not enough information to process");

            if (DbUtils.IsValueType<T>())
            {
                var column = InitialiseObject<T>(dr, columnDefs[0].ColumnName);
                return column == null ? default(T) : ConvertValueType<T>(column);
            }

            var t = typeof(T); //TODO: Move this into parameter that gets passed in; for better speed?
            var newObject = (T)FormatterServices.GetUninitializedObject(typeof(T));

            for (var i = 0; i <= columnDefs.Count - 1; i++)
            {
                t.InvokeMember(columnDefs[i].ColumnName,
                               BindingFlags.SetProperty, null,
                               newObject,
                               new[] { InitialiseObject<T>(dr, columnDefs[i].ColumnName) });
            }

            return newObject;
        }

        private static T ConvertValueType<T>(object value)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)value.ToString();

            return (T) value;
        }

        private static object InitialiseObject<T>(DataRow dr, String columnName)
        {
            return ProcessSqlValue<T>(dr[columnName]);
        }

        private static object ProcessSqlValue<T>(object value)
        {
            if (value is DBNull || value == null)
                return null;

            //Dont bother converting if we are already ok :)
            if(typeof(T) == value.GetType())
                return value;

            if (typeof(T) == typeof(bool))
                return ProcessBool(value);

            if (typeof(T) == typeof(int))
                return ProcessInt(value);

            if (typeof(T) == typeof(long))
                return ProcessLong(value);

            if (typeof(T) == typeof(double))
                return ProcessDouble(value);

            if (typeof(T) == typeof(float))
                return ProcessFloat(value);

            return value;
        }

        #region BoolProcessing
        private static object ProcessBool(object value)
        {
            if (value is int || value is float || value is double)
                return ProcessIntAsBool(value);

            if (value is string)
                return ProcessStringAsBool(value);

            return value;
        }

        private static object ProcessStringAsBool(object value)
        {
            var strVal = (string)value;
            int intParse;

            if (Int32.TryParse(strVal, out intParse))
                return ProcessIntAsBool(intParse);

            return false;
        }

        private static object ProcessIntAsBool(object value)
        {
            return (int)value == 1;
        }
        #endregion

        #region NumberProcessing

        private static object ProcessInt(object value)
        {
            return int.Parse(value.ToString());
        }

        private static object ProcessLong(object value)
        {
            return long.Parse(value.ToString());
        }

        private static object ProcessDouble(object value)
        {
            return double.Parse(value.ToString());
        }

        private static object ProcessFloat(object value)
        {
            return float.Parse(value.ToString());
        }
        #endregion
        #endregion
    }
}