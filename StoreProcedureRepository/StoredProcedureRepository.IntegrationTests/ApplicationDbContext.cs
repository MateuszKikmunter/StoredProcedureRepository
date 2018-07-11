using System.Data.Entity;

namespace StoredProcedureRepository.IntegrationTests
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("StoredProcedureRepository")
        {
        }
    }
}
