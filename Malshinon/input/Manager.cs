using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.input
{
    internal class Manager
    {
        public (string firstName,string lastName) EnterName()
        {
            Console.WriteLine("enter your first name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("enter your last name: ");
            string lastName = Console.ReadLine();
            return (firstName, lastName);
        }
    }
}
