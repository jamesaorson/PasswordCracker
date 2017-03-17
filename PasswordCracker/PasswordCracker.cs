using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    class PasswordCracker {
        static void Main(string[] args) {
            string input = Console.ReadLine();
            Console.WriteLine(hash(input));
        }

        private static string hash(string input) {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder hash = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; ++i) {
                hash.Append(hashBytes[i].ToString("x2"));
            }
            
            return hash.ToString();
        }
    }
}