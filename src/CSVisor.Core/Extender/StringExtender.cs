using CSVisor.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSVisor.Core.Extender
{
    public static class StringExtender
    {
        #region [IsNull]
        /// <summary>
        /// Checks if the provided string is null.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>True if the string is null, otherwise false.</returns>
        public static bool IsNull(this string value) => value is null;
        #endregion

        #region [IsNotNull]
        /// <summary>
        /// Checks if the provided string is not null.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is not null, otherwise false.</returns>
        public static bool IsNotNull(this string value) => !IsNull(value);
        #endregion

        #region [IsEmpty]
        /// <summary>
        /// Checks if the provided string is empty.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if string is empty, otherwise false.</returns>
        public static bool IsEmpty(this string value) => value.Equals(string.Empty);
        #endregion

        #region [IsNotEmpty]
        /// <summary>
        /// Checks if the provided string is not empty.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if string is not empty, otherwise false.</returns>
        public static bool IsNotEmpty(this string value) => !IsEmpty(value);
        #endregion

        #region [ContainsOnlyWhitespaces]
        /// <summary>
        /// Checks if the provided string only contains whitespaces.
        /// </summary>
        /// <param name="value">The provieded string.</param>
        /// <returns>True if the string contains only whitespaces, otherwise false.</returns>
        public static bool ContainsOnlyWhitespaces(this string value) => value.All(x => x.Equals(' '));
        #endregion

        #region [ContainsNotOnlyWhitespaces]
        /// <summary>
        /// Checks if the provided string only contains whitespaces.
        /// </summary>
        /// <param name="value">The provieded string.</param>
        /// <returns>True if the string does not contains only whitespaces, otherwise false.</returns>
        public static bool ContainsNotOnlyWhitespaces(this string value) => value.Any(x => !x.Equals(' '));
        #endregion

        #region [IsNullOrEmpty]
        /// <summary>
        /// Checks if the provided string is null or empty.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is null or empty, otherwise false.</returns>
        public static bool IsNullOrEmpty(this string value) => IsNull(value) || IsEmpty(value);
        #endregion

        #region [IsNotNullOrEmpty]
        /// <summary>
        /// Checks if the provided string is not null nor empty.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is neither null nor empty, otherwise false.</returns>
        public static bool IsNotNullNorEmpty(this string value) => IsNotNull(value) && IsNotEmpty(value);
        #endregion

        #region [IsNullOrEmptyOrWhitespace]
        /// <summary>
        /// Checks if the provided string is null or empty or contains only whitespaces.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is null or empty or contains only whitespaces, otherwise false.</returns>
        public static bool IsNullOrEmptyOrWhitespace(this string value) => IsNull(value) || IsEmpty(value) || ContainsOnlyWhitespaces(value);
        #endregion

        #region [IsNotNullOrEmptyOrWhitespace]
        /// <summary>
        /// Checks if the provided string is not null nor empty nor contains only whitespaces.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is neither null nor empty nor only whitespaces, otherwise false.</returns>
        public static bool IsNotNullNorEmptyNorWhitespace(this string value) => IsNotNull(value) && IsNotEmpty(value) && ContainsNotOnlyWhitespaces(value);
        #endregion

        #region [IsDateTimeString]
        /// <summary>
        /// Checks if the provided string is a date time string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is a date time string, otherwise false.</returns>
        public static bool IsDateTimeString(this string value) => DateTime.TryParse(value, out _);
        /// <summary>
        /// Checks if the provided string is a date time string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if the string is a date time string, otherwise false.</returns>
        internal static bool IsDateTimeString(this string value, out DateTime result) => DateTime.TryParse(value, out result);
        #endregion

        #region [ToDateTime]
        /// <summary>
        /// Converts a string to date time.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>The date time object.</returns>
        /// <exception cref="ArgumentNullOrEmptyOrWhitespaceException">Thrown when the provided string is null or empty or contains only whitespaces.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is not a date time string.</exception>
        public static DateTime ToDateTime(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullOrEmptyOrWhitespaceException(nameof(value));

            if (!value.IsDateTimeString(out var result))
                throw new ArgumentException("The provided string is not a date time string.");

            return result;
        }
        #endregion

        #region [IsDateString]
        /// <summary>
        /// Checks if the provided string is a date string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is a date string, otherwise false.</returns>
        public static bool IsDateString(this string value) => DateOnly.TryParse(value, out _);
        /// <summary>
        /// Checks if the provided string is a date string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <param name="result">The date only object.</param>
        /// <returns>True if the string is a date string, otherwise false.</returns>
        internal static bool IsDateString(this string value, out DateOnly result) => DateOnly.TryParse(value, out result);
        #endregion

        #region [ToDate]
        /// <summary>
        /// Converts a string to date.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>The date object.</returns>
        /// <exception cref="ArgumentNullOrEmptyOrWhitespaceException">Thrown when the provided string is null or empty or contains only whitespaces.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is not a date string.</exception>
        public static DateOnly ToDate(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullOrEmptyOrWhitespaceException(nameof(value));

            if (!value.IsDateString(out var result))
                throw new ArgumentException("The provided string is not a date time string.");

            return result;
        }
        #endregion

        #region [IsDateString]
        /// <summary>
        /// Checks if the provided string is a time string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is a time string, otherwise false.</returns>
        public static bool IsTimeString(this string value) => TimeOnly.TryParse(value, out _);
        /// <summary>
        /// Checks if the provided string is a time string.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <param name="result">The time only object.</param>
        /// <returns>True if the string is a time string, otherwise false.</returns>
        internal static bool IsTimeString(this string value, out TimeOnly result) => TimeOnly.TryParse(value, out result);
        #endregion

        #region [ToTime]
        /// <summary>
        /// Converts a string to time.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>The time object.</returns>
        /// <exception cref="ArgumentNullOrEmptyOrWhitespaceException">Thrown when the provided string is null or empty or contains only whitespaces.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is not a time string.</exception>
        public static TimeOnly ToTime(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullOrEmptyOrWhitespaceException(nameof(value));

            if (!value.IsTimeString(out var result))
                throw new ArgumentException("The provided string is not a date time string.");

            return result;
        }
        #endregion

        #region [IsGuidString]
        /// <summary>
        /// Checks if the provided string is a guid.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>True if the string is a guid, otherwise false.</returns>
        public static bool IsGuidString(this string value) => Guid.TryParse(value, out _);
        internal static bool IsGuidString(this string value, out Guid result) => Guid.TryParse(value, out result);
        #endregion

        #region [ToGuid]
        /// <summary>
        /// Converts a string to guid.
        /// </summary>
        /// <param name="value">The provided string.</param>
        /// <returns>The guid.</returns>
        /// <exception cref="ArgumentNullOrEmptyOrWhitespaceException">Thrown when the provided string is null or empty or contains only whitespaces.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is not a guid.</exception>
        public static Guid ToGuid(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullOrEmptyOrWhitespaceException(nameof(value));

            if (!value.IsGuidString(out var result))
                throw new ArgumentException("The provided string is not a guid.");

            return result;
        }
        #endregion
    }
}
