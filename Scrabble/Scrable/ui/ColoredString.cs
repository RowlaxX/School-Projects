using System;
using System.Collections.Generic;
using System.Text;

namespace Scrable
{
    class ColoredString
    {
        public string Message { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public int Length
        {
            get { return Message.Length; }
        }

        public ColoredString()
        {
            this.Message = null;
            this.ForegroundColor = ConsoleColor.White;
            this.BackgroundColor = ConsoleColor.Black;
        }

        public ColoredString(string str)
        {
            this.Message = str;
            this.ForegroundColor = ConsoleColor.White;
            this.BackgroundColor = ConsoleColor.Black;
        }

        public ColoredString(string str, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.Message = str;
            this.ForegroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
        }

        public override string ToString()
        {
            return Message;
        }

        public Pixel[] ToPixelArray()
        {
            Pixel[] pixels = new Pixel[Message.Length];
            for (int i = 0; i < Message.Length; i++)
                pixels[i] = new Pixel(Message[i], this.BackgroundColor, this.ForegroundColor);
            return pixels;
        }

        public ColoredString Clone()
        {
            return new ColoredString(Message, ForegroundColor, BackgroundColor);
        }

        public void Write()
        {
            Console.BackgroundColor = this.BackgroundColor;
            Console.ForegroundColor = this.ForegroundColor;
            Console.Write(Message);
        }

        public void WriteLine()
        {
            Write();
            Console.WriteLine();
        }

        public char CharAt(int index)
        {
            return Message[index];
        }
    }
}
