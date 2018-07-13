using System.Data.Entity;
using StoredProcedureRepository.IntegrationTests.Entities;
using StoredProcedureRepository.IntegrationTests.EntitiesConfigurations;

namespace StoredProcedureRepository.IntegrationTests
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("StoredProcedureRepositoryConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
        }
    }
}
