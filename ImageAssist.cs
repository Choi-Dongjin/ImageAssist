using OpenCvSharp;
using System.Drawing;
using Point = OpenCvSharp.Point;
using Range = OpenCvSharp.Range;

namespace ImageAssist
{
    public class ImageAssist
    {
        public enum ImageExt
        {
            JPENG = 0,
            PNG = 1,
            BMP = 2,
            TIFF = 3,
        }

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

        public static Mat ImageCrop(Mat mat, Point p1, Point p2)
        {
            OpenCvSharp.Size size = new(p2.X - p1.X, p2.Y - p1.Y);
            Rect rect = new(p1, size);
            return mat.SubMat(rect);
        }

        public static bool ImageSave(Mat mat, string path, ImageExt imageExt)
        {
            ImwriteFlags imwriteFlags;
            ImageEncodingParam imageEncodingParam;
            string? dir = Path.GetDirectoryName(path);
            string? filename = Path.GetFileNameWithoutExtension(path);
            bool IsSave = false;
            if (!string.IsNullOrWhiteSpace(dir) && !string.IsNullOrWhiteSpace(filename))
            {
                switch (imageExt)
                {
                    case ImageExt.JPENG:
                        imwriteFlags = ImwriteFlags.JpegQuality;
                        imageEncodingParam = new(imwriteFlags, 100);
                        IsSave = Cv2.ImWrite(Path.Combine(dir, filename + ".jpg"), mat, imageEncodingParam);
                        break;
                    case ImageExt.PNG:
                        imwriteFlags = ImwriteFlags.PngCompression;
                        imageEncodingParam = new(imwriteFlags, 0);
                        IsSave = mat.ImWrite(Path.Combine(dir, filename + ".png"), imageEncodingParam);
                        break;
                    case ImageExt.BMP:
                        imwriteFlags = ImwriteFlags.WebPQuality;
                        imageEncodingParam = new(imwriteFlags, 1000);
                        IsSave = mat.ImWrite(Path.Combine(dir, filename + ".bmp"));
                        break;
                    case ImageExt.TIFF:
                        imwriteFlags = ImwriteFlags.TiffResUnit;
                        imageEncodingParam = new(imwriteFlags, 600);
                        IsSave = mat.ImWrite(Path.Combine(dir, filename + ".tiff"));
                        break;
                }
            }
            return IsSave;
        }
    }
}