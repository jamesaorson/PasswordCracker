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
        private static Dictionary<string, string> hash(string[] input,
                Dictionary<string, string> dict) {
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

        private static string guessPassword(string hashCode, string salt) {
            string temp;

            if (String.IsNullOrEmpty(salt)) {
                temp = dictionarySearch(hashCode);

                if (!String.IsNullOrEmpty(temp)) {
                    return $"{temp}\n";
                }
            }

            foreach(string word in originalDictWords) {
                //Complicated stuff
                temp = hash(specialCharacterReplace(word));

                if (temp.Equals(hashCode)) {
                    return word;
                }
            }
            foreach(string word in lowerDictWords) {
                //Complicated stuff
                temp = hash(specialCharacterReplace(word));

                if (temp.Equals(hashCode)) {
                    return word;
                }
            }
            foreach(string word in upperDictWords) {
                //Complicated stuff
                temp = hash(specialCharacterReplace(word));

                if (temp.Equals(hashCode)) {
                    return word;
                }
            }

            return $"{hashCode} {salt}";
        }

        private static string dictionarySearch(string hashCode) {
            if (lowerDict.ContainsKey(hashCode)) {
                return lowerDict[hashCode];
            }
            if (upperDict.ContainsKey(hashCode)) {
                return upperDict[hashCode];
            }
            if (capitalizedDict.ContainsKey(hashCode)) {
                return capitalizedDict[hashCode];
            }

            return String.Empty;
        }

        private static string specialCharacterReplace(string input) {
            StringBuilder result = new StringBuilder(input);
            bool containsA = input.Contains("a") || input.Contains("A");
            bool containsE = input.Contains("e") || input.Contains("E");
            bool containsI = input.Contains("i") || input.Contains("I");
            bool containsL = input.Contains("l") || input.Contains("L");
            bool containsO = input.Contains("o") || input.Contains("O");
            bool containsS = input.Contains("s") || input.Contains("S");
            var aIndices = new List<int>();
            var eIndices = new List<int>();
            var iIndices = new List<int>();
            var lIndices = new List<int>();
            var oIndices = new List<int>();
            var sIndices = new List<int>();

            if (containsA) {
                aIndices = AllIndicesOf(input, "a");
                aIndices = AllIndicesOf(input, "A");

                foreach (int index in aIndices) {
                    result[index] = letterReplaceDict['a'];
                }
            }
            if (containsE) {
                eIndices = AllIndicesOf(input, "e");
                eIndices = AllIndicesOf(input, "E");
                
                foreach (int index in eIndices) {
                    result[index] = letterReplaceDict['e'];
                }
            }
            if (containsI) {
                iIndices = AllIndicesOf(input, "i");
                iIndices = AllIndicesOf(input, "I");
                
                foreach (int index in iIndices) {
                    result[index] = letterReplaceDict['i'];
                }
            }
            if (containsL) {
                lIndices = AllIndicesOf(input, "l");
                lIndices = AllIndicesOf(input, "L");
                
                foreach (int index in lIndices) {
                    result[index] = letterReplaceDict['l'];
                }
            }
            if (containsO) {
                oIndices = AllIndicesOf(input, "o");
                oIndices = AllIndicesOf(input, "O");
                
                foreach (int index in oIndices) {
                    result[index] = letterReplaceDict['o'];
                }
            }
            if (containsS) {
                sIndices = AllIndicesOf(input, "s");
                sIndices = AllIndicesOf(input, "S");
                
                foreach (int index in sIndices) {
                    result[index] = letterReplaceDict['s'];
                }
            }

            return result.ToString();
        }
    
        private static List<int> AllIndicesOf(string input, string sub) {
            var indices = new List<int>();
            int index = 0;

            for (int i = 0; i < input.Length; ++i) {
                index = input.IndexOf(sub, index);

                if (index == -1) {
                    return indices;
                }

                indices.Add(index);
            }

            return indices;
        }

        private static string ToTitleCase(string s) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        private static Dictionary<char, char> initReplaceDict() {
            var dict = new Dictionary<char, char>();

            dict['a'] = '@';
            dict['e'] = '3';
            dict['i'] = '1';
            dict['l'] = '1';
            dict['o'] = '0';
            dict['s'] = '$';

            return dict;
        }

        private static string[] readAndSplitFile(string filename) {
            return File.ReadAllText(filename).Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
