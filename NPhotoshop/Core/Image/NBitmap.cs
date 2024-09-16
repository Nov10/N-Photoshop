using Microsoft.UI.Xaml.Media.Imaging;
using NPhotoshop.Core.Layers;
using System.Runtime.InteropServices.WindowsRuntime;

namespace NPhotoshop.Core.Image
{
    public class NBitmap
    {
        public WriteableBitmap ConvertToBitmap()
        {
            WriteableBitmap b = new WriteableBitmap(Width, Height);
            // Bitmap 버퍼에 접근
            using (var stream = b.PixelBuffer.AsStream())
            {
                // 버퍼는 BGRA 순서로 데이터를 가짐
                byte[] pixelData = new byte[Width * Height * 4];

                for (int i = 0; i < Pixels.Length; i++)
                {
                    pixelData[i * 4] = Pixels[i].B; // Blue
                    pixelData[i * 4 + 1] = Pixels[i].G; // Green
                    pixelData[i * 4 + 2] = Pixels[i].R; // Red
                    pixelData[i * 4 + 3] = Pixels[i].A; // Alpha
                }

                stream.Write(pixelData, 0, pixelData.Length);
            }
            return b;
        }
        public void Clear()
        {
            LoopForPixel((x, y) =>
            {
                SetPixel(x, y, new Color(0, 0, 0, 255));
            });
        }
        public static NBitmap CreateWhiteNBitmap(int width, int height)
        {
            NBitmap map = new NBitmap(width, height);
            map.LoopForPixel((x, y) =>
            {
                map.SetPixel(x, y, new Color(255, 255, 255, 255));
            });
            return map;
        }

        public Color[] Pixels;
        public int Width;
        public int Height;

        public NBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Color[Width * Height];
        }

        public void SetPixel(int x, int y, Color c)
        {
            if (x + y * Width >= Pixels.Length)
                return;
            Pixels[x + y * Width] = c;
        }
        public Color GetPixel(int x, int y)
        {
            return Pixels[x + y * Width];
        }

        public void LoopForPixel(System.Action<int, int> loop)
        {
            System.Threading.Tasks.Parallel.For(0, Width * Height, (i) =>
            {
                int y = i / Height;
                int x = i % Height;
                loop(x, y);
            });
            //for (int x = 0; x < Width; x++)
            //{
            //    for (int y = 0; y < Height; y++)
            //    {
            //        loop(x, y);
            //    }
            //}
        }
    }
}