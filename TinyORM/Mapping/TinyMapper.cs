using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using TinyORM.Utils;
using TinyORM.Exceptions;

namespace TinyORM.Mapping
{
    /// <summary>
    /// The default mapper for TinyORM. Maps db values to valid return values. 
    /// </summary>
    public class TinyMapper : IMapper
    {
        /// <summary>
        /// Can throw TinyMapperException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbValue"></param>
        /// <returns></returns>
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
                return dt.Rows.Count > 0 ? ConvertDataRowToObject<T>(GenerateColumnNames(dt, typeof(T)), dt.Rows[0], typeof(T)) : default(T);
            }

            //If we are here, then we have a single object trying to be converted into another object.
            //All we can do is try and cast it... TODO: Find a better way? Will we ever get here? Maybe worth just throwing an error?
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
            var convertDataRowToObjectMethod = GenerateDataRowToObjectHandler<T>(objType);

            var objectColumns = GenerateColumnNames(dt, objType);

            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                    generatedList.Add(convertDataRowToObjectMethod.Invoke(this, new object[] { objectColumns, dt.Rows[i], objType }));
            }

            return (T)generatedList;
        }

        private MethodInfo GenerateDataRowToObjectHandler<T>(Type objType)
        {
            var convertDataRowToObjectHandler = GetType().GetMethod("ConvertDataRowToObject", BindingFlags.NonPublic | BindingFlags.Instance);
            return convertDataRowToObjectHandler.MakeGenericMethod(new[] { objType });
        }

        /// <summary>
        /// Works out the correct property name for the current object. If no name is found, an exception is raised
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        private Dictionary<string, string> GenerateColumnNames(DataTable dt, Type objType)
        {
            var objectColumns = new Dictionary<string, string>();

            //If it is a value type, then just return standard column headers
            if (DbUtils.IsValueType(objType))
            {
                foreach (DataColumn column in dt.Columns)
                    objectColumns.Add(column.ColumnName, column.ColumnName);

                return objectColumns;
            }

            var columns = objType.GetProperties().Where(x => x.CanWrite).Select(x => x.Name).ToArray();

            foreach (DataColumn column in dt.Columns)
            {
                var columnName = column.ColumnName;
                var name = columns.FirstOrDefault(x => x == columnName);

                if (string.IsNullOrEmpty(name))
                    name = columns.FirstOrDefault(x => x.ToUpper() == columnName.ToUpper());

                if (string.IsNullOrEmpty(name) == false)
                    objectColumns.Add(columnName, name);
                else
                    throw new TinyMapperException(string.Format("Unable to find property '{0}' in class type '{1}'", columnName, objType.Name));
            }

            return objectColumns;
        }

        #endregion

        #region MapDatatable

        private object MapDataTable(object dbValue)
        {
            return DbUtils.GetDataTable(dbValue);
        }

        #endregion

        #region MiscFunctions
        /// <summary>
        /// This function converts a datarow to an object by mapping column headers to properties
        /// This function can't be static (ignore resharpers suggestion)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyNames"></param>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private T ConvertDataRowToObject<T>(ICollection<KeyValuePair<string, string>> propertyNames, DataRow dr, Type type)
        {
            if (propertyNames.Count < 1 || dr == null)
                throw new TinyMapperException("Not enough information to process");

            if (DbUtils.IsValueType<T>())
            {
                var column = ProcessSqlValue<T>(dr[propertyNames.First().Key]);
                return column == null ? default(T) : ConvertValueType<T>(column);
            }

            var newObject = (T)FormatterServices.GetUninitializedObject(typeof(T));

            foreach (var columns in propertyNames)
            {
                //TODO: Replace this with Property.SetValue and see if there is any difference in performance
                type.InvokeMember(columns.Value,
                                      BindingFlags.SetProperty, null,
                                      newObject,
                                      new[] { ProcessSqlValue<T>(dr[columns.Key]) });
            }

            return newObject;
        }

        private static T ConvertValueType<T>(object value)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)value.ToString();

            return (T)value;
        }

        private static object ProcessSqlValue<T>(object value)
        {
            if (value is DBNull || value == null)
                return null;

            //Dont bother converting if we are already ok :)
            if (typeof(T) == value.GetType())
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