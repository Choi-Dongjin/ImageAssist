using ImageAssist.LOpenCV.HashAlgorithms;
using ImageAssist.SupportedFunction;
using ImageAssist.SupportType;

namespace ImageAssist.LOpenCV
{
    internal class PerceptualHashing : IImageHash
    {
        public IImageHash Algorithm { get; }

        public PerceptualHashing(ESupportHashType supportHashType)
        {
            Algorithm = supportHashType switch
            {
                ESupportHashType.AverageHash => new AverageHash(),
                ESupportHashType.DifferenceHash => new DifferenceHash(),
                ESupportHashType.PerceptualHash => new PerceptualHash(),
                _ => new DifferenceHash()
            };
        }

        private ulong? _hash;

        public ulong GetHash<T>(ref T image, System.Drawing.Size? size = null) where T : class
        {
            _hash ??= Algorithm.GetHash(ref image, size);
            return (ulong)_hash;
        }

        public ulong GetHash<T>(T image, System.Drawing.Size? size = null) where T : class
        {
            return Algorithm.GetHash(ref image, size);
        }
    }
}
