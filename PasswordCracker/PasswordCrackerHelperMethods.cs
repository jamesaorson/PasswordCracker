using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker
{
    partial class PasswordCracker {
        private static Dictionary<string, string> hashIntoDict(string[] input, Dictionary<string, string> dict) {
            //Converts input to a byte array.
/*            byte[] inputBytes;
            byte[] hashBytes;*/

            for (int i = 0; i < input.Length; ++i) {
                dict[hash(input[i])] = input[i];
/*                inputBytes = Encoding.ASCII.GetBytes(input[i]);
                hashBytes = md5.ComputeHash(inputBytes);

                dict.Add(BitConverter.ToString(hashBytes).Replace("-", "").ToLower(), input[i]);*/
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

            return null;
        }

        public static string ToTitleCase(string str) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
