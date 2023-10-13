using ImageAssist.SupportType;

namespace ImageAssist.DataFormat
{
    public class HashingAssistOption
    {
        /// <summary>
        /// 해시 필터 사이즈
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
        /// 지원 해시 타입
        /// </summary>
        public ESupportHashType SupportHashType { get; init; }

        /// <summary>
        /// 유사도 분석 방법
        /// </summary>
        public EHashAnalysisType HashAnalysisType { get; init; }
    }
}
