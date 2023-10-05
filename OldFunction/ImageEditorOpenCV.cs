using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace ImageAssist.OldFunction
{
    public class ImageEditorOpenCV
    {
        private Mat _image;

        public Mat GetImage { get { return _image; } }

        public ImageEditorOpenCV(string filePath)
        {
            _image = new Mat(filePath, ImreadModes.Color);
        }

        public void Resize(int width, int height)
        {
            Cv2.Resize(_image, _image, new Size(width, height), 0, 0, InterpolationFlags.Linear);
        }

        public void Rotate(double angle)
        {
            Point2f center = new Point2f(_image.Cols / 2, _image.Rows / 2);
            Mat rot = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Cv2.WarpAffine(_image, _image, rot, _image.Size());
        }

        public void Crop(int x, int y, int width, int height)
        {
            Rect cropRect = new Rect(x, y, width, height);
            _image = new Mat(_image, cropRect);
        }

        public Point[][] FindContours()
        {
            Mat gray = new Mat();
            Cv2.CvtColor(_image, gray, ColorConversionCodes.BGR2GRAY);
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
            Cv2.CvtColor(_image, gray, ColorConversionCodes.BGR2GRAY);
            Scalar avg = Cv2.Mean(gray);
            return avg.Val0;
        }

        public void BilateralFilter(int diameter, double sigmaColor, double sigmaSpace)
        {
            Cv2.BilateralFilter(_image, _image, diameter, sigmaColor, sigmaSpace, BorderTypes.Reflect101);
        }

        public void GaussianBlur(Size size, double sigmaX, double sigmaY)
        {
            Cv2.GaussianBlur(_image, _image, size, sigmaX, sigmaY, BorderTypes.Reflect101);
        }

        public void MedianBlur(int ksize)
        {
            Cv2.MedianBlur(_image, _image, ksize);
        }

        public void Save(string filePath)
        {
            Cv2.ImWrite(filePath, _image);
        }
    }
}