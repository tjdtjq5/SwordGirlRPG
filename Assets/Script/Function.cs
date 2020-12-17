using System.Numerics;
using System.Threading;
using UnityEngine;

namespace Function
{
    public static class MyMath
    {
        //큰 수를 요약해서 보여줌 
        public static string ValueToString(string value)
        {
            int su = 4; // 몇 단위로 수를 끊을 건지 
            int len = value.Length;
            if (len <= su)
            {
                return value;
            }

            // enum타입이 없을 경우 
            if (System.Enum.GetValues(typeof(CountType)).Length < (len / su - 1)) return value;

            CountType countType = (CountType)(len / su - 1);
            int remainLenth = len % su;

            // 자리수가 딱 떨어지는 경우 
            if(remainLenth == 0)
            {
                countType = (CountType)((int)countType - 1);
                value = value.Substring(0, su*2);
                value = value.Insert(4, countType.ToString());
                return value;
            }
            else
            {
                value = value.Substring(0, remainLenth + su);
                value = value.Insert(remainLenth, countType.ToString());
                return value;
            }
        }

        public static string Add(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Add(intA, intB).ToString();
        }
        public static string Sub(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Subtract(intA, intB).ToString();
        }
        public static string Multiple(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Multiply(intA, intB).ToString();
        }
        public static string Divide(string A, string B)
        {
            BigInteger intA = BigInteger.Parse(A);
            BigInteger intB = BigInteger.Parse(B);

            return BigInteger.Divide(intA, intB).ToString();
        }

        // 방치형 RPG 계산 공식
        public static string Fomula01(int level, int defaultValue, int A, int B)
        {
            if (level <= 0) return defaultValue.ToString();

            if (level > 1000) level = 1000; 

            BigInteger intA = new BigInteger(A);
            BigInteger intB = new BigInteger(B);
            BigInteger intBPow = 1;
            for (int i = 0; i < level - 1; i++)
            {
                intBPow *= intB;
            }

            string result = (intA * intBPow).ToString();
            result = Add(result, Fomula01(level - 1, defaultValue, A, B));
            return result;
        }
        public static string Fomula02(int level, int defaultValue, int A, int B)
        {
            if (level <= 0) return defaultValue.ToString();

            if (level > 3000) level = 3000;

            BigInteger intA = new BigInteger(A);
            BigInteger intB = new BigInteger(B);
            BigInteger intBPow = intB * (level - 1);

            string result = (intA + intBPow).ToString();
            result = Add(result, Fomula01(level - 1, defaultValue, A, B));
            return result;
        }
        public static string Fomula03(int level, int defaultValue, float percent)
        {
            if (level == 0) return defaultValue.ToString();
            if (level > 3000) level = 3000;

            string pString = ((int)(percent * 100)).ToString();
            string preResult = Fomula03(level - 1, defaultValue, percent);
            string result = Add(preResult, (Divide(preResult, pString)));

            return result;
        }
    }
}