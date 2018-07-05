using System.Collections.Generic;

namespace StoreProcedureRepository.Abstract
{
    public interface IStoredProcedureRepository
    {
        /// <summary>
        /// Returns data for provided type by calling parametrized stored procedure.
        /// Internally creates SqlParameter for every property in provided object. 
        /// Works with user defined table types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IEnumerable<T> Get<T>(string spName, object parameter);

        /// <summary>
        /// Performs DML/DDL parametrized query against database. 
        /// Works with user defined table types.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        int ExecuteCommand(string spName, object parameter);
    }
}
