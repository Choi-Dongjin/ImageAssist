namespace ImageAssist.SupportedFunction
{
    internal interface IPerceptualHashing
    {
        public Type GetDataType();
        public ulong GenerateAverageHash<T>(ref T image, System.Drawing.Size? size = null);
        public ulong GenerateDifferenceHash<T>(ref T image, System.Drawing.Size? size = null);
        public ulong GeneratePerceptualHash<T>(ref T image, System.Drawing.Size? size = null);
    }
}
