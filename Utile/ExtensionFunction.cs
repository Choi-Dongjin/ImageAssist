using ImageAssist.SupportType;

namespace ImageAssist.Utile
{
    public class ExtensionFunction
    {
        /// <summary>
        /// 파일 이름에서 확장자 가져오기 & 지원하는 확장자인지 확인
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ESupportedExtensions GetExtensionFromFileName(string fileName)
        {
            string fileExtension = System.IO.Path.GetExtension(fileName).ToUpper();
            ESupportedExtensions ext = new();

            switch (fileExtension)
            {
                case ".JPG":
                    ext = ESupportedExtensions.JPEG;
                    break;
                case ".TIF":
                    ext = ESupportedExtensions.TIFF;
                    break;
                default:
                    break;
            }

            if (ext == ESupportedExtensions.None || ext == ESupportedExtensions.Unknown)
            {
                foreach (ESupportedExtensions supportedExt in Enum.GetValues(typeof(ESupportedExtensions)))
                {
                    if (fileExtension.Equals("." + supportedExt.ToString()))
                    {
                        ext = supportedExt;
                        break;
                    }
                }
            }
            return ext;
        }


        // Get _image supportedExt based on file signature
        /// <summary>
        /// 파일 데이터에서 확장자 예측
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ESupportedExtensions GuessImageExtension(string fileName)
        {
            byte[] header = new byte[8]; // 이미지 헤더를 읽을 바이트 배열 크기

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                fs.Read(header, 0, header.Length);
            }
            return GuessImageExtension(ref header);
        }
        /// <summary>
        /// 파일 데이터에서 확장자 예측
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static ESupportedExtensions GuessImageExtension(ref byte[] bytes)
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

            return ESupportedExtensions.Unknown; // LTypeDefault supportedExt if signature is not recognized
        }
    }
}
