namespace ImageAssist.OldFunction
{
    public class ImageEditorImageSharp
    {
        private Image<Rgba32> image;

        public ImageEditorImageSharp(string filePath)
        {
            image = Image.Load<Rgba32>(filePath);
        }

        public void Resize(int width, int height)
        {
            image.Mutate(x => x.Resize(new Size(width, height)));
        }

        public void Rotate(float angle)
        {
            image.Mutate(x => x.Rotate(angle));
        }

        public void Crop(int startX, int startY, int width, int height)
        {
            image.Mutate(x => x.Crop(new Rectangle(startX, startY, width, height)));
        }

        public Rgba32[][]? FindContours()
        {
            // Not directly supported in SixLabors.ImageSharp
            return null;
        }

        public double CalculateAverageBrightness()
        {
            double sum = 0;
            int count = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    double brightness = (pixel.R + pixel.G + pixel.B) / 3.0;
                    sum += brightness;
                    count++;
                }
            }
            return sum / count;
        }

        public void BilateralFilter(int diameter, double sigmaColor, double sigmaSpace)
        {
            // Not directly supported in SixLabors.ImageSharp
        }

        public void GaussianBlur(int radius)
        {
            image.Mutate(x => x.GaussianBlur(radius));
        }

        public void MedianBlur(int radius)
        {
            Image<Rgba32> result = image.Clone();
            for (int y = radius; y < image.Height - radius; y++)
            {
                for (int x = radius; x < image.Width - radius; x++)
                {
                    Rgba32[] neighborhood = GetPixelNeighborhood(x, y, radius);
                    Rgba32 median = GetMedianPixel(neighborhood);
                    result[x, y] = median;
                }
            }
            image = result;
        }

        private Rgba32[] GetPixelNeighborhood(int x, int y, int radius)
        {
            int startX = x - radius;
            int startY = y - radius;
            int size = (2 * radius + 1) * (2 * radius + 1);
            Rgba32[] neighborhood = new Rgba32[size];
            int index = 0;
            for (int j = 0; j < 2 * radius + 1; j++)
            {
                for (int i = 0; i < 2 * radius + 1; i++)
                {
                    neighborhood[index++] = image[startX + i, startY + j];
                }
            }
            return neighborhood;
        }

        private Rgba32 GetMedianPixel(Rgba32[] neighborhood)
        {
            neighborhood = neighborhood.OrderBy(p => p.A).ToArray();
            int middleIndex = neighborhood.Length / 2;
            return neighborhood[middleIndex];
        }

        public void Save(string filePath)
        {
            image.Save(filePath);
        }
    }
}