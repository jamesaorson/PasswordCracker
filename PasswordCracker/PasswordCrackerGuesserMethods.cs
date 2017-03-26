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

        /* In the current state, we can get another word by replacing S, which gives us 7 words.
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
