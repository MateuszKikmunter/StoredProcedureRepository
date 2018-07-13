using StoreProcedureRepository.Services;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLastCharacter(this string input)
        {
            Guard.ThrowIfStringNullOrWhiteSpace(input);

            return input.Remove(input.Length - 1);
        }
    }
}
