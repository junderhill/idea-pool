using System;

namespace MyIdeaPool
{
    public static class DateTimeExtensions
    {
        public static int ToUnixEpoch(this DateTime dateTime)
        {
            return (int) dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}