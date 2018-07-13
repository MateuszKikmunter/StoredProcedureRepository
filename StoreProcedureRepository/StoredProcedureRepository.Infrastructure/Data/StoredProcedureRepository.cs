using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using StoredProcedureRepository.Infrastructure.Services;
using StoreProcedureRepository.Abstract;
using StoreProcedureRepository.Services;

namespace StoredProcedureRepository.Infrastructure.Data
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly DbContext _context;

        public StoredProcedureRepository(DbContext context) => _context = context;

        public IEnumerable<T> Get<T>(string spName, object parameter = null)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = GetParametersForProcedure(parameter);
            return _context
                .Database
                .SqlQuery<T>(StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, parameter), parameters)
                .ToList();
        }

        public int ExecuteCommand(string spName, object parameter = null)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = GetParametersForProcedure(parameter);
            return _context
                .Database
                .ExecuteSqlCommand(StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, parameter), parameters);
        }

        private SqlParameter[] GetParametersForProcedure(object parameter)
        {
            return parameter == null
                ? new SqlParameter[] { }
                : SqlParameterFactory.BuildParamsForObject(parameter);
        }
    }
}
