using System;
using System.Text;

namespace Assets.Sources.Models.Characters.Tools
{
    public static class Parser
    {
        public static int[] SplitIntToMoney(int value)
        {
            int[] buffer = new int[3];

            for (int iterator = 0; iterator < buffer.Length; iterator++)
            {
                int powerIndex = iterator == 0 ? 1 : (iterator ==
                    buffer.Length - 1 ? buffer.Length : iterator);

                int valueSpliting = (int)Math.Pow(100, powerIndex);

                buffer[iterator] = value % valueSpliting;
                value /= valueSpliting;
            }

            return buffer;
        }

        public static string ConvertTimeInt32ToTimeString(int value)
        {
            StringBuilder stringBuilder = new StringBuilder(capacity: 8);
            int time = 0;

            if ((value / 60) >= 1)
            {
                if ((time = (value / (60 * 60))) >= 1)
                    return stringBuilder.AppendFormat("{0} h", time).ToString();

                time = (value / 60);
                return stringBuilder.AppendFormat("{0} m", time).ToString();
            }

            return stringBuilder.AppendFormat("{0} s", value).ToString();
        }

        public static string ConvertTimeSpanToTimeString(TimeSpan timeSpan)
        {
            string hours = timeSpan.Hours < 10 ? $"0{timeSpan.Hours}" : timeSpan.Hours.ToString();
            string minutes = timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes.ToString();
            string seconds = timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds.ToString();

            return $"{hours}:{minutes}:{seconds}";
        }
    }
}