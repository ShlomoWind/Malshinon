using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.dal;
using Malshinon.people;
using Malshinon.reports;

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
            int reporterId = target.id;
            Report report = new Report(reporterId, targetId, text);
            this.dal.InsertIntelReport(report);
        }
        public void UpdateReporterStatus(string firstName, string lastName)
        {
            var person = this.dal.GetPersonByName(firstName, lastName);
            if(person.type == "reporter")
            {
                this.dal.UpdateStatusBoth(firstName, lastName);
            }
        }
        public void UpdateTargetStatus(string firstName, string lastName)
        {
            var person = this.dal.GetPersonByName(firstName, lastName);
            if (person.type == "target")
            {
                this.dal.UpdateStatusBoth(firstName, lastName);
            }
        }
    }
}
