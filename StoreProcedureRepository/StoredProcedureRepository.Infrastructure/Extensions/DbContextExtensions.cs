using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static DbCommand LoadStoredProcedure(this DbContext context, string storeProcedureName)
        {
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandText = storeProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }
    }
}
