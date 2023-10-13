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
        double MeanSquaredError(ref object originalImage, ref object modifiedImage);
        /// <summary>
        /// PSNR
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double PeakSignalToNoiseRatio(ref object originalImage, ref object modifiedImage);
        /// <summary>
        /// SSIM
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double StructuralSimilarityIndexMeasure(ref object originalImage, ref object modifiedImage);
        /// <summary>
        /// DeltaE
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        double DeltaE(ref object originalImage, ref object modifiedImage);
    }
}
