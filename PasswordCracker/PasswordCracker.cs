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
        private static Dictionary<char, char> letterReplaceDict
            = new Dictionary<char, char>();
        private static string[] originalDictWords;
        private static string[] lowerDictWords;
        private static string[] upperDictWords;
        private static string[] capitalizedDictWords;
        //Performs MD5 hashing.
        private static MD5 md5 = MD5.Create();

        static void Main(string[] args) {
            //string bibleFile = "../../bible.txt"; //vs
            //string hashesFile = "../../pa4hashes.txt"; //vs
            //string crackedPasswordsFile = "../../crackedPasswords.txt"; //vs
            string bibleFile = "bible.txt";       //Linux
            string hashesFile = "pa4hashes.txt";    //Linux
            string crackedPasswordsFile = "crackedPasswords.txt";  //Linux

            letterReplaceDict = initReplaceDict();

            //Splits bible.txt into a string[] of individual lowercase tokens.
            originalDictWords = readAndSplitFile(bibleFile);
            lowerDictWords = readAndSplitFile(bibleFile);
            upperDictWords = readAndSplitFile(bibleFile);

            //Returns the array with duplicates removed (For program
            //speed purposes).
            originalDictWords = originalDictWords.Distinct().ToArray();
            lowerDictWords = lowerDictWords.Distinct().ToArray();
            upperDictWords = upperDictWords.Distinct().ToArray();

            capitalizedDictWords = new string[lowerDictWords.Length];

            for (int i = 0; i < lowerDictWords.Length; ++i) {
                capitalizedDictWords[i] = ToTitleCase(lowerDictWords[i]);
            }

            capitalizedDictWords = capitalizedDictWords.Distinct().ToArray();

            //Hash and insert words from bible.txt
            lowerDict = hash(lowerDictWords, lowerDict);
            upperDict = hash(upperDictWords, upperDict);
            capitalizedDict = hash(capitalizedDictWords, capitalizedDict);

            Console.WriteLine("done with " + lowerDict.Count() + " words. ");

            var reader = new StringReader(
                File.ReadAllText(hashesFile));
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

            File.WriteAllText(crackedPasswordsFile, result);
            Console.WriteLine("Wrote the file");
            
            Console.Read();
        }
    }
}
