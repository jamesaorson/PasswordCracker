using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    static class PasswordCracker {
        //Holds dictionary of words in format of <hash, word>.
        private static Dictionary<string, string> dict
                 = new Dictionary<string, string>();
        //Performs MD5 hashing.
        private static MD5 md5 = MD5.Create();

        static void Main(string[] args) {
            //Splits bible.txt into a string[] of individual lowercase tokens.
            string[] dictWords = File.ReadAllText("../../bible.txt")
                                .ToLower().Split((string[])null,
                                StringSplitOptions.RemoveEmptyEntries);
            
            //Returns the array with duplicates removed (For program
            //speed purposes).
            dictWords = dictWords.Distinct().ToArray();

            //Hash and insert words from bible.txt
            for (int i = 0; i < dictWords.Length; ++i) {
                dict.Add(hash(dictWords[i]), dictWords[i]);
            }

            Console.WriteLine("done with " + dict.Count() + " words. ");

            StringReader reader = new StringReader(
                                  File.ReadAllText("../../pa4hashes.txt"));
            string result = "";

            while (reader.Peek() != -1) {
                string line = reader.ReadLine();

                if (line.Contains(":")) {
                    int firstColonPos = line.IndexOf(':');
                    int secondColonPos = line.Substring(firstColonPos + 1)
                                        .IndexOf(':');

                    result += line.Substring(0, firstColonPos) + ": ";

                    //Hash of the user's password.
                    string passHash = line.Substring(firstColonPos
                                                     + secondColonPos + 2);
                    string salt = line.Substring(firstColonPos + 1,
                                                 secondColonPos);

                    result += guessPassword(passHash, salt) + "\n";
                }
            }

            File.WriteAllText("../../crackedPasswords.txt", result);
            Console.WriteLine("Wrote the file");
            
            Console.Read();
        }

        private static string hash(string input) {
            //Converts input to a byte array.
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            //Converts bytes to a hex string and fixes formatting.
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static string guessPassword(string hash, string salt) {

            return hash + " " + salt;
        }
    }
}