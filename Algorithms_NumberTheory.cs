using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsTheoryNumber
{
    static public class Algorithms_NumberTheory
    {
        static Random random = new Random();
        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        static public BigInteger st(BigInteger n, BigInteger k, BigInteger mod)
        {
            BigInteger res = 1;
            while (k > 0)
            {
                if (k % 2 == 1) res = (res * n) % mod;
                n = (n * n) % mod;
                k /= 2;
            }
            return res;
        }

        static public BigInteger RevByMod(BigInteger n, BigInteger mod)
        {
            return st(n, phi(mod) - 1, mod);
        }

        static BigInteger GcdExtended(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            x = 1;
            y = 0;
            BigInteger x1 = 0, y1 = 1, a1 = a, b1 = b;

            while (b1 != 0)
            {
                BigInteger q = a1 / b1;
                (x, x1) = (x1, x - q * x1);
                (y, y1) = (y1, y - q * y1);
                (a1, b1) = (b1, a1 - q * b1);
            }

            return a1;
        }

        static public BigInteger RevByModByEcludian(BigInteger n,BigInteger mod)
        {
            BigInteger x, y;
            BigInteger g = GcdExtended(n, mod, out x, out y);
            if (g != 1)
                throw new Exception("No solution!");
            else
            {
                BigInteger res = (x % mod + mod) % mod;
                return res;
            }
        }

        static public BigInteger phi(BigInteger n)
        {
            BigInteger result = n;
            for (BigInteger i = 2; i * i <= n; ++i)
                if (n % i == 0)
                {
                    while (n % i == 0)
                        n /= i;
                    result -= result / i;
                }
            if (n > 1)
                result -= result / n;
            return result;
        }

        static public bool IsPrimeSimple(BigInteger n)
        {
            if (n < 2)
                return false;
            for (BigInteger i = 2; i * i <= n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }

        static public BigInteger MobiusStraight(BigInteger N)
        {
            if (N == 1)
                return 1;
            int p = 0;
            for (BigInteger i = 1; i * i <= N; i++)
            {
                if (N % i == 0 && IsPrimeSimple(i))
                {
                    if (N % (i * i) == 0)
                        return 0;
                    else
                        p++;
                }
            }
            return (p % 2 != 0) ? -1 : 1;
        }

        static public BigInteger gcd(BigInteger a, BigInteger b)
        {
            if (a == 0)
                return b;
            return gcd(b % a, a);
        }
        static public BigInteger lcm(BigInteger a, BigInteger b)
        {
            return (a / gcd(a, b)) * b;
        }

        static public BigInteger ChineseRemainderSolution(BigInteger[] num,
                                BigInteger[] rem,
                                BigInteger k)
        {
            // Compute product  
            // of all numbers  
            BigInteger prod = 1;
            for (int i = 0; i < k; i++)
                prod *= num[i];

            // Initialize result  
            BigInteger result = 0;

            // Apply above formula  
            for (int i = 0; i < k; i++)
            {
                BigInteger pp = prod / num[i];
                BigInteger inv = RevByMod(pp, num[i]);
                result += rem[i] * inv * pp;

            }

            return result % prod;
        }

        static public BigInteger JacobiSymbol(BigInteger n, BigInteger k)
        {
            if (k < 0 || k % 2 == 0) throw new Exception("wrong argument");
            n %= k;
            BigInteger t = 1;
            while (n != 0)
            {
                while (n % 2 == 0)
                {
                    n /= 2;
                    BigInteger r = k % 8;
                    if (r == 3 || r == 5)
                        t = -t;
                }
                Swap(ref n, ref k);
                if (n % 4 == 3 && k % 4 == 3)
                    t = -t;
                n %= k;
            }
            return k == 1 ? t : 0;
        }

        static public BigInteger LegangreSymbol(BigInteger a, BigInteger p)
        {
            if (p==2 || !IsPrimeSimple(p)) throw new Exception("not odd prime p");
            if (a % p == 0) return 0;
            BigInteger res = st(a, (p - 1) / 2, p);
            if (res == p - 1) res = -1;
            return res;
        }
        static public BigInteger DiscreteAlgorithm(BigInteger a, BigInteger b, long  m)
        {
            a = BigInteger.Remainder(a, m);
            b = BigInteger.Remainder(b, m);
            BigInteger n = (long)Math.Sqrt(m) + 1;
            Dictionary<BigInteger, BigInteger> vals = new Dictionary<BigInteger, BigInteger>();

            for (BigInteger p = 1; p <= n; ++p)
                vals[st(a, p * n, m)] = p;

            for (BigInteger q = 0; q <= n; ++q)
            {
                BigInteger cur = BigInteger.Remainder(BigInteger.Multiply(st(a, q, m), b), m);
                if (vals.ContainsKey(cur))
                {
                    BigInteger ans = vals[cur] * n - q;
                    ans= BigInteger.Remainder(ans, phi(m));
                    return ans;
                }
            }

            return -1;
        }

        public static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }

        public static bool miillerTest(BigInteger d, BigInteger n)
        {

            // Pick a random number in [2..n-2]
            // Corner cases make sure that n > 4
            Random r = new Random();
            BigInteger a = 2 + RandomIntegerBelow(n - 4);

            // Compute a^d % n
            BigInteger x = st(a, d, n);

            if (x == 1 || x == n - 1)
                return true;

            // Keep squaring x while one of the
            // following doesn't happen
            // (i) d does not reach n-1
            // (ii) (x^2) % n is not 1
            // (iii) (x^2) % n is not n-1
            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;

                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            // Return composite
            return false;
        }

        public static bool isPrimeMiller(BigInteger n, int k=10)
        {

            // Corner cases
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;

            // Find r such that n = 2^d * r + 1 
            // for some r >= 1
            BigInteger d = n - 1;

            while (d % 2 == 0)
                d /= 2;

            // Iterate given number of 'k' times
            for (int i = 0; i < k; i++)
                if (miillerTest(d, n) == false)
                    return false;

            return true;
        }

    }
}
