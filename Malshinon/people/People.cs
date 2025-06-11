using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.people
{
    public class People
    {
        public int id { get; }
        public string first_name { get; }
        public string last_name { get; }
        public string secret_code { get; }
        public string type { get; }
        public int num_reports { get; }
        public int num_mentions { get; }
        public bool is_dangerous { get; }

        public People(int id, string first_name, string last_name, string secret_code, string type, int num_reports, int num_mentions, bool is_dangerous)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.secret_code = secret_code;
            this.type = type;
            this.num_reports = num_reports;
            this.num_mentions = num_mentions;
            this.is_dangerous = is_dangerous;
        }
        public People(string first_name, string last_name,string secret_code, string type)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.secret_code = secret_code;
            this.type = type;
        }
    }
}
