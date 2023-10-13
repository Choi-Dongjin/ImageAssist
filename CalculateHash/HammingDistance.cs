namespace ImageAssist.CalculateHash
{
    public class HammingDistance
    {
        public static int Distance(ulong hash1, ulong hash2)
        {
            ulong xor = hash1 ^ hash2;
            int distance = 0;
            while (xor != 0)
            {
                distance++;
                xor &= (xor - 1);
            }
            return distance;
        }

        public static double Similarity(ulong hash1, ulong hash2)
        {
            int hammingDistance = Distance(hash1, hash2);
            int maxHammingDistance = 64; // 64비트 해시 값의 경우

            double similarity = 1 - (double)hammingDistance / maxHammingDistance;
            return similarity;
        }
    }
}
