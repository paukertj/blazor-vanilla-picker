using System;
using System.Drawing;

namespace Blazor.VanillaPicker
{
    internal static class Extensions
    {
        public static string ToRgba(this Color color)
        {
            return $"rgba({color.R}, {color.G}, {color.B}, {color.A.ToAlpha()})";
        }

        public static byte ToAlpha(this double a)
        {
            byte alpha = 1;

            a = a < 0 ? 0 : a;

            if (a <= 1)
            {
                alpha = Convert.ToByte(a * 255);
            }

            return alpha;
        }

        public static double ToAlpha(this byte b)
        {
            if (b == byte.MaxValue)
            {
                return 1;
            }

            if (b == byte.MinValue)
            {
                return 0;
            }

            return (double)b / byte.MaxValue;
        }
    }
}
