namespace ImageAssist
{
    public class ImageInfo
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }
        public byte[] Bytes { get; set; }

        public string FilePath { get; init; }

        public ImageInfo(string filePath, LibraryType.LType lType = LibraryType.Default)
        {
            FilePath = filePath;
            switch (lType)
            {
                case LibraryType.LType.OpenCV:
                    {
                        using ImageInfoOpenCV imageInfoOpenCV = new(filePath);
                        Width = imageInfoOpenCV.Width;
                        Height = imageInfoOpenCV.Height;
                        ChannelCount = imageInfoOpenCV.ChannelCount;
                        BitDepth = imageInfoOpenCV.BitDepth;
                        Bytes = imageInfoOpenCV.Bytes;
                        break;
                    }
                case LibraryType.LType.ImageSharp:
                    {
                        using ImageInfoImageSharp imageInfoImageSharp = new(filePath);
                        Width = imageInfoImageSharp.Width;
                        Height = imageInfoImageSharp.Height;
                        ChannelCount = imageInfoImageSharp.ChannelCount;
                        BitDepth = imageInfoImageSharp.BitDepth;
                        Bytes = imageInfoImageSharp.Bytes;
                        break;
                    }
                default:
                    {
                        using ImageInfoImageSharp imageInfoImageSharp = new(filePath);
                        Width = imageInfoImageSharp.Width;
                        Height = imageInfoImageSharp.Height;
                        ChannelCount = imageInfoImageSharp.ChannelCount;
                        BitDepth = imageInfoImageSharp.BitDepth;
                        Bytes = imageInfoImageSharp.Bytes;
                        break;
                    }
            }
        }
    }
}