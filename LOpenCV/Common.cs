using OpenCvSharp;

namespace ImageAssist.LOpenCV;

public static class Common
{
    public static Mat Overlay(ref Mat src, ref Mat overlay, Point location)
    {
        Mat srcClone = src.Clone();
        for (int y = 0; y < overlay.Rows; ++y)
        {
            int ny = y + location.Y;
            if (ny < 0 || ny >= srcClone.Rows)
                continue;

            for (int x = 0; x < overlay.Cols; ++x)
            {
                int nx = x + location.X;
                if (nx < 0 || nx >= srcClone.Cols)
                    continue;

                Vec3b srcColor = srcClone.At<Vec3b>(ny, nx);
                Vec3b overlayColor = overlay.At<Vec3b>(y, x);

                if (overlayColor.Item2 != 0 && overlayColor.Item1 != 0 && overlayColor.Item0 != 0)
                {
                    srcColor.Item0 = overlayColor.Item0;
                    srcColor.Item1 = overlayColor.Item1;
                    srcColor.Item2 = overlayColor.Item2;
                    srcClone.Set(y, x, srcColor);
                }
            }
        }
        return srcClone;
    }

    public static Mat Overlay(ref Mat src, ref Mat src2, double ratio)
    {
        Mat result = new();
        Cv2.AddWeighted(src, ratio, src2, 1.0 - ratio, 0, result);
        return result;
    }
}

