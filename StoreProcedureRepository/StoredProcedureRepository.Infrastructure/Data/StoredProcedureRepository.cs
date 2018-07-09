using System.Collections.Generic;
using System.Data.Entity;
using StoredProcedureRepository.Infrastructure.Services;
using StoreProcedureRepository.Abstract;

namespace StoredProcedureRepository.Infrastructure.Data
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly DbContext _context;

        public StoredProcedureRepository(DbContext context) => _context = context;

        public IEnumerable<T> Get<T>(string spName, object parameter)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = SqlParameterFactory.BuildParamsForObject(parameter);
            return _context
                .Database
                .SqlQuery<T>(StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, parameter), parameters);
        }

        public int ExecuteCommand(string spName, object parameter)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = SqlParameterFactory.BuildParamsForObject(parameter);
            return _context
                .Database
                .ExecuteSqlCommand(StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, parameter), parameters);
        }
    }
}
