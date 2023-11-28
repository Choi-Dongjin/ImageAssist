using ImageAssist.DataFormat;
using System.Runtime.Versioning;

namespace ImageAssist.LDrawing
{
    [SupportedOSPlatform("windows")]
    internal class CommonFunction
    {
        /// <summary>
        /// 이미지 가져오기
        /// </summary>
        /// <param name="path"> 이미지 파일 경로 </param>
        /// <returns></returns>
        public static System.Drawing.Image GetImage(string path)
        {
            return System.Drawing.Image.FromFile(path);
        }

        /// <summary>
        /// 이미지 가져오기
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImage(Stream stream)
        {
            return System.Drawing.Image.FromStream(stream);
        }

        /// <summary>
        /// 이미지 사이즈 가져오기
        /// </summary>
        /// <param name="path"> 이미지 파일 경로 </param>
        /// <returns></returns>
        public static DImageSize GetImageSize(string path) => GetImageSize(GetImage(path));

        /// <summary>
        /// 이미지 사이즈 가져오기
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static DImageSize GetImageSize(Stream stream) => GetImageSize(GetImage(stream));

        /// <summary>
        /// 이미지 사이즈 가져오기
        /// </summary>
        /// <param name="image"> 이미지 파일 경로 </param>
        /// <returns></returns>
        public static DImageSize GetImageSize(System.Drawing.Image image)
        {
            int width = image.Width;
            int height = image.Height;
            int bitDepth = System.Drawing.Image.GetPixelFormatSize(image.PixelFormat);
            int channel = bitDepth / 8;

            DImageSize imageSize = new()
            {
                Width = width,
                Height = height,
                BitDepth = bitDepth,
                Channel = channel,
            };

            return imageSize;
        }
    }
}
