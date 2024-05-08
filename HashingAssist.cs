using ImageAssist.DataFormat;
using ImageAssist.SupportedFunction;
using ImageAssist.SupportType;
using OpenCvSharp;
using System.Diagnostics;

namespace ImageAssist
{
    public class HashingAssist : IDisposable
    {
        #region IDisposable

        protected bool _disposed = false;

        ~HashingAssist() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) { return; }

            if (disposing) { }
            _analysisCTS?.Cancel();
            _disposed = true;
        }

        #endregion IDisposable

        /// <summary>
        /// 진행율
        /// </summary>
        public Action<float>? ProgressRate { get; init; }

        /// <summary>
        /// 진행 작업
        /// </summary>
        public Action<string>? ProgressWork { get; init; }

        /// <summary>
        /// 해싱 사이즈
        /// </summary>
        public System.Drawing.Size HashSize { get; init; }

        /// <summary>
        /// 해밍 거리를 비교할 기준 값 (임계값)을 설정합니다.
        /// </summary>
        public int HammingDistanceThreshold { get; init; }

        /// <summary>
        /// 해밍 거리 유사도 기준 값 을 설정 합니다. 
        /// </summary>
        public double HammingSimilarityThreshold { get; init; }

        /// <summary>
        /// 지원 라이브러리
        /// </summary>
        public LType SupportLibrary { get; init; }

        /// <summary>
        /// 지원 해쉬 타입
        /// </summary>
        public ESupportHashType SupportHashType { get; init; }

        /// <summary>
        /// 유사도 분석 방법
        /// </summary>
        public EHashAnalysisType HashAnalysis { get; init; }

        /// <summary>
        /// 이미지 해싱 진행 확인
        /// </summary>
        public bool IsRunImageHash { get; private set; } = false;

        /// <summary>
        /// 이미지 해싱 완료 확인
        /// </summary>
        public bool IsImageHash { get; private set; } = false;

        /// <summary>
        /// 분석 진행중 확인
        /// </summary>
        public bool IsRunAnalysis { get; private set; } = false;

        /// <summary>
        /// 분석 완료 확인
        /// </summary>
        public bool IsAnalysis { get; private set; } = false;

        /// <summary>
        /// 이미지 해쉬 결과. Key: 이미지 Path, Value: 이미지 해쉬 값
        /// </summary>
        private readonly List<DImageHash> _imageHashes = new();

        /// <summary>
        /// 이미지를 그룹화할 딕셔너리를 생성합니다. 
        /// Key: Image Hash, Value: IamgeFullPaths
        /// </summary>
        private readonly Dictionary<string, List<string>> _imageGroups = new();

        /// <summary>
        /// 해싱 알고리즘
        /// </summary>
        private readonly IImageHash _imageHash;

        /// <summary>
        /// 이미지 해싱 Task 제어 CTS
        /// </summary>
        private CancellationTokenSource? _imageHashCTS;

        /// <summary>
        /// 분석 Task 제어 CTS
        /// </summary>
        private CancellationTokenSource? _analysisCTS;

        /// <summary>
        /// locker
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// 진행 상황 출력
        /// </summary>
        private float _progressRate;

        public HashingAssist(System.Drawing.Size hashSize, int hammingDistance, ESupportHashType supportHashType, LType supportLibrary, EHashAnalysisType hashAnalysis)
        {
            HashSize = hashSize;
            HammingDistanceThreshold = hammingDistance;
            HammingSimilarityThreshold = 0.99D;
            SupportLibrary = supportLibrary;
            SupportHashType = supportHashType;
            HashAnalysis = hashAnalysis;
            _imageHash = SupportLibrary switch
            {
                LType.OpenCV => new LOpenCV.PerceptualHashing(SupportHashType),
                LType.ImageSharp => new LOpenCV.PerceptualHashing(SupportHashType),
                _ => new LOpenCV.PerceptualHashing(SupportHashType)
            };
        }

        public HashingAssist(HashingAssistOption hashingAssistOption)
        {
            HashSize = hashingAssistOption.HashSize;
            HammingDistanceThreshold = hashingAssistOption.HammingDistanceThreshold;
            HammingSimilarityThreshold = hashingAssistOption.HammingSimilarityThreshold;
            SupportLibrary = hashingAssistOption.SupportLibrary;
            SupportHashType = hashingAssistOption.SupportHashType;
            HashAnalysis = hashingAssistOption.HashAnalysisType;
            _imageHash = SupportLibrary switch
            {
                LType.OpenCV => new LOpenCV.PerceptualHashing(SupportHashType),
                LType.ImageSharp => new LOpenCV.PerceptualHashing(SupportHashType),
                _ => new LOpenCV.PerceptualHashing(SupportHashType)
            };
        }

        /// <summary>
        /// 해싱 시작
        /// </summary>
        /// <returns></returns>
        public async Task RunImageHash(List<string> dirs)
        {
            bool run = false;
            lock (_lock)
            {
                if (_imageHashCTS == null)
                {
                    _imageHashCTS = new CancellationTokenSource();
                    run = true;
                }
            }

            if (!run) { return; }

            lock (_lock) { IsImageHash = false; IsRunImageHash = true; }

            ProgressWork?.Invoke("[Hashing] Image Serching. ***");
            List<string> images = await GetImagePaths(dirs, _imageHashCTS.Token);

            ProgressWork?.Invoke("[Hashing] Proceeding. ***");
            void hashing(CancellationToken token)
            {
                int hashCount = 0;
                foreach (string image in images)
                {
                    if (_imageHashCTS.IsCancellationRequested) { break; }

                    _imageHashes.Add(new()
                    {
                        FilePath = image,
                        Hash = ImageHashing(image),
                    });

                    if (ProgressRate != null)
                    {
                        ProgressPrint(++hashCount, images.Count);
                    }
                }
            }
            await Task.Run(() => hashing(_imageHashCTS.Token));

            ProgressWork?.Invoke("[Hashing] Complete. ***");
            lock (_lock) { IsImageHash = true; IsRunImageHash = false; }
        }

        /// <summary>
        /// 해싱 시작
        /// </summary>
        /// <returns></returns>
        public async Task RunImageHash(List<(string, Mat)> mats)
        {
            bool run = false;
            lock (_lock)
            {
                if (_imageHashCTS == null)
                {
                    _imageHashCTS = new CancellationTokenSource();
                    run = true;
                }
            }

            if (!run) { return; }

            lock (_lock) { IsImageHash = false; IsRunImageHash = true; }

            ProgressWork?.Invoke("[Hashing] Image Serching. ***");

            ProgressWork?.Invoke("[Hashing] Proceeding. ***");
            void hashing(CancellationToken token)
            {
                int hashCount = 0;
                foreach ((string fileName, Mat image) in mats)
                {
                    if (_imageHashCTS.IsCancellationRequested) { break; }

                    _imageHashes.Add(new()
                    {
                        FilePath = fileName,
                        Hash = ImageHashing(image),
                    });

                    if (ProgressRate != null)
                    {
                        ProgressPrint(++hashCount, mats.Count);
                    }
                }
            }
            await Task.Run(() => hashing(_imageHashCTS.Token));

            ProgressWork?.Invoke("[Hashing] Complete. ***");
            lock (_lock) { IsImageHash = true; IsRunImageHash = false; }
        }

        /// <summary>
        /// 분석 시작
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>?> RunAnalysis()
        {
            bool run = false;
            lock (_lock) // CTS 확인
            {
                if (_analysisCTS == null) // 초기 분석 시작
                {
                    run = true;
                    _analysisCTS = new CancellationTokenSource();
                }

                if (IsRunAnalysis == false && IsAnalysis == true) // 분석 결과 변경
                {
                    run = true;
                }

                if (IsImageHash == false) // 해싱 완료 확인
                {
                    run = false;
                }
            }
            if (!run)
            {
                bool returnValue = false;
                lock (_lock)
                {
                    if (IsAnalysis) { returnValue = true; }
                }
                if (returnValue) { return _imageGroups; }
                return null;
            }
            lock (_lock) { IsAnalysis = false; IsRunAnalysis = true; }

            ProgressWork?.Invoke("[ImageAnalysis] Proceeding. ***");

            void analysis(CancellationToken token)
            {
                int analysisCount = 0;
                foreach (DImageHash image in _imageHashes)
                {
                    if (token.IsCancellationRequested) { break; }
                    ImageAnalysis(image, _analysisCTS.Token);
                    if (ProgressRate != null) { ProgressPrint(++analysisCount, _imageHashes.Count); } // 진행상황 출력
                }
            }
            await Task.Run(() => analysis(_analysisCTS.Token));

            ProgressRate?.Invoke(100F);
            ProgressWork?.Invoke("[ImageAnalysis] Complete. ***");
            lock (_lock) { IsAnalysis = true; IsRunAnalysis = false; }
            return _imageGroups;
        }

        /// <summary>
        /// 이미지 파일 검색
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static async Task<List<string>> GetImagePaths(IEnumerable<string> imageDir, CancellationToken ct)
        {
            List<string> imagePaths = new();
            List<string> ext = Enum.GetNames(typeof(ESupportedExtensions)).ToList();
            ext.Add("JPG");
            foreach (var workingDirectory in imageDir)
            {
                imagePaths.AddRange(await Task.Run(() => FileIOAssist.Extension.DirFileSearch(workingDirectory, ext.ToArray(), FileIOAssist.Extension.GetNameType.Full, FileIOAssist.Extension.SubSearch.Full, ct)));
            }
            return imagePaths;
        }

        /// <summary>
        /// 이미지 해싱
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ulong ImageHashing(string filePath)
        {
            string imagefilePath = filePath;
            // 이미지를 로드합니다.
            Mat image = Cv2.ImRead(imagefilePath, ImreadModes.Grayscale);
            // 이미지를 DHash로 해싱합니다.
            return _imageHash.GetHash(image, null);
        }

        private ulong ImageHashing(Mat mat)
        {
            // 이미지를 DHash로 해싱합니다.
            return _imageHash.GetHash(mat, null);
        }

        /// <summary>
        /// 이미지 분석
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ct"></param>
        private void ImageAnalysis(DImageHash imageHash, CancellationToken ct)
        {
            // 파일 경로
            string imagefilePath = imageHash.FilePath;

            // 이미지를 DHash로 해싱합니다.
            ulong dhash = imageHash.Hash;

            // 이미지를 다른 그룹에 할당할지 결정합니다.
            double pHashAnalysisSimilarity = HammingSimilarityThreshold;
            double pHashAnalysisDistance = HammingDistanceThreshold;
            string addKey = "";
            foreach (var groupKey in _imageGroups.Keys)
            {
                if (ct.IsCancellationRequested) { break; }

                ulong groupDHash = ulong.Parse(groupKey);
                bool pass = false;

                switch (HashAnalysis)
                {
                    case EHashAnalysisType.HammingSimilarity:
                        {
                            double hashAnalysis = CalculateHash.HammingDistance.Similarity(dhash, groupDHash);
                            // 유사도 이용
                            if (hashAnalysis >= HammingSimilarityThreshold)
                            {
                                if (pHashAnalysisSimilarity < hashAnalysis)
                                {
                                    pHashAnalysisSimilarity = hashAnalysis;
                                    addKey = groupKey;
                                }
                            }
                        }
                        break;
                    default:
                        {
                            double hashAnalysis = CalculateHash.HammingDistance.Distance(dhash, groupDHash);
                            // 해밍 거리가 임계값 이내인 경우 이미지를 그룹에 할당합니다.
                            if (hashAnalysis < HammingDistanceThreshold)
                            {
                                if (pHashAnalysisDistance > hashAnalysis)
                                {
                                    pHashAnalysisDistance = hashAnalysis;
                                    addKey = groupKey;
                                }
                            }
                        }
                        break;
                }
                if (pass == true) { break; }
            }

            if (string.IsNullOrEmpty(addKey))
            {
                string groupKey = dhash.ToString();
                if (!_imageGroups.TryAdd(groupKey, new List<string> { imagefilePath }))
                {
                    _imageGroups[groupKey].Add(imagefilePath);
                }
            }
            else
            {
                _imageGroups[addKey].Add(imagefilePath);
            }
        }

        private void ImageAnalysiGroups()
        {

        }

        /// <summary>
        /// 이미지 저장 TEST
        /// </summary>
        /// <param name="saveDirectory"></param>
        public void TestFileSave(string saveDirectory)
        {
            // 그룹화된 이미지 출력 또는 저장
            foreach (var kvp in _imageGroups)
            {
                Debug.WriteLine($"그룹: {kvp.Key}");
                string imageDir = Path.Combine(saveDirectory, kvp.Key);

                foreach (string imagePath in kvp.Value)
                {
                    Directory.CreateDirectory(imageDir);
                    string fileName = Path.GetFileName(imagePath);
                    FileIOAssist.Assist.FileCopy(imagePath, Path.Combine(imageDir, fileName), out string _);
                    Debug.WriteLine(imagePath);
                }
                Debug.WriteLine("");
            }
        }

        /// <summary>
        /// 진행상황 출력
        /// </summary>
        /// <param name="progressCount"> 진행 카운트 </param>
        /// <param name="maxCount"> 총 작업 수 </param>
        private void ProgressPrint(int progressCount, int maxCount)
        {
            float newRate = (float)Math.Round((double)progressCount / maxCount, 1);
            ProgressPrint(newRate);
        }

        /// <summary>
        /// 진행상황 출력
        /// </summary>
        /// <param name="newRate"> 출력 비율 </param>
        private void ProgressPrint(float newRate)
        {
            if (!newRate.Equals(_progressRate))
            {
                _progressRate = newRate;
                ProgressRate?.Invoke(newRate);
            }
        }
    }
}
