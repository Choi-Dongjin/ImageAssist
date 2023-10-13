namespace ImageAssist.Utile
{
    internal class ErrorThrowHelper
    {
        public static object InputTypeError<T>(T okType, Type errorType)
        {
            string errorMes = string.Format("The input type is not {0}\nInput data type is : {1}", (okType?.GetType().FullName) ?? "Unknown", errorType.FullName);
            throw new InvalidCastException(errorMes);
        }
    }
}
