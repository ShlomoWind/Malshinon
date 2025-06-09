using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.people;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Parameters;

namespace Malshinon.dal
{
    internal class Dal
    {
        //create the connection with the database
        private string connStr = "server=localhost;username=root;password=;database=malshinon";
        private MySqlConnection Conn;
        public Dal()
        {
            this.Conn = new MySqlConnection(this.connStr);
        }
        public MySqlCommand Command(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, this.Conn);
            return cmd;
        }
        //add a new person to the list
        public void InsertNewPerson(People people)
        {
            string query = @"INSERT INTO people(first_name,last_name,secret_code,type)
                           VALUES(@first_name,@last_name,@secret_code,@type)";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@first_name", people.first_name);
                cmd.Parameters.AddWithValue("@last_name", people.last_name);
                cmd.Parameters.AddWithValue("@secret_code", people.secret_code);
                cmd.Parameters.AddWithValue("@type", people.type);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error adding agent: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
    }
}
