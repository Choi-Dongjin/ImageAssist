using ImageAssist.SupportedFunction;
using ImageAssist.Utile;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageAssist.LOpenCV.HashAlgorithms
{
    internal class AverageHash : IImageHash
    {
        public ulong GetHash<T>(ref T inImage, System.Drawing.Size? size = null) where T : class
        {
            if (inImage is not Mat image)
            {
                return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), inImage.GetType());
            }

            Size resize;
            if (size != null)
            {
                resize = new Size(size.Value.Width, size.Value.Height);
            }
            else
            {
                resize = new Size(64, 64);
            }

            Mat resizeImage = new Mat();
            // 이미지 크기를 8x8로 축소
            Cv2.Resize(image, resizeImage, resize, interpolation: InterpolationFlags.Cubic);

            ulong totalValue = 0;

            for (int y = 0; y < resizeImage.Rows; y++)
            {
                for (int x = 0; x < resizeImage.Cols; x++)
                {
                    Vec3b pixelColor = resizeImage.At<Vec3b>(y, x);
                    totalValue += (ulong)(pixelColor.Item0 + pixelColor.Item1 + pixelColor.Item2);
                }
            }

            // 평균 색상 값을 계산하여 해시 생성
            double averageValue = totalValue / (8.0 * 8.0 * 3.0);
            ulong roundedAverage = (ulong)Math.Round(averageValue);
            return roundedAverage;
        }

        public ulong GetHash<T>(T inImage, System.Drawing.Size? size = null) where T : class
        {
            return GetHash(ref inImage, size);
        }
    }
}
