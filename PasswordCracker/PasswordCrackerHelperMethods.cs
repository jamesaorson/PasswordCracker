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
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        private static void InitReplaceDict() {

            replaceDict["a"] = "@";
            replaceDict["at"] = "@";
            replaceDict["and"] = "&";
            replaceDict["b"] = "8";
            replaceDict["e"] = "3";
            replaceDict["h"] = "#";
            replaceDict["i"] = "1";
            replaceDict["o"] = "0";
            replaceDict["s"] = "$";
            replaceDict["S"] = "5";
            replaceDict["z"] = "2";
        }

        private static string[] InitCommonAppends() {
            string[] result = { "0", "1", "2", "3", "4", "5", "6", "7", "8",
                                "9", "12", "123", "1234", "12345", "123456",
                                "1234567", "777", "1212", "123123",
                                "7777777", "666", "666666", "123321",
                                "01", "001", "0001", "00", "11", "!",
                                "?", "_1", "_01", "-1", "-01", "#", "&"};

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
                                     .Replace("'", "").Replace("\"", "");
            }

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
            var alt1Check = new StringBuilder(AlternateCase(word));
            var alt2Check = new StringBuilder(AlternateCase(word, false));
            string[] stringSet = InitCommonAppends();    

            foreach (string s in stringSet) {
                check.Append(s);
                titleCheck.Append(s);
                upperCheck.Append(s);
                alt1Check.Append(s);
                alt2Check.Append(s);
                
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
                if (Hash($"{alt1Check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = alt1Check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{alt2Check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = alt2Check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
                alt1Check = new StringBuilder(AlternateCase(word));
                alt2Check = new StringBuilder(AlternateCase(word, false));
            }

            return false;
        }

        private static bool CheckAppended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            var alt1Check = new StringBuilder(AlternateCase(word));
            var alt2Check = new StringBuilder(AlternateCase(word, false));
            string hashCheck;
            string titleHashCheck;
            string upperHashCheck;
            string alt1HashCheck;
            string alt2HashCheck;
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Append(s);
                titleCheck.Append(s);
                upperCheck.Append(s);
                alt1Check.Append(s);
                alt2Check.Append(s);

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());
                upperHashCheck = Hash(upperCheck.ToString());
                alt1HashCheck = Hash(alt1Check.ToString());
                alt2HashCheck = Hash(alt2Check.ToString());
                
                foreach (var pass in passwords) {
                    if (String.IsNullOrEmpty(pass.Salt)) {
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
                        if (alt1HashCheck.Equals(pass.HashString)) {
                            pass.Pass = alt1Check.ToString();
                            pass.Time = DateTime.Now;
                            
                            return true;
                        }
                        if (alt2HashCheck.Equals(pass.HashString)) {
                            pass.Pass = alt2Check.ToString();
                            pass.Time = DateTime.Now;

                            return true;
                        }
                    }
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
                alt1Check = new StringBuilder(AlternateCase(word));
                alt2Check = new StringBuilder(AlternateCase(word, false));
            }

            return false;
        }

        private static bool CheckPrepended(string word, Password pass) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            var alt1Check = new StringBuilder(AlternateCase(word));
            var alt2Check = new StringBuilder(AlternateCase(word, false));
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Insert(0, s);
                titleCheck.Insert(0, s);
                upperCheck.Insert(0, s);
                alt1Check.Insert(0, s);
                alt2Check.Insert(0, s);

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
                if (Hash($"{alt1Check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = alt1Check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }
                if (Hash($"{alt2Check.ToString()}{pass.Salt}")
                        .Equals(pass.HashString)) {
                    pass.Pass = alt2Check.ToString();
                    pass.Time = DateTime.Now;

                    return true;
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
                alt1Check = new StringBuilder(AlternateCase(word));
                alt2Check = new StringBuilder(AlternateCase(word, false));
            }

            return false;
        }

        private static bool CheckPrepended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            var upperCheck = new StringBuilder(word.ToUpper());
            var alt1Check = new StringBuilder(AlternateCase(word));
            var alt2Check = new StringBuilder(AlternateCase(word, false));
            string hashCheck;
            string titleHashCheck;
            string upperHashCheck;
            string alt1HashCheck;
            string alt2HashCheck;
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Insert(0, s);
                titleCheck.Insert(0, s);
                upperCheck.Insert(0, s);
                alt1Check.Insert(0, s);
                alt2Check.Insert(0, s);

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());
                upperHashCheck = Hash(upperCheck.ToString());
                alt1HashCheck = Hash(alt1Check.ToString());
                alt2HashCheck = Hash(alt2Check.ToString());

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
                    if (alt1HashCheck.Equals(pass.HashString)) {
                        pass.Pass = alt1HashCheck.ToString();
                        pass.Time = DateTime.Now;
                            
                        return true;
                    }
                    if (alt2HashCheck.Equals(pass.HashString)) {
                        pass.Pass = alt2HashCheck.ToString();
                        pass.Time = DateTime.Now;
                            
                        return true;
                    }
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
                upperCheck = new StringBuilder(word.ToUpper());
                alt1Check = new StringBuilder(AlternateCase(word));
                alt2Check = new StringBuilder(AlternateCase(word, false));
            }

            return false;
        }

        private static List<Password> CheckPrepended(string input,
                                                    List<Password> passwords,
                                                    string[] prepends) {
            StringBuilder check = new StringBuilder(input);
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

                check = new StringBuilder(input);
            }

            return passwords;
        }
    }
}
