using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.people;
using Malshinon.reports;
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
        public People GetPersonByName(string name)
        {
            People people = null;
            string query = @"SELECT * FROM people WHERE first_name = @first_name";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@first_name", name);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People(
                        reader.GetInt32("id"),
                        reader.GetString("first_name"),
                        reader.GetString("last_name"),
                        reader.GetString("secret_code"),
                        reader.GetString("type"),
                        reader.GetInt32("num_reports"),
                        reader.GetInt32("num_mentions")
                        );
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error selected {name}" + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return people;
        }
        public People GetPersonBySecretCode(string codeName)
        {
            People people = null;
            string query = @"SELECT * FROM people WHERE secret_code = @secret_code";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code", codeName);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People(
                        reader.GetInt32("id"),
                        reader.GetString("first_name"),
                        reader.GetString("last_name"),
                        reader.GetString("secret_code"),
                        reader.GetString("type"),
                        reader.GetInt32("num_reports"),
                        reader.GetInt32("num_mentions")
                        );
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selected {codeName}" + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return people;
        }
        public void InseIntelReport(Report report)
        {
            string query = @"INSERT INTO intalreports(reporter_id,target_id,text)
                           VALUES(@reporter_id,@target_id,@text)";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@reporter_id", report.reporter_id);
                cmd.Parameters.AddWithValue("@target_id", report.target_id);
                cmd.Parameters.AddWithValue("@text",report.text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding report: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
        public void UpdateReportCount(string secretCode)
        {
            string query = @"UPDATE people SET num_reports = num_reports + 1  
                           WHERE(secret_code = @secret_code)";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code",secretCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error update report count: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
        public void UpdateMentionCount(string secretCode)
        {
            string query = @"UPDATE people SET num_mentions = num_mentions + 1  
                           WHERE(secret_code = @secret_code)";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code", secretCode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error update mentions count: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
        public void GetReporterStats()
        {

        }
    }
}
