using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using StoredProcedureRepository.IntegrationTests.Entities;

namespace StoredProcedureRepository.IntegrationTests.EntitiesConfigurations
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Name).IsRequired().HasMaxLength(50);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
