using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrable.ui
{
    class ScreenBuffer
    {
        private static int Size{
            get { return Screen.Width * 3; }
        }

        private StringBuilder sb = new StringBuilder(Size);
        private ConsoleColor background = Console.BackgroundColor;
        private ConsoleColor foreground = Console.ForegroundColor;
        private BufferedStream bs = new BufferedStream(Console.OpenStandardOutput(), 0x15000);

        public void Append(Pixel pixel)
        {
            if (pixel == null)
                pixel = Pixel.VOID;

            if (background == pixel.BackgroundColor)
            {
                if (foreground != pixel.ForegroundColor) 
                {
                    if (pixel.Char == ' ')
                        Append(' ');
                    else
                    {
                        Flush();
                        background = pixel.BackgroundColor;
                        foreground = pixel.ForegroundColor;
                        Append(pixel.Char);
                    }
                }
                else
                    Append(pixel.Char);
            }
            else
            {
                Flush();
                background = pixel.BackgroundColor;
                foreground = pixel.ForegroundColor;
                Append(pixel.Char);
            }
        }

        public void Append(char Char)
        {
            sb.Append(Char);
        }

        public void Flush()
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            
            string s = sb.ToString();
            var rgb = new byte[s.Length << 1];
            Encoding.Unicode.GetBytes(s, 0, s.Length, rgb, 0);
            bs.Write(rgb, 0, rgb.Length);
            bs.Flush();

            this.sb.Clear();
        }
    }
}
