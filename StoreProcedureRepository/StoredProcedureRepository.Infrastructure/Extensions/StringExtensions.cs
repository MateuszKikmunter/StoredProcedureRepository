using System;

namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLastCharacter(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input.Remove(input.Length - 1);
        }
    }
}
