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
           /* byte[] inputBytes;
            byte[] hashBytes;

            inputBytes = Encoding.ASCII.GetBytes(input);
            hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();*/

            var md5 = new MD5.MD5();

            md5.Value = input;

            return md5.FingerPrint.ToLower();
        }

        private static string DictionarySearch(string hashString) {
            if (dict.ContainsKey(hashString)) {
                return dict[hashString];
            }

            return string.Empty;
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
            StringBuilder result = new StringBuilder(s);

            result[0] = result[0].ToString().ToUpper()[0];

            return result.ToString();
        }

        private static void InitReplaceDict() {

            replaceDict["a"] = "@";
            replaceDict["at"] = "@";
            replaceDict["and"] = "&";
            replaceDict["b"] = "8";
            replaceDict["e"] = "3";
            replaceDict["h"] = "#";
            replaceDict["i"] = "1";
            replaceDict["I"] = "!";
            replaceDict["o"] = "0";
            replaceDict["s"] = "$";
            replaceDict["S"] = "5";
            replaceDict["z"] = "2";
        }

        private static string[] InitCommonAppends() {
            string[] result = { "0", "1", "2", "3", "4", "5", "6", "7", "8",
                                "9", "10", "11", "12", "123", "1234", "12345", "123456",
                                "1234567", "12345678", "123456789", "01", "012", "0123",
                                "01234", "012345", "0123456", "01234567", "012345678",
                                "0123456789", "9876543210", "876543210", "76543210",
                                "6543210", "543210", "43210", "3210", "210", "987654321",
                                "87654321", "7654321", "654321", "54321", "4321", "321",
                                "21", "!", "00", "21" };

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
            string[] result = File.ReadAllText(filename).ToLower()
                              .Split((string[])null,
                               StringSplitOptions.RemoveEmptyEntries);
            result = result.Distinct().ToArray();

            for (int i = 0; i < result.Length; ++i) {
                result[i] = result[i].Replace(".", "").Replace("?", "")
                                     .Replace("!", "").Replace("(", "")
                                     .Replace(")", "").Replace("[", "")
                                     .Replace("]", "").Replace(",", "")
                                     .Replace(";", "").Replace(":", "")
                                     .Replace("'", "").Replace("\"", "")
                                     .Replace("-", "");
            }

            result = result.Distinct().ToArray();

            string text = "";

            foreach (string word in result) {
                text += word + "\n";
            }

            File.WriteAllText("./dict.txt", text);

            return result.Distinct().ToArray();
        }

        private static void WriteOutputFile(List<Password> passwords,
                                               string file) {
            string result = string.Empty;

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

        private static string AlternateCase(string input,
                                            bool upperFirst = true) {
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
            var result = new StringBuilder(input);

            if (input.Contains(sub)) {
                result.Replace(sub, replacement);
            }

            return result.ToString();
        }

        private static string ReplaceSubstring(string input, string[] subs,
                                               string[] replacements) {
            var result = new StringBuilder(input);

            if (subs.Length > replacements.Length) {
                Console.WriteLine("Provide more string replacements");

                return string.Empty;
            }
            
            for (int i = 0; i < subs.Length; ++i) {
                if (result.ToString().Contains(subs[i])) {
                    result.Replace(subs[i], replacements[i]);
                }
            }

            return result.ToString();
        }

        private static bool CheckReplace(string word, Password pass, string sub, string replace) {
            string modWord = word;

            if (!String.IsNullOrEmpty(sub)) {
                modWord = ReplaceSubstring(modWord, sub, replace);
            }

            string temp = Hash($"{modWord}{pass.Salt}");

            if (temp.Equals(pass.HashString)) {
                pass.Pass = modWord;
                pass.Time = DateTime.Now;

                return true;
            }

            modWord = ToTitleCase(modWord);

            temp = Hash($"{modWord}{pass.Salt}");

            if (temp.Equals(pass.HashString)) {
                pass.Pass = modWord;
                pass.Time = DateTime.Now;

                return true;
            }

            modWord = modWord.ToUpper();

            temp = Hash($"{modWord}{pass.Salt}");

            if (temp.Equals(pass.HashString)) {
                pass.Pass = modWord;
                pass.Time = DateTime.Now;

                return true;
            }

            return false;
        }

        private static bool CheckReplace(string word, Password pass,
                                         string[] subs, string[] reps) {
            return CheckReplace(ReplaceSubstring(word, subs, reps), pass,
                                                 string.Empty, string.Empty);
        }

        private static bool CheckAlternateCase(string input, Password pass,
                                               bool upperFirst = true) {
            string temp = Hash($"{AlternateCase(input, upperFirst)}{pass.Salt}");

            if (temp.Equals(pass.HashString)) {
                pass.Pass = temp;
                pass.Time = DateTime.Now;

                return true;
            }

            return false;
        }

        private static bool CheckAppended(string word, Password pass) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            string[] stringSet = InitCommonAppends();    

            foreach (string s in stringSet) {
            /*for (int i = 0; i < 1000; ++i) {
                check.Append(i.ToString());
                titleCheck.Append(i.ToString());
                upperCheck.Append(i.ToString());*/
                check.Append(s);
                titleCheck.Append(s);
                upperCheck.Append(s);

                if (Hash($"{check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{titleCheck.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = titleCheck.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{upperCheck.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = upperCheck.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
            }

            return false;
        }

        private static bool CheckAppended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            string hashCheck;
            string titleHashCheck;
            string upperHashCheck;
            string[] stringSet = InitCommonAppends();

            for (int i = 0; i <= 10000; ++i) {
                check.Append(i.ToString());
                titleCheck.Append(i.ToString());
                upperCheck.Append(i.ToString());

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());
                upperHashCheck = Hash(upperCheck.ToString());

                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
                        if (hashCheck.Equals(pass.HashString)) {
                            Console.WriteLine(check); pass.Pass = check.ToString();
                            pass.Time = DateTime.Now;
                            
                            return true;
                        }
                        if (titleHashCheck.Equals(pass.HashString)) {
                            Console.WriteLine(check); pass.Pass = titleCheck.ToString();
                            pass.Time = DateTime.Now;

                            return true;
                        }
                        if (upperHashCheck.Equals(pass.HashString)) {
                            Console.WriteLine(check); pass.Pass = upperCheck.ToString();
                            pass.Time = DateTime.Now;

                            return true;
                        }
                    }
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
            }

            return false;
        }

        private static bool CheckPrepended(string word, Password pass) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Insert(0, s);
                titleCheck.Insert(0, s);
                upperCheck.Insert(0, s);

                if (Hash($"{check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{titleCheck.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = titleCheck.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{upperCheck.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = upperCheck.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
            }

            return false;
        }

        private static bool CheckPrepended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            string hashCheck;
            string titleHashCheck;
            string upperHashCheck;
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Insert(0, s);
                titleCheck.Insert(0, s);
                upperCheck.Insert(0, s);

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());
                upperHashCheck = Hash(upperCheck.ToString());

                foreach (var pass in passwords) {
                    if (hashCheck.Equals(pass.HashString)) {
                        pass.Pass = check.ToString();
                        pass.Time = DateTime.Now;
                            
                        return true;
                    }
                    if (titleHashCheck.Equals(pass.HashString)) {
                        pass.Pass = titleCheck.ToString();
                        pass.Time = DateTime.Now;
                            
                        return true;
                    }
                    if (upperHashCheck.Equals(pass.HashString)) {
                        pass.Pass = upperCheck.ToString();
                        pass.Time = DateTime.Now;
                            
                        return true;
                    }
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
            }

            return false;
        }

        private static bool CheckAppendAndPrepend(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            string hashCheck;
            string titleHashCheck;
            string upperHashCheck;
            string[] stringSet = InitCommonAppends();

            for (int i = 0; i <= 10000; ++i) {
                for (int j = 0; j <= 10000; ++j) {
                    check.Insert(0, i.ToString());
                    check.Append(j.ToString());

                    titleCheck.Insert(0, i.ToString());
                    titleCheck.Append(j.ToString());

                    upperCheck.Insert(0, i.ToString());
                    upperCheck.Append(j.ToString());

                    hashCheck = Hash(check.ToString());
                    titleHashCheck = Hash(titleCheck.ToString());
                    upperHashCheck = Hash(upperCheck.ToString());

                    foreach (var pass in passwords) {
                        if (String.IsNullOrEmpty(pass.Salt)) {
                            if (hashCheck.Equals(pass.HashString)) {
                                Console.WriteLine(check); pass.Pass = check.ToString();
                                pass.Time = DateTime.Now;

                                return true;
                            }
                            if (titleHashCheck.Equals(pass.HashString)) {
                                Console.WriteLine(check); pass.Pass = titleCheck.ToString();
                                pass.Time = DateTime.Now;

                                return true;
                            }
                            if (upperHashCheck.Equals(pass.HashString)) {
                                Console.WriteLine(check); pass.Pass = upperCheck.ToString();
                                pass.Time = DateTime.Now;

                                return true;
                            }
                        }
                    }

                    check = new StringBuilder(word);
                    titleCheck = new StringBuilder(ToTitleCase(word));
                    upperCheck = new StringBuilder(word.ToUpper());
                }
            }

            return false;
        }
    }
}