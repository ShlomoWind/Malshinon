using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Malshinon.dal;

namespace Malshinon.input
{
    internal class Manager
    {
        private HelpManager helper;
        private Dal dal;
        public Manager(Dal dal)
        {
            this.helper = new HelpManager(dal);
            this.dal = dal;
        }
        public void StartUsing()
        {
            (string reporterFirstName, string reporterLastName) = helper.EnterFullName();
            if (!this.helper.ExistsInSystem(reporterFirstName, reporterLastName))
            {
                this.helper.CreateNewPerson(reporterFirstName, reporterLastName, "reporter");
            }
            var reporter = this.dal.GetPersonByName(reporterFirstName, reporterLastName);
            if(reporter.type == "target")
            {
                this.dal.UpdateStatus(reporterFirstName, reporterLastName,"both");
            }
            this.dal.UpdateReportCount(reporter.secret_code);
            string report = this.helper.EnterReport();
            (string targetFirstName, string targetLastName) = this.helper.ExtractName(report);
            if (!this.helper.ExistsInSystem(targetFirstName, targetLastName))
            {
                this.helper.CreateNewPerson(targetFirstName, targetLastName,"target");
            }
            var target = this.dal.GetPersonByName(targetFirstName, targetLastName);
            if(target.type == "reporter")
            {
                this.dal.UpdateStatus(targetFirstName, targetLastName,"both");
            }
            this.dal.UpdateMentionCount(target.secret_code);
            this.helper.CreateNewReport(targetFirstName, targetLastName, reporterFirstName, reporterLastName, report);
        }
        public bool PotentialAgent(int reporterId)
        {
            (int count, double avgLength) = this.dal.GetReporterStats(reporterId);
            if (count >= 10 || avgLength >= 100)
            {
                return true;
            }
            return false;
        }

    }
}
