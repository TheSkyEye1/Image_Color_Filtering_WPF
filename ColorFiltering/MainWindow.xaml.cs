using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.WpfExtensions;
using static System.Net.Mime.MediaTypeNames;

namespace ColorFiltering
{
    public partial class MainWindow : System.Windows.Window
    {
        Mat sourceMat;
        PixelSorterAlgorithm sorter = new PixelSorterAlgorithm();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void FilterImage_Click(object sender, RoutedEventArgs e)
        {
            //ResultImage.Source = sorter.sortImage(sourceMat).ToWriteableBitmap(PixelFormats.Bgr24);
            ResultImage.Source = sorter.sortImage2(sourceMat).ToWriteableBitmap(PixelFormats.Bgr24);
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.tiff";
            if (openFileDialog.ShowDialog() == true)
            {
                string FilePath = openFileDialog.FileName;


                Mat image = Cv2.ImRead(FilePath);
                Mat sortedImage = ImageSorter.SortPixelsByColor(image);

                ResultImage.Source = sortedImage.ToWriteableBitmap(PixelFormats.Bgr24);

                //Cv2.ImShow("Sorted Image", sortedImage);
                //Cv2.WaitKey();

                //sourceMat = Cv2.ImRead(FilePath, ImreadModes.Color);
                //OriginalImage.Source = sourceMat.ToWriteableBitmap(PixelFormats.Bgr24);
            }
        }

    }
}
