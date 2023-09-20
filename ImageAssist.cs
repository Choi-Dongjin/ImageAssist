using OpenCvSharp;
using System.Drawing;
using System.Runtime.Versioning;

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

        [SupportedOSPlatform("windows")]
        public static Bitmap? LoadBitmap(string path)
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

        public static Mat ImageCrop(Mat mat, OpenCvSharp.Point p1, OpenCvSharp.Point p2)
        {
            OpenCvSharp.Size size = new(p2.X - p1.X, p2.Y - p1.Y);
            Rect rect = new(p1, size);
            return mat.SubMat(rect);
        }

        public static List<Mat> ImageCropRatio(Mat imageMat, int divisionsX, int divisionsY)
        {
            List<Mat> croppedImages = new List<Mat>();

            // 이미지의 가로와 세로 길이 구하기
            int imageWidth = imageMat.Width;
            int imageHeight = imageMat.Height;

            // 자르기 후 이미지의 크기
            int cutWidth = imageWidth / divisionsX;
            int cutHeight = imageHeight / divisionsY;

            // 이미지를 등분하여 잘라내기
            for (int y = 0; y < divisionsY; y++)
            {
                for (int x = 0; x < divisionsX; x++)
                {
                    // 자를 영역의 좌상단 좌표 계산
                    int startX = x * cutWidth;
                    int startY = y * cutHeight;

                    // 영역 잘라내기
                    Rect roi = new Rect(startX, startY, cutWidth, cutHeight);
                    Mat croppedImage = new Mat(imageMat, roi);

                    croppedImages.Add(croppedImage);
                }
            }

            return croppedImages;
        }

        public static List<SixLabors.ImageSharp.Image<Rgba32>> ImageCropRatio(SixLabors.ImageSharp.Image<Rgba32> imageMat, int divisionsX, int divisionsY)
        {


            return new();
        }

        public static bool ImageSave(Mat mat, string path, EAssistExtension imageExt)
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
                    case EAssistExtension.JPENG:
                        imwriteFlags = ImwriteFlags.JpegQuality;
                        imageEncodingParam = new(imwriteFlags, 100);
                        IsSave = Cv2.ImWrite(Path.Combine(dir, filename + ".jpg"), mat, imageEncodingParam);
                        break;

                    case EAssistExtension.PNG:
                        imwriteFlags = ImwriteFlags.PngCompression;
                        imageEncodingParam = new(imwriteFlags, 0);
                        IsSave = mat.ImWrite(Path.Combine(dir, filename + ".png"), imageEncodingParam);
                        break;

                    case EAssistExtension.BMP:
                        imwriteFlags = ImwriteFlags.WebPQuality;
                        imageEncodingParam = new(imwriteFlags, 1000);
                        IsSave = mat.ImWrite(Path.Combine(dir, filename + ".bmp"));
                        break;

                    case EAssistExtension.TIFF:
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