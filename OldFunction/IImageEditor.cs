namespace ImageAssist.OldFunction
{
    internal interface IImageEditor
    {
        public void Resize(int width, int height);
        public void Rotate(float angle);
        public void Crop(int startX, int startY, int width, int height);

    }
}
