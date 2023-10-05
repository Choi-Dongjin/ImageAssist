using OpenCvSharp;

namespace ImageAssist.SupportedFunction
{
    /// <summary>
    /// 이미지 품질 및 차이 평가.
    /// </summary>
    internal interface IQualityAndDifferential
    {
        /// <summary>
        /// MSE
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double MeanSquaredError(ref Mat originalImage, ref Mat modifiedImage);
        /// <summary>
        /// PSNR
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double PeakSignalToNoiseRatio(ref Mat originalImage, ref Mat modifiedImage);
        /// <summary>
        /// SSIM
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double StructuralSimilarityIndexMeasure(ref Mat originalImage, ref Mat modifiedImage);
        /// <summary>
        /// DeltaE
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double DeltaE(ref Mat originalImage, ref Mat modifiedImage);
    }
}
