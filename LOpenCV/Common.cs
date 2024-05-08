using OpenCvSharp;

namespace ImageAssist.LOpenCV;

public static class Common
{
    /// <summary>
    /// weight overlay 
    /// </summary>
    /// <param name="src"></param>
    /// <param name="src2"></param>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static Mat Overlay(Mat src, Mat src2, double ratio)
    {
        Mat result = new();
        Cv2.AddWeighted(src, ratio, src2, 1.0 - ratio, 0, result);
        return result;
    }

    /// <summary>
    /// 이동 overlay
    /// </summary>
    /// <param name="src"></param>
    /// <param name="overlay"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static Mat Overlay(Mat src, Mat overlay, Point offset)
    {
        // 복사본 생성 (원본 이미지를 변경하지 않기 위함)
        Mat result = src.Clone();
        // 오버레이 위치 계산
        Point topLeft = offset;
        // 오버레이를 이미지에 합성
        for (int y = 0; y < overlay.Rows; y++)
        {
            for (int x = 0; x < overlay.Cols; x++)
            {
                // 오버레이 영역이 이미지 경계를 넘어가지 않도록 확인
                if (topLeft.Y + y >= 0 && topLeft.Y + y < src.Rows &&
                    topLeft.X + x >= 0 && topLeft.X + x < src.Cols)
                {
                    Vec3b srcPixel = src.At<Vec3b>(topLeft.Y + y, topLeft.X + x);
                    Vec3b overlayPixel = overlay.At<Vec3b>(y, x);

                    // 오버레이 픽셀의 알파 값이 0이 아닌 경우에만 합성
                    if (overlayPixel.Item2 != 0 && overlayPixel.Item1 != 0 && overlayPixel.Item0 != 0)
                    {
                        Vec3b resultPixel = new(
                            (byte)((srcPixel.Item0 * (255 - overlayPixel.Item0) + overlayPixel.Item0 * overlayPixel.Item2) / 255),
                            (byte)((srcPixel.Item1 * (255 - overlayPixel.Item0) + overlayPixel.Item0 * overlayPixel.Item1) / 255),
                            (byte)((srcPixel.Item2 * (255 - overlayPixel.Item0) + overlayPixel.Item0 * overlayPixel.Item0) / 255)
                        );

                        result.Set(topLeft.Y + y, topLeft.X + x, resultPixel);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 이동 & weight overlay 
    /// </summary>
    /// <param name="src"></param>
    /// <param name="overlay"></param>
    /// <param name="offset"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Mat Overlay(Mat src, Mat overlay, Point offset, double weight)
    {
        // 복사본 생성 (원본 이미지를 변경하지 않기 위함)
        Mat result = src.Clone();
        // 오버레이 위치 계산
        Point topLeft = offset;
        // 오버레이를 이미지에 합성
        for (int y = 0; y < overlay.Rows; y++)
        {
            for (int x = 0; x < overlay.Cols; x++)
            {
                // 오버레이 영역이 이미지 경계를 넘어가지 않도록 확인
                if (topLeft.Y + y >= 0 && topLeft.Y + y < src.Rows &&
                    topLeft.X + x >= 0 && topLeft.X + x < src.Cols)
                {
                    Vec3b srcPixel = src.At<Vec3b>(topLeft.Y + y, topLeft.X + x);
                    Vec3b overlayPixel = overlay.At<Vec3b>(y, x);

                    // 오버레이 픽셀의 알파 값이 0이 아닌 경우에만 합성
                    if (overlayPixel.Item2 != 0 && overlayPixel.Item1 != 0 && overlayPixel.Item0 != 0)
                    {
                        Vec3b resultPixel = new(
                            (byte)(srcPixel.Item0 * (1d - weight) + overlayPixel.Item0 * weight),
                            (byte)(srcPixel.Item1 * (1d - weight) + overlayPixel.Item1 * weight),
                            (byte)(srcPixel.Item2 * (1d - weight) + overlayPixel.Item2 * weight)
                        );

                        result.Set(topLeft.Y + y, topLeft.X + x, resultPixel);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 이동 & weight & alpha overlay 
    /// </summary>
    /// <param name="src"></param>
    /// <param name="overlay"></param>
    /// <param name="offset"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Mat Overlay(Mat src, Mat overlay, Point offset, double weight, byte alpha = 0)
    {
        // 복사본 생성 (원본 이미지를 변경하지 않기 위함)
        Mat result = src.Clone();
        // 오버레이 위치 계산
        Point topLeft = offset;
        // 오버레이를 이미지에 합성
        for (int y = 0; y < overlay.Rows; y++)
        {
            for (int x = 0; x < overlay.Cols; x++)
            {
                // 오버레이 영역이 이미지 경계를 넘어가지 않도록 확인
                if (topLeft.Y + y >= 0 && topLeft.Y + y < src.Rows &&
                    topLeft.X + x >= 0 && topLeft.X + x < src.Cols)
                {
                    Vec3b srcPixel = src.At<Vec3b>(topLeft.Y + y, topLeft.X + x);
                    Vec3b overlayPixel = overlay.At<Vec3b>(y, x);

                    // 오버레이 픽셀의 알파 값이 0이 아닌 경우에만 합성
                    if (overlayPixel.Item2 > alpha && overlayPixel.Item1 > alpha && overlayPixel.Item0 > alpha)
                    {
                        Vec3b resultPixel = new(
                            (byte)(srcPixel.Item0 * (1d - weight) + overlayPixel.Item0 * weight),
                            (byte)(srcPixel.Item1 * (1d - weight) + overlayPixel.Item1 * weight),
                            (byte)(srcPixel.Item2 * (1d - weight) + overlayPixel.Item2 * weight)
                        );

                        result.Set(topLeft.Y + y, topLeft.X + x, resultPixel);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 이동 & weight & alpha overlay 
    /// </summary>
    /// <param name="src"></param>
    /// <param name="overlay"></param>
    /// <param name="offset"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Mat OverlayTopLeftOffset(Mat src, Mat overlay, Point offset, double weight, byte alpha = 0, double resize = 1d)
    {
        // 복사본 생성 (원본 이미지를 변경하지 않기 위함)
        Mat result = src.Clone();

        // 오버레이 이미지 크기 조정
        Mat resizedOverlay = new();
        Cv2.Resize(overlay, resizedOverlay, new Size((int)(overlay.Width * resize), (int)(overlay.Height * resize)));

        // 오버레이 위치 계산
        Point topLeft = offset;
        // 오버레이를 이미지에 합성
        for (int y = 0; y < resizedOverlay.Rows; y++)
        {
            for (int x = 0; x < resizedOverlay.Cols; x++)
            {
                // 오버레이 영역이 이미지 경계를 넘어가지 않도록 확인
                if (topLeft.Y + y >= 0 && topLeft.Y + y < src.Rows &&
                    topLeft.X + x >= 0 && topLeft.X + x < src.Cols)
                {
                    Vec3b srcPixel = src.At<Vec3b>(topLeft.Y + y, topLeft.X + x);
                    Vec3b overlayPixel = resizedOverlay.At<Vec3b>(y, x);

                    // 오버레이 픽셀의 알파 값이 0이 아닌 경우에만 합성
                    if (overlayPixel.Item2 > alpha && overlayPixel.Item1 > alpha && overlayPixel.Item0 > alpha)
                    {
                        Vec3b resultPixel = new(
                            (byte)(srcPixel.Item0 * (1d - weight) + overlayPixel.Item0 * weight),
                            (byte)(srcPixel.Item1 * (1d - weight) + overlayPixel.Item1 * weight),
                            (byte)(srcPixel.Item2 * (1d - weight) + overlayPixel.Item2 * weight)
                        );

                        result.Set(topLeft.Y + y, topLeft.X + x, resultPixel);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 이동 & weight & alpha overlay 
    /// </summary>
    /// <param name="src"></param>
    /// <param name="overlay"></param>
    /// <param name="offset"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Mat Overlay(Mat src, Mat overlay, Point offset, double weight, byte alpha = 0, double resize = 1d, bool useAlpha = true, InterpolationFlags interpolation = InterpolationFlags.Linear)
    {
        // 복사본 생성 (원본 이미지를 변경하지 않기 위함)
        Mat result = src.Clone();

        // 오버레이 이미지 크기 조정
        Mat resizedOverlay = new();
        if (resize != 1d) { Cv2.Resize(overlay, resizedOverlay, new Size((int)(overlay.Width * resize), (int)(overlay.Height * resize)), interpolation: interpolation); }
        else { resizedOverlay = overlay; }

        // 이미지 간 보정값 
        int offsetX = (resizedOverlay.Cols - src.Cols) / 2 - offset.X;
        int offsetY = (resizedOverlay.Rows - src.Rows) / 2 - offset.Y;

        // 오버레이를 이미지에 합성
        for (int y = 0; y < resizedOverlay.Rows; y++)
        {
            for (int x = 0; x < resizedOverlay.Cols; x++)
            {
                int correctionY = y - offsetY; // 수정 죄표 - 이미지 간 보정값
                int correctionX = x - offsetX; // 수정 죄표 - 이미지 간 보정값

                // 오버레이 영역이 이미지 경계를 넘어가지 않도록 확인
                if (correctionY < 0 || correctionY >= src.Rows || correctionX < 0 || correctionX >= src.Cols) { continue; }

                // 좌표 값 가져오기
                Vec3b srcPixel = src.At<Vec3b>(correctionY, correctionX);
                Vec3b overlayPixel = resizedOverlay.At<Vec3b>(y, x);

                // 알파값 사용 확인 & 사용시 값 확인
                if (useAlpha) { if (overlayPixel.Item2 <= alpha || overlayPixel.Item1 <= alpha || overlayPixel.Item0 <= alpha) { continue; } }

                // 값 적용
                Vec3b resultPixel = new(
                           (byte)(srcPixel.Item0 * (1d - weight) + overlayPixel.Item0 * weight),
                           (byte)(srcPixel.Item1 * (1d - weight) + overlayPixel.Item1 * weight),
                           (byte)(srcPixel.Item2 * (1d - weight) + overlayPixel.Item2 * weight)
                       );

                // 이리지 그리시
                result.Set(correctionY, correctionX, resultPixel);
            }
        }

        return result;
    }
}

