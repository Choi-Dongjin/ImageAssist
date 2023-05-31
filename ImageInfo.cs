namespace ImageAssist
{
    public class ImageInfo
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }
        public byte[] Bytes { get; set; }

        public string FileName { get; init; }

        public ImageInfo(string fileName, LibraryType.LType lType = LibraryType.Default)
        {
            FileName = fileName;
            switch (lType)
            {
                case LibraryType.LType.OpenCV:
                    {
                        using ImageInfoOpenCV imageInfoOpenCV = new(fileName);
                        Width = imageInfoOpenCV.Width;
                        Height = imageInfoOpenCV.Height;
                        ChannelCount = imageInfoOpenCV.ChannelCount;
                        BitDepth = imageInfoOpenCV.BitDepth;
                        Bytes = imageInfoOpenCV.Bytes;
                        break;
                    }
                case LibraryType.LType.ImageSharp:
                    {
                        using ImageInfoOpenCV imageInfoOpenCV = new(fileName);
                        Width = imageInfoOpenCV.Width;
                        Height = imageInfoOpenCV.Height;
                        ChannelCount = imageInfoOpenCV.ChannelCount;
                        BitDepth = imageInfoOpenCV.BitDepth;
                        Bytes = imageInfoOpenCV.Bytes;
                        break;
                    }
                default:
                    {
                        using ImageInfoOpenCV imageInfoOpenCV = new(fileName);
                        Width = imageInfoOpenCV.Width;
                        Height = imageInfoOpenCV.Height;
                        ChannelCount = imageInfoOpenCV.ChannelCount;
                        BitDepth = imageInfoOpenCV.BitDepth;
                        Bytes = imageInfoOpenCV.Bytes;
                        break;
                    }
            }
        }
    }
}