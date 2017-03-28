using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordCracker {
    public partial class PasswordCracker {
        private static List<Password> GuessPassword(List<Password> passwords) {
            string temp = String.Empty;

            foreach (var word in dictWords) {
                //Test normal dictionary search.
                foreach (var pass in passwords) {
                    bool found = false;
                    temp = DictionarySearch(pass.HashString);

                    if (Hash(temp).Equals(pass.HashString)) {
                        found = true;
                        pass.Pass = temp;
                        pass.Time = DateTime.Now;
                    }

                    if (!found) {
                        //Test with salt
                        temp = Hash($"{word}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = word;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and capitalizes
                        temp = Hash($"{ToTitleCase(word)}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = ToTitleCase(word);
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "a",
                                             replaceDict["a"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "at",
                                             replaceDict["at"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "e",
                                             replaceDict["e"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "h",
                                             replaceDict["h"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "i",
                                             replaceDict["i"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "o",
                                             replaceDict["o"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "s",
                                             replaceDict["s"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word, "s",
                                             replaceDict["S"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "a", replaceDict["a"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "at", replaceDict["at"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "e", replaceDict["e"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "h", replaceDict["h"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "i", replaceDict["i"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "o", replaceDict["o"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "s", replaceDict["s"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(ToTitleCase(word),
                                             "s", replaceDict["S"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "A",
                                             replaceDict["a"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "AT",
                                             replaceDict["at"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "E",
                                             replaceDict["e"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "H",
                                             replaceDict["h"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "I",
                                             replaceDict["i"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "O",
                                             replaceDict["o"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "S",
                                             replaceDict["s"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and special characters.
                        string modWord = ReplaceSubstring(word.ToUpper(), "S",
                                             replaceDict["S"]);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and alternated case.
                        string modWord = AlternateCase(word);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and alternated case.
                        string modWord = AlternateCase(word, false);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and appended characters.
                        temp = CheckAppended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            found = true;
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and appended characters.
                        temp = CheckAppended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            found = true;
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }

                    if (!found) {
                        //Test with salt and prepended characters.
                        temp = CheckPrepended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            found = true;
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }
                }

                passwords = CheckAppended(word, passwords);
                passwords = CheckAppended(word.ToUpper(), passwords);
                passwords = CheckAppended(ToTitleCase(word), passwords);
                passwords = CheckAppended(AlternateCase(word), passwords);
                passwords = CheckAppended(AlternateCase(word, false), passwords);
                passwords = CheckAppended(word, passwords, InitCommonAppends());

                passwords = CheckPrepended(word, passwords);
                passwords = CheckPrepended(word.ToUpper(), passwords);
                passwords = CheckPrepended(ToTitleCase(word), passwords);
                passwords = CheckPrepended(AlternateCase(word), passwords);
                passwords = CheckPrepended(AlternateCase(word, false), passwords);
                passwords = CheckPrepended(word, passwords, InitCommonAppends());
            }

            return passwords.OrderBy(pass => pass.Time).ToList();
        }
    }
}
