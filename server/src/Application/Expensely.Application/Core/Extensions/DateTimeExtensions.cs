using System;

namespace Expensely.Application.Core.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the start of the week based on the specified start of week day.
        /// </summary>
        /// <param name="dateTime">The date time instance.</param>
        /// <param name="startOfWeek">The day of week representing the start of week.</param>
        /// <returns>The date and time of the first day of the week based on the specified start of week day.</returns>
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int decrementDays = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;

            return dateTime.AddDays(-decrementDays).Date;
        }

        /// <summary>
        /// Gets the start of the week assuming the first day of the week is <see cref="DayOfWeek.Monday"/>.
        /// </summary>
        /// <param name="dateTime">The date time instance.</param>
        /// <returns>The date and time of the first day of the week.</returns>
        public static DateTime StartOfWeek(this DateTime dateTime) => dateTime.StartOfWeek(DayOfWeek.Monday);
    }
}
