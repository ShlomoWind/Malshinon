using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.dal;
using Malshinon.input;

namespace Malshinon
{
    internal class Menu
    {

        private Dal dal;
        private Manager manager;
        private HelpManager helper;
        public Menu()
        {
            dal = new Dal();
            manager = new Manager(dal);
            helper = new HelpManager(dal);
        }
        public void UserManagement()
        {
            Console.WriteLine("Hi please enter Are you: \n1.Reporter \n2.Manager");
            string choise = Console.ReadLine();
            if(choise == "1")
            {
                manager.StartUsing();
            }
            else if(choise == "2")
            {
                //הפניה לתפריט השלושה
            }
            else
            {
                Console.WriteLine("Sorry, wrong choice, please try again.");
                UserManagement();
            }
        }
        public void AdminMenu()
        {
            Console.WriteLine("");
            string choise = Console.ReadLine();
            switch (choise)
            {
                case "1":
                    helper.PrintPotentialAgentsList(dal.AllPotentialAgents());
                    break;
                case "2":
                    //הצגת המטרות המסוכנות
                    break;
                case "3":
                    //הצגת ההתראות הפעילות
                    break;
            }
        }
    }
}
