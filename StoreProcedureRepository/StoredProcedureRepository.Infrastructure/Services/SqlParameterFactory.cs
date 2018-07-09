using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StoredProcedureRepository.Infrastructure.Services
{
    public static class SqlParameterFactory
    {
        /// <summary>
        /// Creates SqlParameter. 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string paramName, object param)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(param);

            return new SqlParameter($"@{ paramName }", param);
        }

        /// <summary>
        /// Creates SqlParameter for every property in provided object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SqlParameter[] BuildParamsForObject(object obj)
        {
            Guard.ThrowIfNull(obj);

            var props = obj.GetType().GetProperties();
            var names = props.Select(p => p.Name).ToList();

            return props.Select(p => CreateParameter(p.Name, p.GetValue(obj, null))).ToArray();
        }

        /// <summary>
        /// Creates user defined table type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlParameter BuildUserDefinedTableTypeParameter<T>(string paramName, IList<T> value)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(value);
            Guard.ThrowIfEmpty(value);

            return new SqlParameter
            {
                ParameterName = $"@{paramName}",
                TypeName = $"dbo.{paramName}",
                SqlDbType = SqlDbType.Structured,
                Value = ConvertToDataTable(value)
            };
        }

        private static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
