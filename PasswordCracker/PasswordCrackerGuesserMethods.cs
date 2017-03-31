using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordCracker {
    public partial class PasswordCracker {
        private static List<Password> GuessPassword(List<Password> passwords) {
            string temp = string.Empty;

            bool found = false;

            foreach (var word in dictWords) {
                foreach (var pass in passwords) {
                    found = false;

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
                        found = CheckReplace(word, pass, "and", replaceDict["and"]);
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
                        found = CheckReplace(word, pass, "i", replaceDict["I"]);
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

                    if (!found) {
                        string[] arr =    { "a", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "1", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "a", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "1", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "a", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "!", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "a", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "!", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "at", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "1", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "at", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "1", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "at", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "!", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "at", "e", "i", "o", "s" };
                        string[] arrRep = { "@", "3", "!", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "and", "e", "i", "o", "s" };
                        string[] arrRep = { "&", "3", "1", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "and", "e", "i", "o", "s" };
                        string[] arrRep = { "&", "3", "1", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "and", "e", "i", "o", "s" };
                        string[] arrRep = { "&", "3", "!", "0", "$" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    if (!found) {
                        string[] arr =    { "and", "e", "i", "o", "s" };
                        string[] arrRep = { "&", "3", "!", "0", "5" };

                        found = CheckReplace(word, pass, arr, arrRep);
                    }

                    //Test with salt and alternated case
                    /*if (!found) {
                        found = CheckAlternateCase(word, pass);
                    }

                    //Test with salt and alternated case
                    if (!found) {
                        found = CheckAlternateCase(word, pass, false);
                    }*/

                    //Test with salt and appended characters
                    if (!found) {
                        found = CheckAppended(word, pass);
                    }
                }
                //Check all special append cases
                if (!found) {
                    found = CheckAppended(word, passwords);
                }

                //Check all special prepend cases
                /*if (!found) {
                    found = CheckPrepended(word, passwords);
                }

                if (!found) {
                    found = CheckAppendAndPrepend(word, passwords);
                }*/
            }

            //Orders passwords by timestamp
            return passwords.OrderBy(pass => pass.Time).ToList();
        }
    }
}
