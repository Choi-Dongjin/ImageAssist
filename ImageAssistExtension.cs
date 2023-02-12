using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace ImageAssist
{
    public class ImageAssistExtension
    {
        public static (Point, Point)? ExtractCoordinatesMaskImage(string imPath)
        {
            using Mat matIm = Cv2.ImRead(imPath);
            return ExtractCoordinatesMaskImage(matIm);
        }

        public static (Point, Point)? ExtractCoordinatesMaskImage(string imPath, out Mat? outMat)
        {
            using Mat matIm = Cv2.ImRead(imPath);
            return ExtractCoordinatesMaskImage(matIm, out outMat, new Scalar(0, 0, 200), new Scalar(50, 50, 255));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imPath"></param>
        /// <param name="outMat"></param>
        /// <param name="minRange"> BGR </param>
        /// <param name="maxRange"> BGR </param>
        /// <returns></returns>
        public static (Point, Point)? ExtractCoordinatesMaskImage(string imPath, out Mat? outMat, Scalar minRange, Scalar maxRange)
        {
            using Mat matIm = Cv2.ImRead(imPath);
            return ExtractCoordinatesMaskImage(matIm, out outMat, minRange, maxRange);
        }

        public static (Point, Point)? ExtractCoordinatesMaskImage(Mat simRImage)
        {
            Mat simROIImage = new();
            Cv2.InRange(simRImage, new Scalar(0, 0, 200), new Scalar(50, 50, 255), simROIImage);
            Cv2.Threshold(simROIImage, simROIImage, 127, 255, ThresholdTypes.Binary);

            Cv2.FindContours(simROIImage, out Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            List<int> xList = new();
            List<int> yList = new();

            int p1X;
            int p2X;
            int p1Y;
            int p2Y;
            try
            {
                List<Point[]> new_contours = new();
                foreach (Point[] p in contours)
                {
                    foreach (Point p2 in p)
                    {
                        xList.Add(p2.X);
                        yList.Add(p2.Y);
                    }

                    double length = Cv2.ArcLength(p, true);
                    if (length > 100)
                    {
                        new_contours.Add(p);
                    }
                }

                p1X = xList.Min();
                p2X = xList.Max();
                p1Y = yList.Min();
                p2Y = yList.Max();
            }
            catch
            {
                return null;
            }

            simROIImage.Dispose();
            return (new Point(p1X, p1Y), new Point(p2X, p2Y));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="simRImage"></param>
        /// <param name="outMat"></param>
        /// <param name="minRange"> BGR </param>
        /// <param name="maxRange"> BGR </param>
        /// <returns></returns>
        public static (Point, Point)? ExtractCoordinatesMaskImage(Mat simRImage, out Mat? outMat, Scalar minRange, Scalar maxRange)
        {
            //Cv2.PyrDown(simRImage, simRImage);

            Mat simROIImage = new();
            Cv2.InRange(simRImage, minRange, maxRange, simROIImage);
            Cv2.Threshold(simROIImage, simROIImage, 127, 255, ThresholdTypes.Binary);

            Cv2.FindContours(simROIImage, out Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            List<int> xList = new();
            List<int> yList = new();

            int p1X;
            int p2X;
            int p1Y;
            int p2Y;
            try
            {
                List<Point[]> new_contours = new();
                foreach (Point[] p in contours)
                {
                    foreach (Point p2 in p)
                    {
                        xList.Add(p2.X);
                        yList.Add(p2.Y);
                    }

                    double length = Cv2.ArcLength(p, true);
                    if (length > 100)
                    {
                        new_contours.Add(p);
                    }
                }

                p1X = xList.Min();
                p2X = xList.Max();
                p1Y = yList.Min();
                p2Y = yList.Max();
            }
            catch
            {
                outMat = null;
                return null;
            }

            outMat = simRImage.Clone();
            Cv2.Rectangle(outMat, new Point(p1X, p1Y), new Point(p2X, p2Y), Scalar.GreenYellow, 2, LineTypes.AntiAlias);
            simROIImage.Dispose();

            return (new Point(p1X, p1Y), new Point(p2X, p2Y));
        }
    }
}