using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StoredProcedureRepository.Infrastructure.Extensions;
using StoredProcedureRepository.Infrastructure.Services;
using StoreProcedureRepository.Abstract;

namespace StoredProcedureRepository.Infrastructure.Data
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly DbContext _context;

        public StoredProcedureRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Get<T>(string spName, object parameter)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = SqlParameterFactory.BuildParamsWhenUserDefinedTableType(parameter);
            return _context
                .Database
                .SqlQuery<T>(GetStoredProcedureNameWithParameters(spName, parameter), parameters);
        }

        public int ExecuteCommand(string spName, object parameter)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);

            var parameters = SqlParameterFactory.BuildParamsWhenUserDefinedTableType(parameter);
            return _context
                .Database
                .ExecuteSqlCommand(GetStoredProcedureNameWithParameters(spName, parameter), parameters);
        }

        private string GetStoredProcedureNameWithParameters(string spName, object param)
        {
            var names = param.GetType().GetProperties().Select(p => p.Name).ToList();
            names.ForEach(name => spName += $" @{name},");

            return spName.RemoveLastCharacter();
        }
    }
}
