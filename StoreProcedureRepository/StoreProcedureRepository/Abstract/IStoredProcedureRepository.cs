using System.Collections.Generic;

namespace StoreProcedureRepository.Abstract
{
    public interface IStoredProcedureRepository
    {
        /// <summary>
        /// Returns data for provided type by calling parametrized stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IEnumerable<T> Get<T>(string spName, object parameter = null);

        /// <summary>
        /// Performs DML/DDL parametrized query against database and returns the number of rows affected. 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int ExecuteCommand(string spName, object parameter = null);
    }
}
