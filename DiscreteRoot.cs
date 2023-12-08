using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;   

namespace AlgorithmsTheoryNumber
{
    internal static class DiscreteRoot
    {
        static BigInteger Gcd(BigInteger a, BigInteger b)
        {
            return a != 0 ? Gcd(b % a, a) : b;
        }

        static BigInteger PowerMod(BigInteger a, BigInteger b, BigInteger p)
        {
            BigInteger res = 1;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    res = (res * a) % p;
                }
                a = (a * a) % p;
                b /= 2;
            }
            return res;
        }

        static BigInteger Generator(BigInteger p)
        {
            List<BigInteger> fact = new List<BigInteger>();
            BigInteger phi = p - 1, n = phi;
            for (BigInteger i = 2; i * i <= n; ++i)
            {
                if (n % i == 0)
                {
                    fact.Add(i);
                    while (n % i == 0)
                        n /= i;
                }
            }
            if (n > 1)
                fact.Add(n);

            for (BigInteger res = 2; res <= p; ++res)
            {
                bool ok = true;
                foreach (BigInteger factor in fact)
                {
                    if (PowerMod(res, phi / factor, p) == 1)
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok) return res;
            }
            return -1;
        }

        public static List<BigInteger> DiscreteRootAlg(BigInteger n, BigInteger k, BigInteger a)
        {
            if (a == 0)
            {
                return new List<BigInteger> { 0 };
            }

            BigInteger g = Generator(n);
            int sq = (int)Math.Sqrt((double)n) + 1;
            List<(BigInteger, BigInteger)> dec = new List<(BigInteger, BigInteger)>(sq);
            for (BigInteger i = 1; i <= sq; ++i)
            {
              //  Console.WriteLine($"{(i * sq * k) % (n - 1)} + {PowerMod(g, (i * sq * k) % (n - 1), n)}");
                dec.Add((PowerMod(g, (i * sq * k) % (n - 1), n), i));
            }

            dec.Sort();

            BigInteger anyAns = -1;
            for (int i = 0; i < sq; ++i)
            {
                BigInteger my = PowerMod(g, (i * k) % (n - 1), n) * a % n;
                var it = dec.BinarySearch((my, BigInteger.Zero), new Comparer());
                if (it >= 0)
                {
                    anyAns = dec[it].Item2 * sq - i;
                    break;
                }
            }

            if (anyAns == -1)
            {
                return new List<BigInteger>();
            }

            BigInteger delta = (n - 1) / Gcd(k, n - 1);
            List<BigInteger> ans = new List<BigInteger>();
            for (BigInteger cur = anyAns % delta; cur < n - 1; cur += delta)
                ans.Add(PowerMod(g, cur, n));

            ans.Sort();
            return ans;
        }

        class Comparer : IComparer<(BigInteger, BigInteger)>
        {
            public int Compare((BigInteger, BigInteger) x, (BigInteger, BigInteger) y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}
