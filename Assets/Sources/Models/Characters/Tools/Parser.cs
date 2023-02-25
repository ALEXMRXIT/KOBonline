using System;

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
    }
}