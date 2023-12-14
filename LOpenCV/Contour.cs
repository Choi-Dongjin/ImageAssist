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
            Mat contourMat = new();
            // 이진화를 위한 임계값 처리
            Cv2.Threshold(channel, contourMat, thresh, maxval, ThresholdTypes.Binary);
            // 윤곽선 찾기
            Cv2.FindContours(contourMat, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            // 윤곽선을 면적에 따라 정렬
            var sortedContours = contours.OrderByDescending(c => Cv2.ContourArea(c)).ToArray();
            contourMat.Dispose();
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
            Point center = new(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            Mat cropped = new(src, boundingBox);
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

        /// <summary>
        /// 윤곽선 내부 이미지만 취득
        /// </summary>
        /// <param name="src"></param>
        /// <param name="contours"></param>
        /// <returns></returns>
        public static Mat ExtractContourInterior(Mat src, Point[][] contours)
        {
            Mat mask = Mat.Zeros(src.Size(), MatType.CV_8UC1);
            Cv2.DrawContours(mask, contours, -1, Scalar.White, thickness: -1);
            // 마스크를 이용하여 원본 이미지에서 컨투어 내부의 영역만 추출합니다.
            Mat result = new();
            src.CopyTo(result, mask);
            return result;
        }

        public static Point[][] MergeContours(Point[][] contours1, Point[][] contours2)
        {
            var mergedContours = new List<Point[]>();

            // 첫 번째 이미지의 윤곽선 추가
            mergedContours.AddRange(contours1);

            // 두 번째 이미지의 윤곽선에 대해 처리
            foreach (var contour in contours2)
            {
                double a = Cv2.ContourArea(contour);
                // 겹치는 부분을 확인하여 이미 추가된 윤곽선과 겹친다면 추가하지 않음
                if (!IsContourOverlapping(mergedContours, contour))
                {
                    // 큰 윤곽선 안에 작은 윤곽선이 있다면 큰 윤곽선만을 추가
                    if (!IsContourInsideAny(mergedContours, contour))
                    {
                        mergedContours.Add(contour);
                    }
                }
            }
            return mergedContours.ToArray();
        }

        public static bool IsContourOverlapping(List<Point[]> contours, Point[] newContour)
        {
            foreach (var existingContour in contours)
            {
                if (Cv2.PointPolygonTest(existingContour, newContour[0], false) >= 0)
                {
                    // 겹치는 부분이 있으면 true 반환
                    return true;
                }
            }
            return false;
        }

        public static bool IsContourInsideAny(List<Point[]> contours, Point[] newContour)
        {
            foreach (var existingContour in contours)
            {
                if (Cv2.PointPolygonTest(existingContour, newContour[0], true) > 0)
                {
                    // 큰 윤곽선 안에 작은 윤곽선이 있다면 true 반환
                    return true;
                }
            }
            return false;
        }

    }
}
