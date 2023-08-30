using Scrable.ui;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scrable
{
    class Screen
    {
        private Pixel[,] pixels;
        private ScreenBuffer buffer = new ScreenBuffer();
        public static int Width
        {
            get { return Console.WindowWidth; }
        }

        public static int Height
        {
            get { return Console.WindowHeight; }
        }

        public void Update()
        {
            this.pixels = new Pixel[Height, Width];
        }

        public void Clear()
        {
            this.pixels = new Pixel[pixels.GetLength(0), pixels.GetLength(1)];
        }

        public void Print()
        {
            Console.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (EndOfLine(y, x))
                        break;
                    buffer.Append(pixels[y, x]);
                }
                if (y < Height - 1)
                    buffer.Append('\n');
            }
            buffer.Flush();
        }

        private bool EndOfLine(int y, int x)
        {
            for (int c = x; c < Width; c++)
                if (pixels[y, c] != null && pixels[y, c] != Pixel.VOID)
                    return false;
            return true;
        }

        public Pixel PixelAt(int line, int row)
        {
            return pixels[line, row];
        }

        public void Draw(Pixel pixel, int line, int row)
        {
            this.pixels[line, row] = pixel;
        }

        public void Draw(string msg, int line, int row)
        {
            Draw(new ColoredString(msg), line, row);
        }

        public void Draw(Pixel[] pixels, int line, int row)
        {
            for (int i = 0; i < pixels.Length; i++)
                try
                {
                    Draw(pixels[i], line, row + i);
                }
                catch (IndexOutOfRangeException) { }
        }

        public void Draw(Pixel[,] p, int line, int row)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    try
                    {
                        this.pixels[line+i, row+j] = p[i, j];
                    }
                    catch (IndexOutOfRangeException) { }
            }
        }

        public void Draw(ColoredString text, int line, int row)
        {
            Draw(text.ToPixelArray(), line, row);
        }
    }
}
