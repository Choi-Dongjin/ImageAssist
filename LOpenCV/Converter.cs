using ImageAssist.DataFormat;
using OpenCvSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ImageAssist.LOpenCV
{
    public class Converter
    {
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
        /// Byte[] Mat 으로 변환
        /// </summary>
        /// <param name="src"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public static Mat ExtractData(byte[] src, DImageSize imageSize)
        {
            Mat result = new Mat(imageSize.Height, imageSize.Width, MatType.CV_8UC1, src);
            return result;
        }

        [SupportedOSPlatform("windows")]
        /// <summary>
        /// Mat을 Bitmap으로 변환하는 함수
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Bitmap MatToBitmap(Mat mat)
        {
            if (mat == null)
                throw new ArgumentNullException(nameof(mat));

            // Mat 데이터를 바이트 배열로 가져오기
            byte[] data = new byte[mat.Total() * mat.ElemSize()];
            Marshal.Copy(mat.Data, data, 0, data.Length);

            // Mat의 색상 채널 및 크기 가져오기
            int channels = mat.Channels();
            int width = mat.Width;
            int height = mat.Height;

            // Bitmap 생성
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // Bitmap 데이터를 바이트 배열로 가져오기
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            byte[] bitmapBytes = new byte[bitmapData.Stride * height];

            // Mat 데이터를 Bitmap으로 복사
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int c = 0; c < channels; c++)
                    {
                        bitmapBytes[y * bitmapData.Stride + x * channels + c] = data[y * width * channels + x * channels + c];
                    }
                }
            }

            Marshal.Copy(bitmapBytes, 0, bitmapData.Scan0, bitmapBytes.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
