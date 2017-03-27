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
        private static Dictionary<string, string> dict
            = new Dictionary<string, string>();
        private static Dictionary<char, char> letterReplaceDict
            = new Dictionary<char, char>();
        private static string[] dictWords;
        //Performs MD5 hashing.
        private static MD5 md5 = MD5.Create();
        private static DateTime start;

        static void Main(string[] args) {
            start = DateTime.Now;
            
            //string bibleFile = "../../bible.txt"; //vs
            //string hashesFile = "../../pa4hashes.txt"; //vs
            //string crackedPasswordsFile = "../../crackedPasswords.txt"; //vs
            
            string bibleFile = "bible.txt";       //Linux
            string hashesFile = "pa4hashes.txt";    //Linux
            string crackedPasswordsFile = "crackedPasswords.txt";  //Linux

            letterReplaceDict = initReplaceDict();

            //Splits bible.txt into a string[] of individual lowercase tokens.
            dictWords = readAndSplitFile(bibleFile);

            //Hash and insert words from bible.txt
            dict = hash(dictWords, dict);

            Console.WriteLine("done with " + dict.Count() + " words. ");

            string result = "";
            var passwords = parseHashesFile(hashesFile);

            passwords = guessPassword(passwords);

            foreach(var pass in passwords) {
                if (!String.IsNullOrEmpty(pass.Pass)) {
                    string temp = pass.Name;

                    if (temp.Length < 6) {
                        for (int i = temp.Length; i < 6; ++i) {
                            temp += " ";
                        }
                    }

                    result += $"{temp}:\t{pass.Pass}\t{pass.Time - start}\n";
                }
            }

            File.WriteAllText(crackedPasswordsFile, result);
            Console.WriteLine($"Wrote the file in {DateTime.Now - start}");
        }
    }
}
