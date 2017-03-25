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

        private static string guessPassword(string hashString, string salt) {
            string temp;

            //Test normal dictionary search.
            if (String.IsNullOrEmpty(salt)) {
                temp = dictionarySearch(hashString);

                if (!String.IsNullOrEmpty(temp)) {
                    return $"{temp}\n";
                }
            }

            foreach(var word in originalDictWords) {
                //Test with salt
                temp = hash($"{word}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }

                //Test special characters.
                temp = hash($"{specialCharacterReplace(word)}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }
            }
            foreach(var word in lowerDictWords) {
                //Test with salt
                temp = hash($"{word}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }

                //Test special characters.
                temp = hash($"{specialCharacterReplace(word)}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }
            }
            foreach(var word in upperDictWords) {
                //Test with salt
                temp = hash($"{word}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }

                //Test special characters.
                temp = hash($"{specialCharacterReplace(word)}{salt}");

                if (temp.Equals(hashString)) {
                    return word;
                }
            }

            return $"{hashString} {salt}";
        }

        private static string dictionarySearch(string hashString) {
            if (lowerDict.ContainsKey(hashString)) {
                return lowerDict[hashString];
            }
            if (upperDict.ContainsKey(hashString)) {
                return upperDict[hashString];
            }
            if (capitalizedDict.ContainsKey(hashString)) {
                return capitalizedDict[hashString];
            }

            return String.Empty;
        }

        /*In the current state, we can get another word by replacing S, which gives us 7 words.
         * We must figure out a way to permute the combinations of what letters the string
         * contains, for not all string will replace every possible type of special letter.
         */
        private static string specialCharacterReplace(string input) {
            StringBuilder result = new StringBuilder(input);
            bool containsA = input.Contains("a") || input.Contains("A");
            bool containsB = input.Contains("b") || input.Contains("B");
            bool containsE = input.Contains("e") || input.Contains("E");
            bool containsI = input.Contains("i") || input.Contains("I");
            bool containsO = input.Contains("o") || input.Contains("O");
            bool containsS = input.Contains("s") || input.Contains("S");
            bool containsZ = input.Contains("z") || input.Contains("Z");
            var aIndices = new List<int>();
            var bIndices = new List<int>();
            var eIndices = new List<int>();
            var iIndices = new List<int>();
            var oIndices = new List<int>();
            var sIndices = new List<int>();
            var zIndices = new List<int>();

            /*if (containsA) {
                aIndices = AllIndicesOf(input.ToLower(), "a");

                foreach (int index in aIndices) {
                    result[index] = letterReplaceDict['a'];
                }
            }*/
            /*if (containsB) {
                aIndices = AllIndicesOf(input.ToLower(), "b");

                foreach (int index in bIndices) {
                    result[index] = letterReplaceDict['b'];
                }
            }*/
            /*if (containsE) {
                eIndices = AllIndicesOf(input.ToLower(), "e");
                
                foreach (int index in eIndices) {
                    result[index] = letterReplaceDict['e'];
                }
            }*/
            /*if (containsI) {
                iIndices = AllIndicesOf(input.ToLower(), "i");
                
                foreach (int index in iIndices) {
                    result[index] = letterReplaceDict['i'];
                }
            }*/
            /*if (containsO) {
                oIndices = AllIndicesOf(input.ToLower(), "o");
                
                foreach (int index in oIndices) {
                    result[index] = letterReplaceDict['o'];
                }
            }*/
            if (containsS) {
                sIndices = AllIndicesOf(input.ToLower(), "s");
                
                foreach (int index in sIndices) {
                    result[index] = letterReplaceDict['s'];
                }
            }
            /*if (containsZ) {
                aIndices = AllIndicesOf(input.ToLower(), "z");

                foreach (int index in zIndices) {
                    result[index] = letterReplaceDict['z'];
                }
            }*/

            return result.ToString();
        }
    
        private static List<int> AllIndicesOf(string input, string sub) {
            var indices = new List<int>();
            int index = -1;
            int lastIndex = input.LastIndexOf(sub);

            for (int i = 0; i < input.Length; ++i) {
                if (index < input.Length - 1) {
                    index = input.IndexOf(sub, index + 1);

                    if (index == -1) {
                        return indices;
                    }

                    indices.Add(index);
                }
                else {
                    return indices;
                }
            }

            return indices;
        }

        private static string ToTitleCase(string s) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        private static Dictionary<char, char> initReplaceDict() {
            var dict = new Dictionary<char, char>();

            dict['a'] = '@';
            dict['b'] = '8';
            dict['e'] = '3';
            dict['i'] = '1';
            dict['o'] = '0';
            dict['s'] = '$';
            dict['z'] = '2';

            return dict;
        }

        private static List<Password> parseHashesFile(string filename) {
            var reader = new StringReader(
                File.ReadAllText(filename));
            var passwords = new List<Password>();

            while (reader.Peek() != -1) {
                string line = reader.ReadLine();

                if (line.Contains(":")) {
                    int firstColonPos = line.IndexOf(':');
                    int secondColonPos = line.Substring(firstColonPos + 1)
                                        .IndexOf(':');

                    passwords.Add(new Password(line.Substring(0, firstColonPos),
                        line.Substring(firstColonPos + secondColonPos + 2),
                        line.Substring(firstColonPos + 1, secondColonPos)));
                }
            }

            return passwords;
        }

        private static string[] readAndSplitFile(string filename) {
            return File.ReadAllText(filename).Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
