using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private static string DictionarySearch(string hashString) {
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

        private static Dictionary<string, string> InitReplaceDict() {
            var dict = new Dictionary<string, string>();

            dict["a"] = "@";
            dict["at"] = "@";
            dict["b"] = "8";
            dict["e"] = "3";
            dict["h"] = "#";
            dict["i"] = "1";
            dict["o"] = "0";
            dict["s"] = "$";
            dict["S"] = "5";
            dict["z"] = "2";

            return dict;
        }

        private static string[] InitCommonAppends() {
            string[] result = { "12", "123", "1234", "12345", "123456",
                                "1234567", "777", "1212", "123123",
                                "7777777", "666", "666666", "123321" };

            return result;
        }

        private static List<Password> ParseHashesFile(string filename) {
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

        private static string[] ReadAndSplitFile(string filename) {
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

        private static void WriteOutputFile(List<Password> passwords,
                                               string file) {
            string result = String.Empty;

            foreach(var pass in passwords) {
                if (!String.IsNullOrEmpty(pass.Pass)) {
                    string tempName = pass.Name;

                    if (tempName.Length < 6) {
                        for (int i = tempName.Length; i < 6; ++i) {
                            tempName += " ";
                        }
                    }

                    string tempPass = pass.Pass;

                    if (tempPass.Length < 20) {
                        for (int i = tempPass.Length; i < 20; ++i) {
                            tempPass += " ";
                        }
                    }

                    result += $"{tempName}:\t{tempPass}\t{pass.Time - start}\n";
                }
            }

            File.WriteAllText(file, result);
            Console.WriteLine($"Wrote the file in {DateTime.Now - start}");
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

        private static string ReplaceSubstring(string input, string sub,
                                               string replacement) {
            StringBuilder result = new StringBuilder(input);

            if (input.Contains(sub)) {
                result.Replace(sub, replacement);
            }

            return result.ToString();
        }

        private static string ReplaceSubstring(string input, string[] subs,
                                               string[] replacements) {
            StringBuilder result = new StringBuilder(input);

            if (subs.Length > replacements.Length) {
                Console.WriteLine("Provide more string replacements");

                return String.Empty;
            }
            
            for (int i = 0; i < subs.Length; ++i) {
                if (result.ToString().Contains(subs[i])) {
                    result.Replace(subs[i], replacements[i]);
                }
            }

            return result.ToString();
        }

        private static List<Password> CheckAppended(string word,
                                                    List<Password> passwords,
                                                    string[] appends) {
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (var ap in appends) {
                check.Append(ap);
                hashCheck = Hash(check.ToString());

                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
                        if (hashCheck.Equals(pass.HashString)) {
                            pass.Pass = check.ToString();
                            pass.Time = DateTime.Now;
                            break;
                        }
                    }
                }

                check = new StringBuilder(word);
            }

            return passwords;
        }

        private static string CheckAppended(string input, string hashString,
                                            string salt) {
            StringBuilder result = new StringBuilder(input);
            result.Append(" ");
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '!', '#', '?', '*' };

            if (!String.IsNullOrEmpty(salt)) {
                foreach (char c in charSet) {
                    result[result.Length - 1] = c;
                
                    if (Hash($"{result.ToString()}{salt}").Equals(hashString)) {
                        return $"{result.ToString()}";
                    }
                }
            }

            return String.Empty;
        }

        private static List<Password> CheckAppended(string word,
                                                    List<Password> passwords) {
            StringBuilder check = new StringBuilder(word);
            check.Append(" ");
            string hashCheck;
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            
            foreach (char c in charSet) {
                check[check.Length - 1] = c;
                hashCheck = Hash(check.ToString());    

                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
                        if (hashCheck.Equals(pass.HashString)) {
                            pass.Pass = check.ToString();
                            pass.Time = DateTime.Now;
                            break;
                        }
                    }
                }
            }

            return passwords;
        }

        private static List<Password> CheckPrepended(string word,
                                                    List<Password> passwords,
                                                    string[] prepends) {
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (var pr in prepends) {
                check.Insert(0, pr);
                hashCheck = Hash(check.ToString());

                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
                        if (hashCheck.Equals(pass.HashString)) {
                            pass.Pass = check.ToString();
                            pass.Time = DateTime.Now;
                            break;
                        }
                    }
                }

                check = new StringBuilder(word);
            }

            return passwords;
        }

        private static string CheckPrepended(string input, string hashString,
                                            string salt) {
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            StringBuilder check = new StringBuilder(input);

            if (!String.IsNullOrEmpty(salt)) {
                foreach (char c in charSet) {
                    check.Insert(0, c);

                    if (Hash($"{check.ToString()}{salt}").Equals(hashString)) {
                        return $"{check.ToString()}";
                    }
                }
            }

            return String.Empty;
        }

        private static List<Password> CheckPrepended(string word,
                                                    List<Password> passwords) {
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (char c in charSet) {
                check.Insert(0, c);
                hashCheck = Hash(check.ToString());

                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
                        if (hashCheck.Equals(pass.HashString)) {
                            pass.Pass = check.ToString();
                            pass.Time = DateTime.Now;
                            break;
                        }
                    }
                }
            }

            return passwords;
        }
    }
}
