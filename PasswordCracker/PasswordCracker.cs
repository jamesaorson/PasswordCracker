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
        private static Dictionary<string, string> replaceDict
            = new Dictionary<string, string>();
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

            replaceDict = InitReplaceDict();

            //Splits bible.txt into a string[] of individual lowercase tokens
            dictWords = ReadAndSplitFile(bibleFile);

            //Hash and insert words from bible.txt
            dict = Hash(dictWords, dict);

            Console.WriteLine("done with " + dict.Count() + " words. ");

            var passwords = ParseHashesFile(hashesFile);

            passwords = GuessPassword(passwords);

            WriteOutputFile(passwords, crackedPasswordsFile);
        }
    }
}
