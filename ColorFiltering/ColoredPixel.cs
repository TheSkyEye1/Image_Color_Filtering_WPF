using OpenCvSharp;

namespace ColorFiltering
{
    public class ColoredPixel
    {
        public Vec3b Color { get; private set; }
        public int Num { get; private set; }
        public int Count { get; private set; }
        public ColoredPixel(int num, Vec3b color)
        {
            this.Num = num;
            this.Color = color;
        }
        public bool compareColors(Vec3b col)
        {
            if(col.Item0 == Color.Item0 && col.Item1 == Color.Item1 && col.Item2 == Color.Item2)
            {
                Count++;
                return true;
            }
            return false;
        } 
    }
}
