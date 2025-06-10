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
        public Manager(Dal dal)
        {
            this.helper = new HelpManager(dal);
        }
        public void StartUsing()
        {
            (string reporterFirstName, string reporterLastName) = helper.EnterFullName();
            if (!this.helper.ExistsInSystem(reporterFirstName, reporterLastName))
            {
                this.helper.CreateNewPerson(reporterFirstName, reporterLastName, "reporter");
            }
            else
            {
                this.helper.UpdateTargetStatus(reporterFirstName, reporterLastName);
            }
        }
    }
}
