using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citacka_hesiel
{
    class User
    {
        public bool ValidUser { get; private set; }
        public string Name { get; private set; }
        public string HasehedPassword { get; private set; }
        public string ThrownExeption { get; private set; }
        

        public User(bool validuser, string name, string password, string exeption)
        {
            ValidUser = validuser;
            Name = name;
            HasehedPassword = password;
            ThrownExeption = exeption;
        }
    }
}
