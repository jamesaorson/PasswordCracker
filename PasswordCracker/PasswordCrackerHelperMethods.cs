using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker
{
    partial class PasswordCracker {
        private static Dictionary<string, string> hashIntoDict(string[] input, Dictionary<string, string> dict) {
            for (int i = 0; i < input.Length; ++i) {
                //Inserts hashes into dictionary.
                dict[hash(input[i])] = input[i];
            }

            //Converts bytes to a hex string and fixes formatting.
            return dict;
        }

        private static string hash(string input) {
            byte[] inputBytes;
            byte[] hashBytes;

            inputBytes = Encoding.ASCII.GetBytes(input);
            hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static string guessPassword(string hash, string salt) {
            if (String.IsNullOrEmpty(salt)) {
                string temp = dictionarySearch(hash);

                if (!String.IsNullOrEmpty(temp)) {
                    return $"{temp}\n";
                }
            }

            //Complicated stuff

            return hash + " " + salt;
        }

        private static string dictionarySearch(string hash) {
            if (lowerDict.ContainsKey(hash)) {
                return lowerDict[hash];
            }
            if (upperDict.ContainsKey(hash)) {
                return upperDict[hash];
            }
            if (capitalizedDict.ContainsKey(hash)) {
                return capitalizedDict[hash];
            }

            return String.Empty;
        }

        private static string specialCharacterReplace(string input) {
            

            return String.Empty;
        }

        public static string ToTitleCase(string s) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static Dictionary<string, string> initReplaceDict() {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["a"] = "@";
            dict["A"] = "@";
            dict["e"] = "3";
            dict["E"] = "3";
            dict["i"] = "1";
            dict["I"] = "1";
            dict["l"] = "1";
            dict["L"] = "1";
            dict["o"] = "0";
            dict["O"] = "0";
            dict["s"] = "$";
            dict["S"] = "$";

            return dict;
        }

        public static string[] readAndSplitFile(string filename) {
            return File.ReadAllText(filename).Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
