using System;
using UnityEngine;

namespace Assets.Sources.Tools
{
    public static class ConvertHEXtoInt32
    {
        public static Color ConvertHEX(string hex, int defaultAlphaChannel = 255)
        {
            int r = Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = Convert.ToInt32(hex.Substring(5, 2), 16);

            return new Color(r, g, b, defaultAlphaChannel);
        }
    }
}