
using System.Collections.Generic;
using OpenCvSharp;
using static OpenCvSharp.LineIterator;

namespace ColorFiltering
{
    public class PixelSorterAlgorithm
    {
        private List<ColoredPixel> PixelPallet;
        Mat sourceMat;

        //Вот тут не работает
        private void CreatePallet()
        {
            PixelPallet = new List<ColoredPixel>();
            for (byte b = 0; b<= byte.MaxValue;  b++)
                for(byte g = 0; g<= byte.MaxValue; g++)
                    for (byte r = 0; r<= byte.MaxValue; r++)
                        PixelPallet.Add(new ColoredPixel(0, new Vec3b(b, g, r)));
        }
        private void RemoveEmptyPixels()
        {
            foreach(ColoredPixel pixel in PixelPallet)
                if(pixel.Count == 0)
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
                for(int i = 0; i < pixel.Count; i++)
                {
                    sourceMat.At<Vec3b>(r, c) = pixel.Color;
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


        //Вот тут работает
        private void FillPaletteByPixelsFromImage()
        {
            PixelPallet = new List<ColoredPixel>(sourceMat.Rows * sourceMat.Cols);

            for (int r = 0; r < sourceMat.Rows; r++)
            {
                for (int c = 0; c < sourceMat.Cols; c++)
                {
                    Vec3b bgr = sourceMat.At<Vec3b>(r, c);
                    int colorValue = (bgr.Item2 << 16) | (bgr.Item1 << 8) | bgr.Item0;
                    PixelPallet.Add(new ColoredPixel(colorValue, bgr));
                }
            }

            PixelPallet.Sort((p1, p2) => p2.Num.CompareTo(p1.Num));
        }

        private void FillImageFromPalette()
        {
            int i = 0;
            for (int r = 0; r < sourceMat.Rows; r++)
            {
                for (int c = 0; c < sourceMat.Cols; c++)
                {
                    sourceMat.At<Vec3b>(r, c) = PixelPallet[i].Color;
                    i++;
                }
            }
        }

        private void FillPaletteByHSV()
        {
            PixelPallet = new List<ColoredPixel>();

            for (int r = 0; r < sourceMat.Rows; r++)
            {
                for (int c = 0; c < sourceMat.Cols; c++)
                {
                    Vec3b bgr = sourceMat.At<Vec3b>(r, c);

                    Mat singlePixelMat = new Mat(1, 1, MatType.CV_8UC3);
                    singlePixelMat.Set(0, 0, bgr);

                    Cv2.CvtColor(singlePixelMat, singlePixelMat, ColorConversionCodes.BGR2HSV);

                    Vec3b hsvVec = singlePixelMat.At<Vec3b>(0, 0);

                    double hue = hsvVec.Item0;  
                    double saturation = hsvVec.Item1; 
                    double value = hsvVec.Item2;

                    double colorValue = hue * 1000000 + saturation * 1000 + value;
                    PixelPallet.Add(new ColoredPixel((int)colorValue, bgr));
                }
            }

            PixelPallet.Sort((p1, p2) => p1.Num.CompareTo(p2.Num));

            int i = 0;
            for (int r = 0; r < sourceMat.Rows; r++)
            {
                for (int c = 0; c < sourceMat.Cols; c++)
                {
                    sourceMat.At<Vec3b>(r, c) = PixelPallet[i].Color;
                    i++;
                }
            }
        }


        public Mat sortImage2(Mat source)
        {
            sourceMat = source;
            FillPaletteByPixelsFromImage();
            FillImageFromPalette();
            return sourceMat;
        }

        public Mat SortImageByHSV(Mat source)
        {
            sourceMat = source;
            FillPaletteByHSV();
            FillImageFromPalette();
            return sourceMat;
        }

    }
}
