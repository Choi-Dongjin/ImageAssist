﻿using ImageAssist.SupportedFunction;
using ImageAssist.Utile;
using OpenCvSharp;

namespace ImageAssist.LOpenCV;

/// <summary>
/// 이미지 퀄리티 & 차이점
/// </summary>
internal class QualityAndDifferential : IQualityAndDifferential
{
    /// <summary>
    /// MSE
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="modifiedImage"></param>
    /// <returns></returns>
    public static double MeanSquaredError(ref Mat originalImage, ref Mat modifiedImage)
    {
        Mat diff = new();
        Cv2.Absdiff(originalImage, modifiedImage, diff);
        diff = diff.Mul(diff);
        Scalar mean = Cv2.Mean(diff);
        diff.Dispose();
        return mean.Val0;
    }

    /// <summary>
    /// PSNR
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="modifiedImage"></param>
    /// <returns></returns>
    public static double PeakSignalToNoiseRatio(ref Mat originalImage, ref Mat modifiedImage)
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
    public static double StructuralSimilarityIndexMeasure(ref Mat originalImage, ref Mat modifiedImage)
    {
        // Calculate the mean of the original _image
        Scalar meanOriginal = Cv2.Mean(originalImage);
        double meanOrig = meanOriginal.Val0;

        // Calculate the mean of the modified _image
        Scalar meanModified = Cv2.Mean(modifiedImage);
        double meanMod = meanModified.Val0;

        // Calculate the standard deviation of the original _image
        Mat diffOriginal = new();
        Cv2.Absdiff(originalImage, meanOrig, diffOriginal);
        diffOriginal = diffOriginal.Mul(diffOriginal);
        Scalar meanDiffOrig = Cv2.Mean(diffOriginal);
        double stdOrig = Math.Sqrt(meanDiffOrig.Val0);

        // Calculate the standard deviation of the modified _image
        Mat diffModified = new();
        Cv2.Absdiff(modifiedImage, meanMod, diffModified);
        diffModified = diffModified.Mul(diffModified);
        Scalar meanDiffMod = Cv2.Mean(diffModified);
        double stdMod = Math.Sqrt(meanDiffMod.Val0);

        // Calculate the covariance between the original and modified images
        Mat diff = new();
        Cv2.Absdiff(originalImage, modifiedImage, diff);
        diff = diff.Mul(diff);
        Scalar meanDiff = Cv2.Mean(diff);
        double cov = meanDiff.Val0;

        // Calculate the SSIM score
        double ssim = (2 * meanOrig * meanMod + 0.01) * (2 * cov + 0.03) / ((meanOrig * meanOrig + meanMod * meanMod + 0.01) * (stdOrig * stdOrig + stdMod * stdMod + 0.03));

        // Dispose
        diffOriginal.Dispose();
        diffModified.Dispose();
        diff.Dispose();

        return ssim;
    }

    /// <summary>
    /// DeltaE
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="modifiedImage"></param>
    /// <returns></returns>
    public static double DeltaE(ref Mat originalImage, ref Mat modifiedImage)
    {
        Mat labOriginal = new();
        Mat labModified = new();
        Cv2.CvtColor(originalImage, labOriginal, ColorConversionCodes.BGR2Lab);
        Cv2.CvtColor(modifiedImage, labModified, ColorConversionCodes.BGR2Lab);

        Mat diff = new();
        Cv2.Absdiff(labOriginal, labModified, diff);
        diff = diff.Mul(diff);
        Scalar mean = Cv2.Mean(diff);

        // Dispose
        labOriginal.Dispose();
        labModified.Dispose();

        double deltaE = Math.Sqrt(mean.Val0 + mean.Val1 + mean.Val2);
        return deltaE;
    }

    double IQualityAndDifferential.MeanSquaredError(ref object originalImage, ref object modifiedImage)
    {
        if (originalImage is not Mat convertOrgImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), originalImage.GetType()); }
        if (modifiedImage is not Mat convertModImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), modifiedImage.GetType()); }

        double value = MeanSquaredError(ref convertOrgImage, ref convertModImage);
        convertOrgImage.Dispose();
        convertModImage.Dispose();
        return value;
    }

    double IQualityAndDifferential.PeakSignalToNoiseRatio(ref object originalImage, ref object modifiedImage)
    {
        if (originalImage is not Mat convertOrgImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), originalImage.GetType()); }
        if (modifiedImage is not Mat convertModImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), modifiedImage.GetType()); }
        double value = PeakSignalToNoiseRatio(ref convertOrgImage, ref convertModImage);
        convertOrgImage.Dispose();
        convertModImage.Dispose();
        return value;
    }

    double IQualityAndDifferential.StructuralSimilarityIndexMeasure(ref object originalImage, ref object modifiedImage)
    {
        if (originalImage is not Mat convertOrgImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), originalImage.GetType()); }
        if (modifiedImage is not Mat convertModImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), modifiedImage.GetType()); }
        double value = StructuralSimilarityIndexMeasure(ref convertOrgImage, ref convertModImage);
        convertOrgImage.Dispose();
        convertModImage.Dispose();
        return value;
    }

    double IQualityAndDifferential.DeltaE(ref object originalImage, ref object modifiedImage)
    {
        if (originalImage is not Mat convertOrgImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), originalImage.GetType()); }
        if (modifiedImage is not Mat convertModImage)
        { return (ulong)ErrorThrowHelper.InputTypeError(new Mat(), modifiedImage.GetType()); }
        double value = DeltaE(ref convertOrgImage, ref convertModImage);
        convertOrgImage.Dispose();
        convertModImage.Dispose();
        return value;
    }
}