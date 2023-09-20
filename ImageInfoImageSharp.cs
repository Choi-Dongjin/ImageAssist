using Image = SixLabors.ImageSharp.Image;

namespace ImageAssist
{
    public class ImageInfoImageSharp : IDisposable
    {
        #region IDisposable

        // To detect redundant calls
        private bool _disposed = false;

        ~ImageInfoImageSharp() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Bytes = Array.Empty<byte>();
            }
            _disposed = true;
        }

        #endregion IDisposable

        public ImageSize ImageSize { get; set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; init; }
        public EAssistExtension Extension { get; set; }

        public ImageInfoImageSharp(string fileName)
        {
            FileName = fileName;
            try
            {
                Bytes = System.IO.File.ReadAllBytes(fileName);
                Extension = DImageInfo.GetImageExtension(Bytes);
                ImageSize = new();
                try
                {
                    using var image = Image.Load(Bytes);
                    var pixelType = image.GetType().GenericTypeArguments[0]; // 이미지의 픽셀 형식 가져오기

                    if (pixelType == typeof(Rgb24))
                    {
                        // RGBA32 이미지인 경우
                        ImageSize.Width = image.Width;
                        ImageSize.Height = image.Height;
                        ImageSize.Channel = 3;
                        ImageSize.BitDepth = 24;
                    }
                    else if (pixelType == typeof(Rgba32))
                    {
                        // Rgb24 이미지인 경우
                        ImageSize.Width = image.Width;
                        ImageSize.Height = image.Height;
                        ImageSize.Channel = 4;
                        ImageSize.BitDepth = 32;
                    }
                    else if (pixelType == typeof(Bgr24))
                    {
                        // BGR24 이미지인 경우
                        ImageSize.Width = image.Width;
                        ImageSize.Height = image.Height;
                        ImageSize.Channel = 3;
                        ImageSize.BitDepth = 24;
                    }
                    else if (pixelType == typeof(Bgra32))
                    {
                        // BG   R24 이미지인 경우
                        ImageSize.Width = image.Width;
                        ImageSize.Height = image.Height;
                        ImageSize.Channel = 4;
                        ImageSize.BitDepth = 32;
                    }
                    else
                    {
                        // 지원하지 않는 픽셀 형식인 경우
                        throw new NotSupportedException($"Pixel format '{pixelType.Name}' is not supported.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while getting image info: {ex.Message}");
                    throw new Exception("Failed to read image in image helper.");
                }

            }
            catch (Exception ex)
            {
                throw new System.IO.IOException(ex.ToString());
            }
        }
    }
}