using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StoredProcedureRepository.Infrastructure.Services
{
    public static class SqlParameterFactory
    {
        public static SqlParameter CreateParameter(string paramName, object param)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(param);

            return new SqlParameter($"@{ paramName }", param);
        }

        public static SqlParameter CreateParameter(string paramName)
        {
            return string.IsNullOrEmpty(paramName)
                ? throw new ArgumentException(nameof(paramName))
                : new SqlParameter { ParameterName = $"@{ paramName }" };
        }

        public static SqlParameter BuildParamsWhenUserDefinedTableType<T>(string paramName, object obj)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(obj);

            return new SqlParameter(paramName, SqlDbType.Structured)
            {
                TypeName = $"dbo.{ paramName }",
                Value = ConvertToDataTable(obj as List<T>)
            };
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
        /// Creates SqlParameter
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SqlParameter[] BuildParamsWhenUserDefinedTableType(object obj)
        {
            Guard.ThrowIfNull(obj);

            var props = obj.GetType().GetProperties();
            var names = props.Select(p => p.Name).ToList();

            var result = new List<SqlParameter>();
            foreach (var prop in props)
            {
                if (!(typeof(IList<>).IsAssignableFrom(prop.PropertyType) && prop.GetType().IsGenericType))
                {
                    result.Add(CreateParameter(prop.Name, prop.GetValue(obj, null)));
                }
                else
                {
                    result.Add(new SqlParameter($"@{ prop.Name }", SqlDbType.Structured)
                    {
                        TypeName = $"dbo.{ prop.Name }",
                        Value = ConvertToDataTable(new List<object> { prop.GetValue(obj, null) })
                    });
                }
            }
            return result.ToArray();
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
