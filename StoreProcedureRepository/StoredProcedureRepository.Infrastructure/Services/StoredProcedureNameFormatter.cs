using System.Linq;
using StoredProcedureRepository.Infrastructure.Extensions;

namespace StoredProcedureRepository.Infrastructure.Services
{
    public static class StoredProcedureNameFormatter
    {
        public static string GetStoredProcedureNameWithParameters(string spName, object param)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(spName);
            Guard.ThrowIfNull(param);

            var names = param.GetType().GetProperties().Select(p => p.Name).ToList();
            names.ForEach(name => spName += $" @{name},");

            return spName.RemoveLastCharacter();
        }
    }
}
