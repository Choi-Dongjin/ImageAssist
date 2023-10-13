namespace ImageAssist.CalculateHash
{
    public class JaccardSimilarity
    {
        public static double Similarity(ulong hash1, ulong hash2)
        {
            int commonBits = CountCommonBits(hash1, hash2);
            int totalBits = CountTotalBits(hash1, hash2);

            double jaccardSimilarity = (double)commonBits / totalBits;
            return jaccardSimilarity;
        }

        public static int CountCommonBits(ulong hash1, ulong hash2)
        {
            ulong commonBits = hash1 & hash2;
            int count = 0;

            while (commonBits > 0)
            {
                if ((commonBits & 1) == 1)
                {
                    count++;
                }
                commonBits >>= 1;
            }

            return count;
        }

        public static int CountTotalBits(ulong hash1, ulong hash2)
        {
            ulong totalBits = hash1 | hash2;
            int count = 0;

            while (totalBits > 0)
            {
                count++;
                totalBits >>= 1;
            }

            return count;
        }

    }
}
