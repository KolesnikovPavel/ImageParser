using System.IO;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Collections.Generic;

namespace ImageParser
{
    public class ImageParser : IImageParser
    {
        class Image
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public string Format { get; set; }
            public int Size { get; set; }
        }

        public static string GetHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string DisplayGifInfo(byte[] bytes, Stream stream)
        {
            Image imageInfo = new Image()
            {
                Height = bytes[8] | bytes[9] << 8,
                Width = bytes[6] | bytes[7] << 8,
                Format = Encoding.ASCII.GetString(bytes, 0, 3),
                Size = (int)stream.Length
            };
            return JsonConvert.SerializeObject(imageInfo);
        }

        public static string DisplayPngInfo(byte[] bytes, Stream stream)
        {
            string widthHexValue = GetHex(bytes).Remove(0,32).Remove(8);
            string heightHexValue = GetHex(bytes).Remove(0, 40).Remove(8);
            Image imageInfo = new Image()
            {
                Height = Convert.ToInt32(heightHexValue, 16),
                Width = Convert.ToInt32(widthHexValue, 16),
                Format = "Png",
                Size = (int)stream.Length
            };
            return JsonConvert.SerializeObject(imageInfo);
        }

        public static string ConvertHexToTheCorrectFormat(string hexValue)
        {
            int i = 0;
            List<string> hexValueList = new List<string>();
            while (i < hexValue.Length)
            {
                hexValueList.Add(hexValue.Substring(i, 2));
                i = i + 2;
            }
            return hexValueList[3] + hexValueList[2] + hexValueList[1] + hexValueList[0];
        }

        public static string DisplayBmpInfo(byte[] bytes, Stream stream)
        {
            string widthHexValue = GetHex(bytes).Remove(0, 36).Remove(8);
            string heightHexValue = GetHex(bytes).Remove(0, 44).Remove(8);
            Image imageInfo = new Image()
            {
                Height = Convert.ToInt32(ConvertHexToTheCorrectFormat(heightHexValue), 16),
                Width = Convert.ToInt32(ConvertHexToTheCorrectFormat(widthHexValue), 16),
                Format = "bmp",
                Size = (int)stream.Length
            };
            return JsonConvert.SerializeObject(imageInfo);
        }

        public string GetImageInfo(Stream stream)
        {
            string imageInfo = null;
            byte[] bytes = new byte[50];
            stream.Read(bytes, 0, 50);
            string type = Encoding.ASCII.GetString(bytes, 0, 4);
            if (type.ToLower().Contains("gif"))
                imageInfo = DisplayGifInfo(bytes, stream);
            if (type.ToLower().Contains("png"))
                imageInfo = DisplayPngInfo(bytes, stream);
            if (type.ToLower().Contains("bm"))
                imageInfo = DisplayBmpInfo(bytes, stream);
            return imageInfo;
        }
    }
}