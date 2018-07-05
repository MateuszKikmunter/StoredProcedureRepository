using System;
using System.Collections.Generic;
using System.Linq;

namespace StoredProcedureRepository.Infrastructure.Services
{
    public static class Guard
    {
        public static void ThrowIfNull(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public static void ThrowIfStringNullOrEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(input, nameof(input));
            }
        }

        public static void ThrowIfStringNullOrWhiteSpace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(input, nameof(input));
            }
        }

        public static void ThrowIfAnyIsNullOrEmpty(params string[] values)
        {
            if (values.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException(nameof(values));
            }
        }

        public static void ThrowIfEmpty<T>(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                throw new ArgumentException($"{ nameof(entities) } cannot be empty.");
            }
        }
    }
}
