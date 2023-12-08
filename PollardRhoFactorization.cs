using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsTheoryNumber;
using static AlgorithmsTheoryNumber.Algorithms_NumberTheory;

namespace AlgorithmsTheoryNumber
{
    public class PollardRhoFactorization
    {
        Random Random = new Random();
        static BigInteger mult(BigInteger a, BigInteger b, BigInteger mod)
        {
            return (a * b) % mod;
        }
        static BigInteger f(BigInteger x, BigInteger c, BigInteger mod)
        {
            return (mult(x, x, mod) + c) % mod;
        }

        Dictionary<BigInteger, int> Composites;
        public PollardRhoFactorization()
        {
            Composites = new Dictionary<BigInteger, int>();
            Composites.Add(4, 2);
            Composites.Add(6, 2);
            Composites.Add(8, 2);
            Composites.Add(9, 3);
            Composites.Add(10, 2);
            Composites.Add(12, 2);
            Composites.Add(14, 2);
        }


        public List<BigInteger> PollardRho(BigInteger n,int depth=0)
        {
            if (n==1)return new List<BigInteger> { };
            if (isPrimeMiller(n))return new List<BigInteger>() { n };
            BigInteger c = Random.Next(0, 10);
            BigInteger x = Random.Next(0, 10);
            BigInteger y = x;
            BigInteger g = 1;
            while (g == 1)
            {
                x = f(x, c, n);
                y = f(y, c, n);
                y = f(y, c, n);
                g = gcd(BigInteger.Abs(x - y), n);
            }
            if (n % 2 == 0) g = 2;
            if (g == n)
            {
                return PollardRho(n, depth+1);
            }
            else
            {
                List<BigInteger> t1 = PollardRho(g, 0);
                List<BigInteger> t2 = PollardRho(n / g, 0);
                List<BigInteger>res = t1.Concat(t2).ToList();
                res.Sort();
                return res;
            }
        }


    }
}
