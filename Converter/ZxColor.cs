using System;
using System.Drawing;

namespace _1bitToPng.Converter
{
    public class ZxColor
    {
        const int Difference = 64;
        const int Alpha = 255;

        public static Color[] BrightColor =
        {
            Color.Black,
            Color.Blue,
            Color.Red,
            Color.Magenta,
            Color.Green,
            Color.Cyan,
            Color.Yellow,
            Color.White
        };

        public static Color[] OtherColor =
        {
            Color.Black,
            Color.DarkBlue,
            Color.DarkRed,
            Color.DarkMagenta,
            Color.DarkGreen,
            Color.DarkCyan,
            Color.Yellow,
            Color.White
        };


        public static InkPaper GetColor(byte zxColor)
        {
            var inkB = zxColor & 0b00000111;
            var paperB = (zxColor & 0b00111000) >> 3;
            var brightB = zxColor & 0b01000000;

            var ink = BrightColor[inkB];
            var paper = BrightColor[paperB];


            if (brightB == 0)
            {
                ink = Color.FromArgb(
                    ink.A,
                    ink.R - Difference,
                    ink.G - Difference,
                    ink.B - Difference
                );

                paper = Color.FromArgb(
                    paper.A,
                    paper.R - Difference,
                    paper.G - Difference,
                    paper.B - Difference
                );
            }

            return new InkPaper(ink, paper);
        }
    }

    public struct InkPaper
    {
        public Color Ink;
        public Color Paper;

        public InkPaper(Color ink, Color paper)
        {
            Ink = ink;
            Paper = paper;
        }
    }
}