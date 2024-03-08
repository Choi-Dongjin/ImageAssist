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

        public static DImageSize MetadataReaderGetImageSize(string path)
        {
            //var bit = string.Empty;
            //var width = string.Empty;
            //var height = string.Empty;
            //var widthData = 0;
            //var heightData = 0;
            //var bitData = 0;
            var metadata = MetadataExtractor.ImageMetadataReader.ReadMetadata(path);
            var bit = metadata.OfType<MetadataExtractor.Formats.Jpeg.JpegDirectory>().FirstOrDefault()?.GetDescription(MetadataExtractor.Formats.Jpeg.JpegDirectory.TagDataPrecision);
            var width = metadata.OfType<MetadataExtractor.Formats.Jpeg.JpegDirectory>().FirstOrDefault()?.GetDescription(MetadataExtractor.Formats.Jpeg.JpegDirectory.TagImageWidth);
            var height = metadata.OfType<MetadataExtractor.Formats.Jpeg.JpegDirectory>().FirstOrDefault()?.GetDescription(MetadataExtractor.Formats.Jpeg.JpegDirectory.TagImageHeight);
            var channel = metadata.OfType<MetadataExtractor.Formats.Jpeg.JpegDirectory>().FirstOrDefault()?.GetDescription(MetadataExtractor.Formats.Jpeg.JpegDirectory.TagNumberOfComponents);
            if (!int.TryParse(bit?.Split(' ')[0], out int bitData))
            {
                if (!int.TryParse(bit, out bitData))
                {
                    bitData = 0;
                }
            }
            if (!int.TryParse(width?.Split(' ')[0], out int widthData)) { widthData = 0; }
            if (!int.TryParse(height?.Split(' ')[0], out int heightData)) { heightData = 0; }
            if (!int.TryParse(channel, out int channelData)) { channelData = 0; }

            return new DImageSize()
            {
                Width = widthData,
                Height = heightData,
                Channel = channelData,
                BitDepth = bitData * 3,
            };
        }
    }
}
