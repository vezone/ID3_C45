#define Debug

namespace ID3.src.ID3.v2
{
    public class Additional
    {
        public static double Entropy(int numberOfPositive, int numberOfNegative)
        {
            double entropy = 0.0;

            if (numberOfPositive == 0 || numberOfNegative == 0)
                return entropy;

            int sum = numberOfPositive + numberOfNegative;
            double positive = ((double)numberOfPositive) / sum;
            double negative = ((double)numberOfNegative) / sum;

            entropy = - positive * System.Math.Log(positive, 2.0) - negative * System.Math.Log(negative, 2.0);

            return entropy;
        }
        public static void Log(object obj)
        {
#if Debug
            System.Console.WriteLine(obj);
#endif
        }
        public static double SplitInfo(int numberOfPositive, int numberOfNegative)
        {
            double splitInfo = 0.0;

            if (numberOfPositive == 0 || numberOfNegative == 0)
                return splitInfo;

            int sum = numberOfPositive + numberOfNegative;
            double positive = ((double)numberOfPositive) / sum;
            
            splitInfo = positive * System.Math.Log(positive, 2.0);

            return System.Math.Abs(splitInfo);
        }
    }
}
