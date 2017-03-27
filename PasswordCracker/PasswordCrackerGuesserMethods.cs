using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    public partial class PasswordCracker {
        private static List<Password> guessPassword(List<Password> passwords) {
            string temp = String.Empty;

            foreach (var word in dictWords) {
                //Test normal dictionary search.
                foreach (var pass in passwords) {
                    temp = dictionarySearch(pass.HashString);

                    if (hash(temp).Equals(pass.HashString)) {
                        pass.Pass = temp;
                        pass.Time = DateTime.Now;
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt
                        temp = hash($"{word}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            pass.Pass = word;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and capitalizes
                        temp = hash($"{ToTitleCase(word)}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            pass.Pass = ToTitleCase(word);
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and special characters.
                        string modWord = specialCharacterReplace(word);

                        temp = hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and alternated case.
                        string modWord = AlternateCase(word);

                        temp = hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and alternated case.
                        string modWord = AlternateCase(word, false);

                        temp = hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and appended characters.
                        temp = checkAppended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and appended characters.
                        temp = checkAppended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (String.IsNullOrEmpty(pass.Pass)) {
                        //Test with salt and prepended characters.
                        temp = checkPrepended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }
                }

                passwords = checkAppended(word, passwords);
                passwords = checkAppended(word.ToUpper(), passwords);
                passwords = checkAppended(ToTitleCase(word), passwords);
                passwords = checkAppended(AlternateCase(word), passwords);
                passwords = checkAppended(AlternateCase(word, false), passwords);
                passwords = checkAppended(word, passwords, initCommonAppends());

                passwords = checkPrepended(word, passwords);
                passwords = checkPrepended(word.ToUpper(), passwords);
                passwords = checkPrepended(ToTitleCase(word), passwords);
                passwords = checkPrepended(AlternateCase(word), passwords);
                passwords = checkPrepended(AlternateCase(word, false), passwords);
                passwords = checkPrepended(word, passwords, initCommonAppends());
            }

            return passwords;
        }
         
        private static string specialCharacterReplace(string input) {
            StringBuilder result = new StringBuilder(input);
            bool[] contains = new bool[7];
            contains[0] = (input.Contains("a") || input.Contains("A"));
            contains[1] = (input.Contains("b") || input.Contains("B"));
            contains[2] = (input.Contains("e") || input.Contains("E"));
            contains[3] = (input.Contains("i") || input.Contains("I"));
            contains[4] = (input.Contains("o") || input.Contains("O"));
            contains[5] = (input.Contains("s") || input.Contains("S"));
            contains[6] = (input.Contains("z") || input.Contains("Z"));

            var indices = new List<int[]>();

            if (contains[0]) {
                indices.Add(AllIndicesOf(input.ToLower(), "a"));

                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['a'];
                }
            }
            if (contains[1]) {
                indices.Add(AllIndicesOf(input.ToLower(), "b"));

                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['b'];
                }
            }
            if (contains[2]) {
                indices.Add(AllIndicesOf(input.ToLower(), "e"));
                
                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['e'];
                }
            }
            if (contains[3]) {
                indices.Add(AllIndicesOf(input.ToLower(), "i"));
                
                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['i'];
                }
            }
            if (contains[4]) {
                indices.Add(AllIndicesOf(input.ToLower(), "o"));
                
                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['o'];
                }
            }
            if (contains[5]) {
                indices.Add(AllIndicesOf(input.ToLower(), "s"));

                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['s'];
                }
            }
            if (contains[6]) {
                indices.Add(AllIndicesOf(input.ToLower(), "z"));

                foreach (int index in indices[indices.Count - 1]) {
                    result[index] = letterReplaceDict['z'];
                }
            }

            return result.ToString();
        }

        private static List<Password> checkAppended(string word,
                                                    List<Password> passwords,
                                                    string[] appends) {
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (var ap in appends) {
                check.Append(ap);
                hashCheck = hash(check.ToString());

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

        private static string checkAppended(string input, string hashString,
                                            string salt) {
            StringBuilder result = new StringBuilder(input);
            result.Append(" ");
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };

            if (!String.IsNullOrEmpty(salt)) {
                foreach (char c in charSet) {
                    result[result.Length - 1] = c;
                
                    if (hash($"{result.ToString()}{salt}").Equals(hashString)) {
                        return $"{result.ToString()}";
                    }
                }
            }

            return String.Empty;
        }

        private static List<Password> checkAppended(string word,
                                                    List<Password> passwords) {
            StringBuilder check = new StringBuilder(word);
            check.Append(" ");
            string hashCheck;
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            
            foreach (char c in charSet) {
                check[check.Length - 1] = c;
                hashCheck = hash(check.ToString());    

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

        private static List<Password> checkPrepended(string word,
                                                    List<Password> passwords,
                                                    string[] prepends) {
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (var pr in prepends) {
                check.Insert(0, pr);
                hashCheck = hash(check.ToString());

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

        private static string checkPrepended(string input, string hashString,
                                            string salt) {
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            StringBuilder check = new StringBuilder(input);

            if (!String.IsNullOrEmpty(salt)) {
                foreach (char c in charSet) {
                    check.Insert(0, c);

                    if (hash($"{check.ToString()}{salt}").Equals(hashString)) {
                        return $"{check.ToString()}";
                    }
                }
            }

            return String.Empty;
        }

        private static List<Password> checkPrepended(string word,
                                                    List<Password> passwords) {
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '#', '?' };
            StringBuilder check = new StringBuilder(word);
            string hashCheck;

            foreach (char c in charSet) {
                check.Insert(0, c);
                hashCheck = hash(check.ToString());

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
