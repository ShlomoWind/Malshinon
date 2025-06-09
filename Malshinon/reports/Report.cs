using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.reports
{
    internal class Report
    {
        public int id { get; }
        public int reporter_id { get; }
        public int target_id{ get; }
        public string text { get; }
        public string timestamp { get; }
        public Report(int id, int reporter_id, int target_id, string text, string timestamp)
        {
            this.id = id;
            this.reporter_id = reporter_id;
            this.target_id = target_id;
            this.text = text;
            this.timestamp = timestamp;
        }
    }
}