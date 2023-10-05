using ImageAssist.SupportedFunction;
using OpenCvSharp;

namespace ImageAssist.LOpenCV
{
    internal class QualityAndDifferential : IQualityAndDifferential
    {
        /// <summary>
        /// MSE
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        public double MeanSquaredError(ref Mat originalImage, ref Mat modifiedImage)
        {
            Mat diff = new();
            Cv2.Absdiff(originalImage, modifiedImage, diff);
            diff = diff.Mul(diff);
            Scalar mean = Cv2.Mean(diff);
            return mean.Val0;
        }

        /// <summary>
        /// PSNR
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        public double PeakSignalToNoiseRatio(ref Mat originalImage, ref Mat modifiedImage)
        {
            double mse = MeanSquaredError(ref originalImage, ref modifiedImage);
            double psnr = 10 * Math.Log10(255 * 255 / mse);
            return psnr;
        }

        /// <summary>
        /// SSIM
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        public double StructuralSimilarityIndexMeasure(ref Mat originalImage, ref Mat modifiedImage)
        {
            // Calculate the mean of the original _image
            Scalar meanOriginal = Cv2.Mean(originalImage);
            double meanOrig = meanOriginal.Val0;

            // Calculate the mean of the modified _image
            Scalar meanModified = Cv2.Mean(modifiedImage);
            double meanMod = meanModified.Val0;

            // Calculate the standard deviation of the original _image
            Mat diffOriginal = new Mat();
            Cv2.Absdiff(originalImage, meanOrig, diffOriginal);
            diffOriginal = diffOriginal.Mul(diffOriginal);
            Scalar meanDiffOrig = Cv2.Mean(diffOriginal);
            double stdOrig = Math.Sqrt(meanDiffOrig.Val0);

            // Calculate the standard deviation of the modified _image
            Mat diffModified = new Mat();
            Cv2.Absdiff(modifiedImage, meanMod, diffModified);
            diffModified = diffModified.Mul(diffModified);
            Scalar meanDiffMod = Cv2.Mean(diffModified);
            double stdMod = Math.Sqrt(meanDiffMod.Val0);

            // Calculate the covariance between the original and modified images
            Mat diff = new Mat();
            Cv2.Absdiff(originalImage, modifiedImage, diff);
            diff = diff.Mul(diff);
            Scalar meanDiff = Cv2.Mean(diff);
            double cov = meanDiff.Val0;

            // Calculate the SSIM score
            double ssim = (2 * meanOrig * meanMod + 0.01) * (2 * cov + 0.03) / ((meanOrig * meanOrig + meanMod * meanMod + 0.01) * (stdOrig * stdOrig + stdMod * stdMod + 0.03));
            return ssim;
        }

        /// <summary>
        /// DeltaE
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="modifiedImage"></param>
        /// <returns></returns>
        public double DeltaE(ref Mat originalImage, ref Mat modifiedImage)
        {
            Mat labOriginal = new Mat();
            Mat labModified = new Mat();
            Cv2.CvtColor(originalImage, labOriginal, ColorConversionCodes.BGR2Lab);
            Cv2.CvtColor(modifiedImage, labModified, ColorConversionCodes.BGR2Lab);

            Mat diff = new Mat();
            Cv2.Absdiff(labOriginal, labModified, diff);
            diff = diff.Mul(diff);
            Scalar mean = Cv2.Mean(diff);

            double deltaE = Math.Sqrt(mean.Val0 + mean.Val1 + mean.Val2);
            return deltaE;
        }
    }
}
