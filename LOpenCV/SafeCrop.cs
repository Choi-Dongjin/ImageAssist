using OpenCvSharp;

namespace ImageAssist.LOpenCV
{
    public class SafeCrop
    {
        // startPoint와 endPoint가 범위 내에 있는지 확인하는 함수
        public static bool IsPointWithinBounds(Point point, int maxWidth, int maxHeight)
        {
            return point.X >= 0 && point.X < maxWidth && point.Y >= 0 && point.Y < maxHeight;
        }

        // startPoint와 endPoint를 사용하여 올바른 크롭 영역을 계산하는 함수
        public static Rect CalculateCropRect(Point startPoint, Point endPoint, int maxWidth, int maxHeight)
        {
            int left = Math.Max(0, Math.Min(startPoint.X, endPoint.X));
            int top = Math.Max(0, Math.Min(startPoint.Y, endPoint.Y));
            int right = Math.Min(maxWidth, Math.Max(startPoint.X, endPoint.X));
            int bottom = Math.Min(maxHeight, Math.Max(startPoint.Y, endPoint.Y));

            int width = right - left;
            int height = bottom - top;

            return new Rect(left, top, width, height);
        }

        // 크롭할 이미지를 안전하게 처리하는 함수
        public static Mat Crop(Mat org, Point startPoint, Point endPoint)
        {
            int maxWidth = org.Width;
            int maxHeight = org.Height;

            // 올바른 크롭 영역 계산
            Rect cropRect = CalculateCropRect(startPoint, endPoint, maxWidth, maxHeight);

            // 이미지 크롭
            Mat cropped = new Mat(org, cropRect);

            return cropped;
        }
    }
}
