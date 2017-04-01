using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordCracker {
    public partial class PasswordCracker {
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

            List<string> listResult = result.ToList();

            for (int i = 0; i < listResult.Count; ++i) {
                if (Char.IsNumber(listResult[i][0])) {
                    listResult.RemoveRange(i, 1);
                    --i;
                }
            }

            result = listResult.ToArray();

            string text = "";

            foreach (string word in result) {
                text += word + "\n";
            }

            File.WriteAllText("./dict.txt", text);

            result = result.OrderBy(s => s[0]).ToArray();

            return result.Distinct().ToArray();
        }

        private static void WriteOutputFile(List<Password> passwords,
                                               string file) {
            string result = string.Empty;

            foreach (var pass in passwords) {
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
    }
}
