using ImageAssist.SupportedFunction;
using ImageAssist.Utile;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageAssist.LOpenCV.HashAlgorithms
{
    internal class PerceptualHash : IImageHash
    {
        public ulong GetHash<T>(ref T inImage, System.Drawing.Size? size = null) where T : class
        {
            if (inImage is not Mat image)
            {
                return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), inImage.GetType());
            }

            Size resize;
            if (size != null)
            { resize = new(size.Value.Width, size.Value.Height); }
            else { resize = new(64, 64); }

            // 이미지 크기를 조정합니다.
            Mat resizedImage = new Mat();
            Cv2.Resize(image, resizedImage, resize);

            // 이미지 데이터를 부동 소수점 형식으로 변환합니다.
            Mat floatImage = new Mat();
            resizedImage.ConvertTo(floatImage, MatType.CV_32F);

            // DCT를 적용합니다.
            Mat dctImage = new Mat();
            Cv2.Dct(floatImage, dctImage);

            // DCT 결과에서 상위 왼쪽 영역을 추출합니다.
            Mat subImage = dctImage.SubMat(new Rect(0, 0, 8, 8));

            // 중앙값을 계산합니다.
            Scalar median = Cv2.Mean(subImage);

            ulong hash = 0;
            for (int y = 0; y < subImage.Height; y++)
            {
                for (int x = 0; x < subImage.Width; x++)
                {
                    hash <<= 1;
                    if (subImage.At<byte>(y, x) > median.Val0)
                    {
                        hash |= 1UL;
                    }
                }
            }
            return hash;
        }

        public ulong GetHash<T>(T inImage, System.Drawing.Size? size = null) where T : class
        {
            return GetHash<T>(ref inImage, size);
        }
    }
}
