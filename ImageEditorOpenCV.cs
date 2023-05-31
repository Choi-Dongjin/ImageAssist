using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageAssist
{
    internal class ImageEditorOpenCV
    {
        private Mat image;

        public ImageEditorOpenCV(string filePath)
        {
            this.image = new Mat(filePath, ImreadModes.Color);
        }

        public void Resize(int width, int height)
        {
            Cv2.Resize(image, image, new Size(width, height), 0, 0, InterpolationFlags.Linear);
        }

        public void Rotate(double angle)
        {
            Point2f center = new Point2f(image.Cols / 2, image.Rows / 2);
            Mat rot = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Cv2.WarpAffine(image, image, rot, image.Size());
        }

        public void Crop(int x, int y, int width, int height)
        {
            Rect cropRect = new Rect(x, y, width, height);
            image = new Mat(image, cropRect);
        }

        public Point[][] FindContours()
        {
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, gray, 128, 255, ThresholdTypes.Binary);

            Point[][] contours;
            using (Mat hierarchy = new Mat())
            {
                Cv2.FindContours(
                    gray,
                    out contours,
                    out _,
                    RetrievalModes.External,
                    ContourApproximationModes.ApproxTC89KCOS
                );
            }

            return contours;
        }

        public double CalculateAverageBrightness()
        {
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            Scalar avg = Cv2.Mean(gray);
            return avg.Val0;
        }

        public void BilateralFilter(int diameter, double sigmaColor, double sigmaSpace)
        {
            Cv2.BilateralFilter(image, image, diameter, sigmaColor, sigmaSpace, BorderTypes.Reflect101);
        }

        public void GaussianBlur(Size size, double sigmaX, double sigmaY)
        {
            Cv2.GaussianBlur(image, image, size, sigmaX, sigmaY, BorderTypes.Reflect101);
        }

        public void MedianBlur(int ksize)
        {
            Cv2.MedianBlur(image, image, ksize);
        }

        public void Save(string filePath)
        {
            Cv2.ImWrite(filePath, image);
        }
    }
}