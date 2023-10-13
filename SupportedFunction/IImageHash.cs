namespace ImageAssist.SupportedFunction
{
    internal interface IImageHash
    {

        ulong GetHash<T>(ref T image, System.Drawing.Size? size = null) where T : class;

        ulong GetHash<T>(T image, System.Drawing.Size? size = null) where T : class;
    }
}
