using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.input;
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
        public People GetPersonByName(string firstName,string lastName)
        {
            People people = null;
            string query = @"SELECT * FROM people WHERE first_name = @first_name AND last_name = @last_name";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People
                        (
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
                Console.WriteLine($"Error selected {firstName} {lastName}" + ex.Message);
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
        public void InsertIntelReport(Report report)
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
                Console.WriteLine("נוסף");

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
        public (int,double) GetReporterStats(int reporterId)
        {
            string query = @"SELECT COUNT(*) AS count, AVG(CHAR_LENGTH(text)) AS avgLength FROM intelreports WHERE reporter_id = @reporter_id";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int count = reader.GetInt32("count");
                    double avgLength = reader.GetDouble("avgLength");
                    return (count, avgLength);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error return statistic of reporter: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return (0, 0);
        }
        public (int,int) GetTargetStats(string secretCode)
        {
            string query = @"SELECT p.num_mentions AS totalMentions, COUNT(i.id) AS mentionsLast15Min
                           FROM People p
                           LEFT JOIN intelreports i
                           ON i.target_id = p.id 
                           AND i.timestamp >= NOW() - INTERVAL 15 MINUTE
                           WHERE p.secret_code = @secret_code
                           GROUP BY p.num_mentions";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code", secretCode);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int totalMentions = reader.GetInt32("totalMentions");
                    int mentionsLast15Min = reader.GetInt32("mentionsLast15Min");
                    return (totalMentions, mentionsLast15Min);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error return statistic of reporter: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return (0, 0);
        }
        public void UpdateStatus(string firstName, string lastName, string status)
        {
            string query = "UPDATE people SET type = @status WHERE first_name = @first_name AND last_name = @last_name";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error update type: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
    }
}