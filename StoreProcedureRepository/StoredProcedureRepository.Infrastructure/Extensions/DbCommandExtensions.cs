using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using StoredProcedureRepository.Infrastructure.Services;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class DbCommandExtensions
    {
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue, Action<SqlParameter> configureParam = null)
        {

            Guard.ThrowIfAnyIsNullOrEmpty(cmd.CommandText, paramName);
            Guard.ThrowIfNull(paramValue);

            var sqlParam = SqlParameterFactory.CreateParameter(paramName, paramValue);
            cmd.ConfigureParamAndAdd(sqlParam, configureParam);

            return cmd;
        }

        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, Action<SqlParameter> configureParam = null)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);

            var sqlParam = SqlParameterFactory.CreateParameter(paramName);
            cmd.ConfigureParamAndAdd(sqlParam, configureParam);

            return cmd;
        }

        public static DbCommand WithUserDefinedDataTableSqlParam<T>(this DbCommand cmd, string paramName, IList<T> paramValue, Action<SqlParameter> configureParam = null)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);
            Guard.ThrowIfNull(paramValue);

            var sqlParam = SqlParameterFactory.BuildUserDefinedTableTypeParameter(paramName, paramValue);
            cmd.ConfigureParamAndAdd(sqlParam, configureParam);

            return cmd;
        }

        private static void ConfigureParamAndAdd(this DbCommand cmd, SqlParameter param, Action<SqlParameter> configureParam)
        {
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
        }
    }
}
