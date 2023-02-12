using OpenCvSharp;

namespace ImageAssist
{
    public class ImageProcessor
    {
        private Mat _originalImage;
        private Mat _modifiedImage;

        public ImageProcessor(Mat originalImage, Mat modifiedImage)
        {
            _originalImage = originalImage;
            _modifiedImage = modifiedImage;
        }

        public double MeanSquaredError()
        {
            Mat diff = new Mat();
            Cv2.Absdiff(_originalImage, _modifiedImage, diff);
            diff = diff.Mul(diff);
            Scalar mean = Cv2.Mean(diff);
            return mean.Val0;
        }

        public double PeakSignalToNoiseRatio()
        {
            double mse = MeanSquaredError();
            double psnr = 10 * Math.Log10(255 * 255 / mse);
            return psnr;
        }

        public double StructuralSimilarityIndex()
        {
            // Calculate the mean of the original image
            Scalar meanOriginal = Cv2.Mean(_originalImage);
            double meanOrig = meanOriginal.Val0;

            // Calculate the mean of the modified image
            Scalar meanModified = Cv2.Mean(_modifiedImage);
            double meanMod = meanModified.Val0;

            // Calculate the standard deviation of the original image
            Mat diffOriginal = new Mat();
            Cv2.Absdiff(_originalImage, meanOrig, diffOriginal);
            diffOriginal = diffOriginal.Mul(diffOriginal);
            Scalar meanDiffOrig = Cv2.Mean(diffOriginal);
            double stdOrig = Math.Sqrt(meanDiffOrig.Val0);

            // Calculate the standard deviation of the modified image
            Mat diffModified = new Mat();
            Cv2.Absdiff(_modifiedImage, meanMod, diffModified);
            diffModified = diffModified.Mul(diffModified);
            Scalar meanDiffMod = Cv2.Mean(diffModified);
            double stdMod = Math.Sqrt(meanDiffMod.Val0);

            // Calculate the covariance between the original and modified images
            Mat diff = new Mat();
            Cv2.Absdiff(_originalImage, _modifiedImage, diff);
            diff = diff.Mul(diff);
            Scalar meanDiff = Cv2.Mean(diff);
            double cov = meanDiff.Val0;

            // Calculate the SSIM score
            double ssim = ((2 * meanOrig * meanMod + 0.01) * (2 * cov + 0.03)) / ((meanOrig * meanOrig + meanMod * meanMod + 0.01) * (stdOrig * stdOrig + stdMod * stdMod + 0.03));
            return ssim;
        }

        public double DeltaE()
        {
            Mat labOriginal = new Mat();
            Mat labModified = new Mat();
            Cv2.CvtColor(_originalImage, labOriginal, ColorConversionCodes.BGR2Lab);
            Cv2.CvtColor(_modifiedImage, labModified, ColorConversionCodes.BGR2Lab);

            Mat diff = new Mat();
            Cv2.Absdiff(labOriginal, labModified, diff);
            diff = diff.Mul(diff);
            Scalar mean = Cv2.Mean(diff);

            double deltaE = Math.Sqrt(mean.Val0 + mean.Val1 + mean.Val2);
            return deltaE;
        }
    }
}