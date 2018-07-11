using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static IList<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetProperties();

            var colMapping = Enumerable.Range(0, dr.FieldCount)
                .Select(dr.GetName)
                .Where(columnName => props.Any(property => string.Equals(property.Name, columnName, StringComparison.OrdinalIgnoreCase)))
                .ToDictionary(key => key);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        var value = dr[colMapping[prop.Name]];
                        prop.SetValue(obj, value == DBNull.Value ? null : value);
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }
    }
}
