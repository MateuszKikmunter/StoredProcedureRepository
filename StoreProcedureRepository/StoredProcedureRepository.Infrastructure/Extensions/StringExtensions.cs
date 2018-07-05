namespace StoredProcedureRepository.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLastCharacter(this string input)
        {
            return input.Remove(input.Length - 1);
        }
    }
}
