using ImageAssist.OldFunction;
using ImageAssist.SupportType;
using System.Runtime.Versioning;

namespace ImageAssist.DataFormat
{
    public class DImageInfo
    {
        public string FilePath { get; init; }
        public string Encoding { get { return Extension.ToString(); } }
        public DImageSize ImageSize { get; init; }
        public LType LType { get; init; }
        public ESupportedExtensions Extension { get; init; }
        public byte[]? Bytes { get; set; }

        public DImageInfo()
        {
            FilePath = string.Empty;
            ImageSize = new();
            Extension = ESupportedExtensions.None;
        }

        [SupportedOSPlatform("windows")]
        public DImageInfo(string filePath)
        {
            FilePath = filePath;
            LType = ImageAssistSystemDefault.LTypeDefault;
            using ImageInfoOpenCV imageInfoOpenCV = new(filePath);
            ImageSize = imageInfoOpenCV.ImageSize;
            Extension = imageInfoOpenCV.Extension;
        }

        public DImageInfo(string filePath, LType? lType, bool includeRowImage = false)
        {
            FilePath = filePath;
            if (lType == null)
            {
                LType = ImageAssistSystemDefault.LTypeDefault;
            }
            switch (lType)
            {
                case LType.None:
                    throw new NotImplementedException(lType.ToString());
                case LType.OpenCV:
                    {
                        using ImageInfoOpenCV imageInfoOpenCV = new(filePath);
                        ImageSize = imageInfoOpenCV.ImageSize;
                        Extension = imageInfoOpenCV.Extension;
                        if (includeRowImage)
                            Bytes = imageInfoOpenCV.Bytes;
                        break;
                    }
                case LType.ImageSharp:
                    {
                        using ImageInfoImageSharp imageInfoImageSharp = new(filePath);
                        ImageSize = imageInfoImageSharp.ImageSize;
                        Extension = imageInfoImageSharp.Extension;
                        if (includeRowImage)
                            Bytes = imageInfoImageSharp.Bytes;
                        break;
                    }
                default: throw new NotImplementedException(lType.ToString());
            }
        }
    }
}