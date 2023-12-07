using ImageAssist.OldFunction;
using ImageAssist.SupportType;
using ImageAssist.Utile;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace ImageAssist.DataFormat
{
    public class DImageInfo
    {
        public string FilePath { get; init; }
        public string FileName { get; init; }
        public string Encoding { get { return Extension.ToString(); } }
        public DImageSize OriginImageSize { get; set; }
        public LType LType { get; init; }
        public ESupportedExtensions Extension { get; init; }

        public DImageInfo()
        {
            FilePath = string.Empty;
            FileName = string.Empty;
            OriginImageSize = new();
            Extension = ESupportedExtensions.None;
        }

        [SupportedOSPlatform("windows")]
        public DImageInfo(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            LType = ImageAssistSystemDefault.LTypeDefault;
            using ImageInfoOpenCV imageInfoOpenCV = new(filePath);
            OriginImageSize = imageInfoOpenCV.ImageSize;
            Extension = imageInfoOpenCV.Extension;
        }

        [SupportedOSPlatform("windows")]
        public DImageInfo(string filePath, LType? lType = LType.Drawing)
        {
            OriginImageSize = new();
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            if (lType == null) { LType = LType.OpenCV; }
            else { LType = lType.Value; }
            Extension = ExtensionFunction.GetExtensionFromFileName(FilePath);

            // 확장자 가져오기
            if (Extension == ESupportedExtensions.None || Extension == ESupportedExtensions.Unknown)
            {
                Extension = ExtensionFunction.GuessImageExtension(FilePath);
            }

            // 이미지 사이즈 가져오기
            OriginImageSize = OSCaseProcessing(filePath, LType) ?? new();
        }

        [SupportedOSPlatform("windows")]
        private static DImageSize? OSCaseProcessing(string file, LType lType)
        {
            var osInfo = Environment.OSVersion; // OS 버전 정보를 가져옴
            var platform = osInfo.Platform; // 운영체제의 플랫폼을 가져옴
            DImageSize? imageSize = null;

            switch (platform)
            {

                case PlatformID.Win32NT: // 대부분의 윈도우 OS 버전
                    Debug.WriteLine("OS is Windows NT based."); // Windows 관련 작업 수행
                    imageSize = OSCaseWindows(file, lType);
                    break;

                case PlatformID.Unix: // 유닉스 기반 시스템 (리눅스 포함)
                    Debug.WriteLine("OS is Unix-based."); // Unix/Linux 관련 작업 수행
                    break;

                case PlatformID.MacOSX: // 맥 OS X 
                    Debug.WriteLine("OS is MacOSX."); // MacOS 관련 작업 수행
                    break;

                default:
                    Debug.WriteLine("Unknown platform."); // 알 수 없는 OS의 경우의 작업 수행
                    break;
            }
            return imageSize;
        }

        [SupportedOSPlatform("windows")]
        private static DImageSize? OSCaseWindows(string file, LType lType)
        {
            DImageSize? imageSize = null;
            switch (lType)
            {
                case LType.Drawing:
                    imageSize = LDrawing.CommonFunction.GetImageSize(file);
                    break;
                case LType.OpenCV: break;
                case LType.ImageSharp: break;
                default: break;
            }
            return imageSize;
        }

        private static DImageSize? OSCaseUnixBased(string file, LType lType)
        {
            return null;
        }

        private static DImageSize? OSCaseMacOSX(string file, LType lType)
        {
            return null;
        }
    }
}
