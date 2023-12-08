using System;
using System.Numerics;
using System.Globalization;
using AlgorithmsTheoryNumber;
using static AlgorithmsTheoryNumber.Algorithms_NumberTheory;
using System.Collections.Specialized;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
class Program 
{
    static async Task Main()
    {
        // test for prime RsaCryptoSystem
        BigInteger p = BigInteger.Parse("38915677910859092389");
        BigInteger q = BigInteger.Parse("63270625592967682411");
        BigInteger msg = BigInteger.Parse("35764868468357357346246");

        var keys = RsaCryptosystem.RsaKeys(p, q, msg);

        Console.WriteLine($"Rsa public key {keys.Item1}, {keys.Item2}");
        Console.WriteLine($"Rsa private key {keys.Item3}, {keys.Item4}");

    }
}