using ImageAssist.SupportedFunction;
using ImageAssist.Utile;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace ImageAssist.LOpenCV.HashAlgorithms
{
    internal class DifferenceHash : IImageHash
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
            else { resize = new(9, 8); }

            Mat smallImage = new Mat();
            Cv2.Resize(image, smallImage, resize);

            ulong hash = 0;
            for (int y = 0; y < resize.Height; y++)
            {
                for (int x = 0; x < resize.Width - 1; x++)
                {
                    if (smallImage.At<byte>(y, x) < smallImage.At<byte>(y, x + 1))
                    {
                        hash |= 1UL << (y * resize.Width + x);
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
