using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

public class PixelData
{
    public Vec3b Color { get; set; }
    public int ColorGroup { get; set; }
    public double Luminosity { get; set; }

    public PixelData(Vec3b color, int colorGroup, double luminosity)
    {
        Color = color;
        ColorGroup = colorGroup;
        Luminosity = luminosity;
    }
}

public class ImageSorter
{
    public static Mat SortPixelsByColor(Mat source)
    {
        int rows = source.Rows;
        int cols = source.Cols;
        List<PixelData> pixels = new List<PixelData>();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vec3b bgr = source.At<Vec3b>(r, c);
                (double h, double s, double l) = RgbToHsl(bgr.Item2, bgr.Item1, bgr.Item0);

                double luminosity = (bgr.Item2 + bgr.Item1 + bgr.Item0) / 3.0;
                int colorGroup = GetColorGroup(bgr.Item2, bgr.Item1, bgr.Item0);

                pixels.Add(new PixelData(bgr, colorGroup, luminosity));
            }
        }

        pixels = pixels.OrderBy(p => p.ColorGroup).ThenByDescending(p => p.Luminosity).ToList();

        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                source.Set(r, c, pixels[index].Color);
                index++;
            }
        }

        return source;
    }

    private static (double, double, double) RgbToHsl(int r, int g, int b)
    {
        double rNorm = r / 255.0;
        double gNorm = g / 255.0;
        double bNorm = b / 255.0;

        double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
        double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
        double h, s, l = (max + min) / 2.0;

        if (max == min)
        {
            h = s = 0; 
        }
        else
        {
            double d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == rNorm)
                h = (gNorm - bNorm) / d + (gNorm < bNorm ? 6 : 0);
            else if (max == gNorm)
                h = (bNorm - rNorm) / d + 2;
            else
                h = (rNorm - gNorm) / d + 4;

            h /= 6.0;
        }

        return (h * 360.0, s * 100.0, l * 100.0);
    }

    private static int GetColorGroup(int r, int g, int b)
    {
        double threshold = 1.2;

        if (Math.Abs(r - g) < 10 && Math.Abs(g - b) < 10 && Math.Abs(r - b) < 10)
            return 0; // Grayscale
        else if (r > g * threshold && r > b * threshold)
            return 1; // Red
        else if (g > r * threshold && g > b * threshold)
            return 2; // Green
        else if (b > r * threshold && b > g * threshold)
            return 3; // Blue
        else if (r > b && g > b)
            return 4; // Yellow
        else if (r > g && b > g)
            return 5; // Magenta
        else if (g > r && b > r)
            return 6; // Cyan
        else
            return 7; // Mixed colors
    }
}
