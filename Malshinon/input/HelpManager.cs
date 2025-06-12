using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.dal;
using Malshinon.people;
using Malshinon.reports;
using malshinon1.people;

namespace Malshinon.input
{
    internal class HelpManager
    {
        private Dal dal;
        public HelpManager(Dal dal)
        {
            this.dal = dal;
        }

        public (string firstName,string lastName) EnterFullName()
        {
            Console.WriteLine("enter your first name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("enter your last name: ");
            string lastName = Console.ReadLine();
            return (firstName, lastName);
        }
        public string EnterReport()
        {
            Console.WriteLine("enter the report, but make sure the target name is first: ");
            string text = Console.ReadLine();
            return text;
        }
        public bool ExistsInSystem(string firstName,string lastName)
        {
            var exists = this.dal.GetPersonByName(firstName, lastName);
            if(exists != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CreateNewPerson(string firstName, string lastName,string type)
        {
            string secretCode = SecretCode.CreateSecretCode(firstName, lastName);
            People person = new People(firstName,lastName,secretCode,type);
            this.dal.InsertNewPerson(person);
        }
        public (string firstName, string lastName) ExtractName(string report)
        {
            string[] text = report.Split(' ');
            if(text.Length >= 2)
            {
                return (text[0], text[1]);
            }
            else
            {
                return ("Unknown", "Unknown");
            }
        }
        public void CreateNewReport(string targetFirstName, string targetLastName, string reporterFirstName, string reporterLastName, string text)
        {
            var target = this.dal.GetPersonByName(targetFirstName, targetLastName);
            int targetId = target.id;
            var reporter = this.dal.GetPersonByName(reporterFirstName, reporterLastName);
            int reporterId = reporter.id;
            Report report = new Report(reporterId, targetId, text);
            this.dal.InsertIntelReport(report);
            this.dal.MarkAsDangerous(target.secret_code);
        }
        public void PrintPeopleList(List<People> peopleList)
        {
            foreach(People people in peopleList)
            {
                Console.WriteLine("=================================");
                Console.WriteLine($"id: {people.id}");
                Console.WriteLine($"first name: {people.first_name}");
                Console.WriteLine($"last name: {people.last_name}");
                Console.WriteLine($"secret code: {people.secret_code}");
                Console.WriteLine($"type: {people.type}");
                Console.WriteLine($"reports: {people.num_reports}");
                Console.WriteLine($"mentions: {people.num_mentions}");
                Console.WriteLine($"is dangerous: {people.is_dangerous}");
                Console.WriteLine("=================================");
            }
        }
    }
}
