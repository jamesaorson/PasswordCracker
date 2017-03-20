using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    static class PasswordCracker {
        private static MD5 md5 = MD5.Create();
        static void Main(string[] args) {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            
            string[] words = File.ReadAllText("../../bible.txt").Split((string[])null,
                             StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < words.Length; ++i) {
                string temp = hash(words[i]);
                if (!dict.ContainsKey(temp)) {
                    dict.Add(temp, words[i]);
                }
            }

            Console.WriteLine("done with " + dict.Count() + " words. ");
            Console.Read();
        }

        private static string hash(string input) {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}