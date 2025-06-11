using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.input
{
    static class SecretCode
    {
        public static string CreateSecretCode(string firstName, string lastName)
        {
            string code = $"{firstName[0]}{firstName[firstName.Length -1]}{lastName[0]}{lastName[lastName.Length -1]}";
            return code;
        }
    }
}
