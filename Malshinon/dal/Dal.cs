using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Malshinon.dal
{
    internal class Dal
    {
        //Establishes the connection with the database
        private string connStr = "server=localhost;username=root;password=;database=malshinon";
        private MySqlConnection Conn;
        public Dal()
        {
            this.Conn = new MySqlConnection(this.connStr);
        }
    }
}
