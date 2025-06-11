using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace malshinon1.people
{
    internal static class SecretCode
    {
        public static string CreateSecretCode(string firstName, string lastName)
        {
            string secretCode = "";
            for (int i = 0; i < (firstName.Length < lastName.Length ? firstName.Length : lastName.Length); i++)
            {
                if (i % 2 == 0)
                {
                    secretCode += firstName[i];
                }
                else
                {
                    secretCode += lastName[i];
                }
            }
            return secretCode;
        }
    }
}