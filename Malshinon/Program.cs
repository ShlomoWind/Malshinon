using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.dal;
using Malshinon.input;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var Dal = new Dal();
            var Manager = new Manager(Dal);
            Manager.StartUsing();
        }
    }
}
