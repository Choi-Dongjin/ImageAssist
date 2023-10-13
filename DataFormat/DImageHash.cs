namespace ImageAssist.DataFormat
{
    public class DImageHash
    {
        /// <summary>
        /// 파일 경로
        /// </summary>
        public string FilePath { get; init; } = string.Empty;

        /// <summary>
        /// 값
        /// </summary>
        public ulong Hash { get; init; }
    }
}
