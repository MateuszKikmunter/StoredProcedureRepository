using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using StoredProcedureRepository.Infrastructure.Services;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds SqlParameter with optional configuration to DbCommand DbParametersCollection.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue, Action<SqlParameter> configureParam = null)
        {

            Guard.ThrowIfAnyIsNullOrEmpty(cmd.CommandText, paramName);
            Guard.ThrowIfNull(paramValue);

            var sqlParam = SqlParameterFactory.CreateParameter(paramName, paramValue);
            cmd.ConfigureParamAndAdd(sqlParam, configureParam);

            return cmd;
        }

        /// <summary>
        /// Adds SqlParameter with optional configuration to DbCommand DbParametersCollection.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, Action<SqlParameter> configureParam = null)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(paramName);

            var sqlParam = SqlParameterFactory.CreateParameter(paramName);
            cmd.ConfigureParamAndAdd(sqlParam, configureParam);

            return cmd;
        }

        /// <summary>
        /// Adds UserDefinedTableType SqlParameter with optional configuration to DbCommand DbParametersCollection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
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
