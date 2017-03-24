using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    partial class PasswordCracker {
        //Holds dictionary of words in format of <hash, word>.
        private static Dictionary<string, string> lowerDict
            = new Dictionary<string, string>();
        private static Dictionary<string, string> upperDict
            = new Dictionary<string, string>();
        private static Dictionary<string, string> capitalizedDict
            = new Dictionary<string, string>();
        //Performs MD5 hashing.
        private static MD5 md5 = MD5.Create();

        static void Main(string[] args) {
            //Splits bible.txt into a string[] of individual lowercase tokens.
            string[] lowerDictWords = File
                .ReadAllText(/*"../../bible.txt"*/"bible.txt")
                .ToLower().Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);
            string[] upperDictWords = File
                .ReadAllText(/*"../../bible.txt"*/"bible.txt")
                .ToUpper().Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);
            string[] capitalizedDictWords;

            //Returns the array with duplicates removed (For program
            //speed purposes).
            lowerDictWords = lowerDictWords.Distinct().ToArray();
            capitalizedDictWords = new string[lowerDictWords.Length];
            upperDictWords = upperDictWords.Distinct().ToArray();

            for (int i = 0; i < lowerDictWords.Length; ++i) {
                capitalizedDictWords[i] = ToTitleCase(lowerDictWords[i]);
            }

            capitalizedDictWords = capitalizedDictWords.Distinct().ToArray();

            //Hash and insert words from bible.txt
            lowerDict = hashIntoDict(lowerDictWords, lowerDict);
            upperDict = hashIntoDict(upperDictWords, upperDict);
            capitalizedDict = hashIntoDict(capitalizedDictWords, capitalizedDict);

            Console.WriteLine("done with " + lowerDict.Count() + " words. ");

            StringReader reader = new StringReader(
                File.ReadAllText(/*"../../pa4hashes.txt"*/"pa4hashes.txt"));
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

                    result += $"{guessPassword(passHash, salt)}\n";
                }
            }

            File.WriteAllText(/*"../../crackedPasswords.txt*/"crackedPasswords.txt", result);
            Console.WriteLine("Wrote the file");
            
            Console.Read();
        }
    }
}
