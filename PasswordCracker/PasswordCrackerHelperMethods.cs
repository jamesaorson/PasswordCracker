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
            string[] result = { "1", "!", "1234" };

            return result;
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
            string[] stringSet = InitCommonAppends();    

            foreach (string s in stringSet) {
                check.Append(s);
                titleCheck.Append(s);

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

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
            }

            return false;
        }

        private static bool CheckAppended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            string hashCheck;
            string titleHashCheck;

            for (int i = 0; i <= 100; ++i) {
                check.Append(i);
                titleCheck.Append(i);

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());

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
                    }
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
            }

            return false;
        }

        private static bool CheckPrepended(string word, Password pass) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            string[] stringSet = InitCommonAppends();

            foreach (string s in stringSet) {
                check.Insert(0, s);
                titleCheck.Insert(0, s);

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

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
            }

            return false;
        }

        private static bool CheckPrepended(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            string hashCheck;
            string titleHashCheck;

            for (int i = 1900; i <= 2000; ++i) {
                check.Insert(0, i);
                titleCheck.Insert(0, i);

                hashCheck = Hash(check.ToString());
                titleHashCheck = Hash(titleCheck.ToString());

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
                }

                check = new StringBuilder(word);
                titleCheck = new StringBuilder(ToTitleCase(word));
            }

            return false;
        }

        private static bool CheckAppendAndPrepend(string word,
                                                    List<Password> passwords) {
            var check = new StringBuilder(word);
            var titleCheck = new StringBuilder(ToTitleCase(word));
            string hashCheck;
            string titleHashCheck;

            for (int i = 32; i <= 124; ++i) {
                for (int j = 32; j <= 124; ++j) {
                    check.Insert(0, (char)i);
                    check.Append((char)j);

                    titleCheck.Insert(0, (char)i);
                    titleCheck.Append((char)j);

                    hashCheck = Hash(check.ToString());
                    titleHashCheck = Hash(titleCheck.ToString());

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
                        }
                    }

                    check = new StringBuilder(word);
                    titleCheck = new StringBuilder(ToTitleCase(word));
                }              
            }

            return false;
        }
    }
}