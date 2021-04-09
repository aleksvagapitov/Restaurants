using System;

namespace Application.Restaurants
{
    public class RestaurantInfo
    {
        public TimeZoneInfo tz;

        public bool IsOpenAt(TimeSpan time, TimeSpan open, TimeSpan close)
        {
            TimeZoneInfo local = TimeZoneInfo.Local;
            TimeSpan offset = TimeZoneInfo.Local.BaseUtcOffset;

            // Is the store in the same time zone?
            if (tz.Equals(local))
            {
                return time >= open & time <= close;
            }
            else
            {
                TimeSpan delta = TimeSpan.Zero;
                TimeSpan storeDelta = TimeSpan.Zero;

                // Is it daylight saving time in either time zone?
                if (local.IsDaylightSavingTime(DateTime.Now.Date + time))
                    delta = local.GetAdjustmentRules()[local.GetAdjustmentRules().Length - 1].DaylightDelta;

                if (tz.IsDaylightSavingTime(TimeZoneInfo.ConvertTime(DateTime.Now.Date + time, local, tz)))
                    storeDelta = tz.GetAdjustmentRules()[tz.GetAdjustmentRules().Length - 1].DaylightDelta;

                TimeSpan comparisonTime = time + (offset - tz.BaseUtcOffset).Negate() + (delta - storeDelta).Negate();
                return comparisonTime >= open & comparisonTime <= close;
            }
        }
    }
}