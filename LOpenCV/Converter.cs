using ImageAssist.DataFormat;
using OpenCvSharp;

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
        public static Mat ExtractData(Mat src, DImageSize imageSize)
        {
            Mat result = new Mat(imageSize.Height, imageSize.Width, imageSize.BitDepth, imageSize.Channel);
            result.CopyTo(src);
            return result;
        }
    }
}
