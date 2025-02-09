
using System.Collections.Generic;
using OpenCvSharp;

namespace ColorFiltering
{
    public class PixelSorterAlgorithm
    {
        private List<ColoredPixel> PixelPallet;
        Mat sourceMat;
        private void CreatePallet()
        {
            PixelPallet = new List<ColoredPixel>();
            for (byte b = 0; b<= byte.MaxValue;  b++)
                for(byte g = 0; g<= byte.MaxValue; g++)
                    for (byte r = 0; r<= byte.MaxValue; r++)
                        PixelPallet.Add(new ColoredPixel(new Vec3b(b, g, r)));
        }
        private void RemoveEmptyPixels()
        {
            foreach(ColoredPixel pixel in PixelPallet)
                if(pixel.count == 0)
                    PixelPallet.Remove(pixel);
        }
        private void FillPallet()
        {
            for (int r = 0; r < sourceMat.Rows; r++)
                for (int c = 0; c < sourceMat.Cols; c++)
                    foreach (ColoredPixel pixel in PixelPallet)
                        if (pixel.compareColors(sourceMat.At<Vec3b>(r, c)))
                            break;
        }
        private void FillSortetImage()
        {
            int r = 0;
            int c = 0;

            foreach(ColoredPixel pixel in PixelPallet)
            {
                for(int i = 0; i < pixel.count; i++)
                {
                    sourceMat.At<Vec3b>(r, c) = pixel.color;
                    c++;
                    if(c > sourceMat.Cols)
                    {
                        r++;
                        c = 0;
                    }
                }
            }
        }

        public Mat sortImage(Mat source)
        {
            sourceMat = source;
            CreatePallet();
            FillPallet();
            RemoveEmptyPixels();
            FillSortetImage();
            return sourceMat;
        }
    }
}
