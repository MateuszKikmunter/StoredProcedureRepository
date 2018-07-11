using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using StoredProcedureRepository.Infrastructure.Services;
using StoreProcedureRepository.Services;

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

        /// <summary>
        /// Executes stored procedure and and returns the number of rows affected. 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int ExecuteStoredProceure(this DbCommand cmd)
        {
            using (cmd)
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                    
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes stored procedure and returns collection of entities with specified data type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IList<T> ExecuteStoredProcedure<T>(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.MapToList<T>();
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        private static void ConfigureParamAndAdd(this DbCommand cmd, SqlParameter param, Action<SqlParameter> configureParam)
        {
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
        }
    }
}
