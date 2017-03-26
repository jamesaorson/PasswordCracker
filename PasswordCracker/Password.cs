using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCracker {
    class Password {
        public string Name { get; set; }
        public string HashString { get; set; }
        public string Salt { get; set; }
        public string Pass { get; set; }
        
        public Password(string n = "", string h = "", string s = "", string p = "") {
            Name = n;
            HashString = h;
            Salt = s;
            Pass = p;
        }
    }
}
