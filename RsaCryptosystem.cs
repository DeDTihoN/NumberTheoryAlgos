using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static AlgorithmsTheoryNumber.Algorithms_NumberTheory;

namespace AlgorithmsTheoryNumber
{
    internal class RsaCryptosystem
    {
        public static Tuple<BigInteger, BigInteger, BigInteger, BigInteger> RsaKeys(BigInteger p,BigInteger q,
            BigInteger msg)
        {
            if (!isPrimeMiller(p) || !isPrimeMiller(q))
            {
                throw new ArgumentException("p or q is not prime");
            }

            // Stores the first part of public key:
            BigInteger n = p * q;

            // Finding the other part of public key.
            // double e stands for encrypt
            BigInteger e = 2;
            BigInteger phi = (p - 1) * (q - 1);
            while (e < phi)
            {
                /*
                 * e must be co-prime to phi and
                 * smaller than phi.
                 */
                if (gcd(e, phi) == 1)
                    break;
                else
                    e++;
            }

            BigInteger k = 2; // A constant value

            BigInteger d = RevByModByEcludian(e, phi);

            // Message to be encrypted

            Console.WriteLine("Message data = "
                              + msg);

            // Encryption c = (msg ^ e) % n
            BigInteger c = st(msg,e,n);
            Console.WriteLine("Encrypted data = "
                              + c);

            // Decryption m = (c ^ d) % n
            BigInteger m = st(c,d,n);
            Console.WriteLine("Original Message Sent = "
                              + m);
            return Tuple.Create(n, e, n, d);
        }
    }
}
