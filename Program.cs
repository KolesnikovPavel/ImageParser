﻿using System;
using System.IO;

namespace ImageParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var parser = new ImageParser();
            string imageInfoJson;

            using (var file = new FileStream("image.png", FileMode.Open, FileAccess.Read))
            {
                imageInfoJson = parser.GetImageInfo(file);
            }

            Console.WriteLine(imageInfoJson);

            //using (var file = new FileStream("12.png", FileMode.Open, FileAccess.Read))
            //{
            //    imageInfoJson = parser.GetImageInfo(file);
            //}

            //Console.WriteLine(imageInfoJson);

            //using (var file = new FileStream("gif_image.gif", FileMode.Open, FileAccess.Read))
            //{
            //    imageInfoJson = parser.GetImageInfo(file);
            //}

            //Console.WriteLine(imageInfoJson);

            //using (var file = new FileStream("1.bmp", FileMode.Open, FileAccess.Read))
            //{
            //    imageInfoJson = parser.GetImageInfo(file);
            //}

            //Console.WriteLine(imageInfoJson);
        }
    }
}