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
        private static Dictionary<string, string> Hash(string[] input,
                Dictionary<string, string> dict) {
            for (int i = 0; i < input.Length; ++i) {
                //Inserts hashes into dictionary.
                dict[Hash(input[i])] = input[i];
            }

            //Converts bytes to a hex string and fixes formatting.
            return dict;
        }

        private static string Hash(string input) {
            byte[] inputBytes;
            byte[] hashBytes;

            inputBytes = Encoding.ASCII.GetBytes(input);
            hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static string dictionarySearch(string hashString) {
            if (dict.ContainsKey(hashString)) {
                return dict[hashString];
            }

            return String.Empty;
        }
    
        private static int[] AllIndicesOf(string input, string sub) {
            var indices = new List<int>();
            int index = -1;

            for (int i = 0; i < input.Length; ++i) {
                if (index < input.Length - 1) {
                    index = input.IndexOf(sub, index + 1);

                    if (index == -1) {
                        return indices.ToArray();
                    }

                    indices.Add(index);
                }
                else {
                    return indices.ToArray();
                }
            }

            return indices.ToArray();
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

        private static string[] initCommonAppends() {
            string[] result = { "12", "123", "1234", "12345", "123456",
                                "1234567", "777", "1212", "123123",
                                "7777777", "666", "666666", "123321" };

            return result;
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
            string[] result = File.ReadAllText(filename).ToLower().Split((string[])null,
                StringSplitOptions.RemoveEmptyEntries);

            result = result.Distinct().ToArray();

            for (int i = 0; i < result.Length; ++i) {
                result[i] = result[i].Replace(".", "").Replace("?", "")
                                        .Replace("!", "").Replace("(", "")
                                        .Replace(")", "").Replace("[", "")
                                        .Replace("]", "").Replace(",", "")
                                        .Replace(";", "").Replace(":", "");
            }

            return result.Distinct().ToArray();
        }

        private static string AlternateCase(string input, bool upperFirst = true) {
            string result = "";

            if (upperFirst) {
                for (int i = 0; i < input.Length; ++i) {
                    if (i % 2 == 0) {
                        result += input[i].ToString().ToUpper();
                    }
                    else {
                        result += input[i].ToString().ToLower();
                    }
                }
            }
            else {
                for (int i = 0; i < input.Length; ++i) {
                    if (i % 2 == 0) {
                        result += input[i].ToString().ToLower();
                    }
                    else {
                        result += input[i].ToString().ToUpper();
                    }
                }
            }

            return result;
        }
    }
}
