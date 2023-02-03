using OpenCvSharp;
using System.Drawing;

namespace ImageAssist
{
    public class ImageAssist
    {
        /// <summary>
        /// 이미지 검색 확장자 필터 설정
        /// </summary>
        private static readonly string[] imageExts = new[] { ".jpg", ".bmp", ".png" }; // 검색할 확장자 필터

        public static void SWImage(Mat img_contour)
        {
            Cv2.ImShow("Image_Contour", img_contour);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
        // ExtractCoordinatesMaskImage
        // ImageAssistExtension

        public static Bitmap LoadBitmap(string path)
        {
            if (imageExts.Contains(Path.GetExtension(path)))
            {
                if (File.Exists(path))
                {
                    // open file in read only mode
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        // get a binary reader for the file stream
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            // copy the content of the file into a memory stream
                            var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                            // make a new Bitmap object the owner of the MemoryStream
                            return new Bitmap(memoryStream);
                        }
                    }
                }
                else
                {
                    // MessageBox.Show("Error Loading File.", "Error!", MessageBoxButtons.OK);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}