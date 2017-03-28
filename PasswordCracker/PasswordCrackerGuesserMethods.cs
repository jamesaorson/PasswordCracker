using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordCracker {
    public partial class PasswordCracker {
        private static List<Password> GuessPassword(List<Password> passwords) {
            string temp = string.Empty;

            foreach (var word in dictWords) {
                foreach (var pass in passwords) {
                    bool found = false;

                    //Test normal dictionary search
                    temp = DictionarySearch(pass.HashString);

                    if (Hash(temp).Equals(pass.HashString)) {
                        found = true;
                        pass.Pass = temp;
                        pass.Time = DateTime.Now;
                    }

                    //Test with salt
                    if (!found) {
                        temp = Hash($"{word}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = word;
                            pass.Time = DateTime.Now;
                        }
                    }

                    //Test with salt and capitalized
                    if (!found) {
                        temp = Hash($"{ToTitleCase(word)}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = ToTitleCase(word);
                            pass.Time = DateTime.Now;
                        }
                    }

                    //Test with salt and special characters
                    if (!found) {
                        found = CheckReplace(word, pass, "a", replaceDict["a"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "at", replaceDict["at"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "e", replaceDict["e"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "h", replaceDict["h"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "i", replaceDict["i"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "o", replaceDict["o"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "s", replaceDict["s"]);
                    }

                    if (!found) {
                        found = CheckReplace(word, pass, "s", replaceDict["S"]);
                    }

                    if (!found)
                    {
                        string[] arr =    { "a", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "1", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    //Test with salt and alternated case
                    if (!found) {
                        string modWord = AlternateCase(word);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    //Test with salt and alternated case
                    if (!found) {
                        string modWord = AlternateCase(word, false);

                        temp = Hash($"{modWord}{pass.Salt}");

                        if (temp.Equals(pass.HashString)) {
                            found = true;
                            pass.Pass = modWord;
                            pass.Time = DateTime.Now;
                        }
                    }

                    //Test with salt and appended characters
                    if (!found) {
                        temp = CheckAppended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            found = true;
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }

                    //Test with salt and prepended characters
                    if (!found) {
                        temp = CheckPrepended(word, pass.HashString, pass.Salt);

                        if (!String.IsNullOrEmpty(temp)) {
                            found = true;
                            pass.Pass = temp;
                            pass.Time = DateTime.Now;
                        }
                    }
                }

                //Check all special append cases
                passwords = CheckAppended(word, passwords);
                passwords = CheckAppended(word.ToUpper(), passwords);
                passwords = CheckAppended(ToTitleCase(word), passwords);
                passwords = CheckAppended(AlternateCase(word), passwords);
                passwords = CheckAppended(AlternateCase(word, false), passwords);
                passwords = CheckAppended(word, passwords, InitCommonAppends());

                //Check all special prepend cases
                passwords = CheckPrepended(word, passwords);
                passwords = CheckPrepended(word.ToUpper(), passwords);
                passwords = CheckPrepended(ToTitleCase(word), passwords);
                passwords = CheckPrepended(AlternateCase(word), passwords);
                passwords = CheckPrepended(AlternateCase(word, false), passwords);
                passwords = CheckPrepended(word, passwords, InitCommonAppends());
            }

            //Orders passwords by timestamp
            return passwords.OrderBy(pass => pass.Time).ToList();
        }
    }
}
