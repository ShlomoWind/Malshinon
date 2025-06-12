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
            string choice = Console.ReadLine();
            if(choice == "1")
            {
                manager.StartUsing();
            }
            else if(choice == "2")
            {
                Console.WriteLine("Hi please enter your selection: \n1. View the list of potential agents \n2. View the list of dangerous targets \n3. View active alerts");
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        helper.PrintPeopleList(dal.AllPotentialAgents());
                        break;
                    case "2":
                        helper.PrintPeopleList(dal.AllDangersPeople());
                        break;
                    case "3":
                        dal.GetAllAlerts();
                        break;
                    default:
                        Console.WriteLine("Sorry, wrong choice");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Sorry, wrong choice, please try again.");
                this.UserManagement();
            }
        }
    }
}
