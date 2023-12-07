using OpenCvSharp;
using System.Runtime.InteropServices;
using Point = OpenCvSharp.Point;

namespace ImageAssist.LOpenCV
{
    public class Contour
    {
        /// <summary>
        /// 윤곽선 따기
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static Point[][] FindAndSortContoursByArea(Mat channel, double thresh, double maxval)
        {
            // 이진화를 위한 임계값 처리
            Cv2.Threshold(channel, channel, thresh, maxval, ThresholdTypes.Binary);
            // 윤곽선 찾기
            Cv2.FindContours(channel, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            // 윤곽선을 면적에 따라 정렬
            var sortedContours = contours.OrderByDescending(c => Cv2.ContourArea(c)).ToArray();
            return sortedContours;
        }

        /// <summary>
        /// 가장큰 윤곽선 따기
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static Point[]? FindLargestContour(Mat channel, double thresh, double maxval)
        {
            Mat mat = channel.Clone();
            // 이진화를 위한 임계값 처리
            Cv2.Threshold(channel, mat, thresh, maxval, ThresholdTypes.Binary);

            // 윤곽선 찾기
            Cv2.FindContours(mat, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 가장 큰 윤곽선 찾기
            OpenCvSharp.Point[]? largestContour = null;
            double maxArea = 0;
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > maxArea)
                {
                    maxArea = area;
                    largestContour = contour;
                }
            }
            mat.Dispose();
            return largestContour;
        }

        /// <summary>
        /// 데이터 있는부분 크롭하고 원본에서 이동한 좌료값 가져오기
        /// </summary>
        /// <param name="src"></param>
        public static (Mat crop, Point center) CropAndPrintContourInfo(Mat src, Point[] contour)
        {
            Rect boundingBox = Cv2.BoundingRect(contour);
            Point center = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            Mat cropped = new Mat(src, boundingBox);
            return (cropped, center);
        }

        /// <summary>
        /// 데이터 있는부분 크롭하고 원본에서 이동한 좌료값 가져오기
        /// </summary>
        /// <param name="src"></param>
        public static (Mat crop, Point center) CropAndPrintContourInfo(Mat src, double thresh, double maxval)
        {
            Point[]? contour = FindLargestContour(src, thresh, maxval);
            if (contour == null) { throw new ArgumentNullException(nameof(contour)); }
            Rect boundingBox = Cv2.BoundingRect(contour);
            Point center = new(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            Mat cropped = new(src, boundingBox);
            return (cropped, center);
        }

        public static List<(Mat crop, Point center)> CropAndPrintContourInfos(Mat src, double thresh, double maxval)
        {
            List<(Mat crop, Point center)> results = new();
            Point[][] contours = FindAndSortContoursByArea(src, thresh, maxval);
            foreach (var contour in contours)
            {
                var result = CropAndPrintContourInfo(src, contour);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// 바이트 배열로 변환
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] ExtractData(Mat src)
        {
            // 바이트 배열로 변환
            int row = src.Width;
            int cols = src.Height;
            byte[] redChannelData = new byte[row * cols];
            src.GetArray(out redChannelData);
            return redChannelData;
        }

        /// <summary>
        /// 바이트 배열을 Mat 객체로
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Mat ConvertByteArrayToMat(byte[] byteArray, int width, int height)
        {
            // 새 Mat 객체 생성
            Mat mat = new Mat(height, width, MatType.CV_8UC1); // 가정: 3 채널 이미지 (예: BGR)
            unsafe
            {
                // 바이트 배열을 Mat 객체로 복사
                IntPtr dataPointer = new(mat.DataPointer);
                Marshal.Copy(byteArray, 0, dataPointer, byteArray.Length);
            }
            return mat;
        }

    }
}
