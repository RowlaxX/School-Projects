using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    class Pixel
    {
        public static Pixel VOID = new Pixel(' ', ConsoleColor.Black, ConsoleColor.White);
        public static void Write(Pixel pixel)
        {
            if (pixel == null)
            {
                if (Console.BackgroundColor != ConsoleColor.Black)
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(' ');
            }
            else
                pixel.Write();
        }

        public char Char { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public Pixel(char Char, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            this.Char = Char;
            this.BackgroundColor = backgroundColor;
            this.ForegroundColor = foregroundColor;
        }

        public void Write()
        {
            if (this.BackgroundColor != Console.BackgroundColor)
                Console.BackgroundColor = this.BackgroundColor;
            if (this.ForegroundColor != Console.ForegroundColor)
                Console.ForegroundColor = this.ForegroundColor;
            Console.Write(Char);
        }
    }
}
