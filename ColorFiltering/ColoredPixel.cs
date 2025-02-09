using OpenCvSharp;

namespace ColorFiltering
{
    public class ColoredPixel
    {
        public Vec3b color { get; private set; }
        public int count { get; private set; }
        public ColoredPixel(Vec3b color)
        {
            this.color = color;
        }
        public bool compareColors(Vec3b col)
        {
            if(col.Item0 == color.Item0 && col.Item1 == color.Item1 && col.Item2 == color.Item2)
            {
                count++;
                return true;
            }
            return false;
        } 
    }
}
