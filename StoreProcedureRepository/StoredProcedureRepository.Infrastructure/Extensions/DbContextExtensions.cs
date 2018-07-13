using System.Data;
using System.Data.Common;
using System.Data.Entity;
using StoreProcedureRepository.Services;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static DbCommand LoadStoredProcedure(this DbContext context, string storedProcedureName)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(storedProcedureName);

            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }
    }
}
