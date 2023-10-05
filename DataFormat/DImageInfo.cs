﻿using ImageAssist.OldFunction;
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
        public byte[]? Bytes { get; init; }

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

        // Get _image extension based on file signature
        internal static ESupportedExtensions GetImageExtension(byte[] bytes)
        {
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8) // JPEG signature
            {
                return ESupportedExtensions.JPEG;
            }
            else if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
                      && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A) // PNG signature
            {
                return ESupportedExtensions.PNG;
            }
            else if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D) // BMP signature
            {
                return ESupportedExtensions.BMP;
            }
            else if (bytes.Length >= 4 && bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00
                      || bytes.Length >= 4 && bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A) // TIFF signature
            {
                return ESupportedExtensions.TIFF;
            }
            // Add more signatures for other _image formats if needed

            return ESupportedExtensions.Unknown; // LTypeDefault extension if signature is not recognized
        }
    }
}