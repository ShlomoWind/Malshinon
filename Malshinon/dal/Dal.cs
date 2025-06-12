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
    public class Dal
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
        //add a new person (type People) to the people table
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
        //return a object People from people table by first name and last name
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
                        reader.GetInt32("num_mentions"),
                        reader.GetBoolean("is_dangerous")
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
        //return a object People from people table by code name
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
                        reader.GetInt32("num_mentions"),
                        reader.GetBoolean("is_dangerous")
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
        //add a new report (type Report) to the intalreports table
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
        //update the reporters count with one more
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
        //update the mentions count with one more
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
        //return for an reporter the number of reports and average chars of all reports 
        public (int num_reports,double avg_chars) GetReporterStats(int reporterId)
        {
            string query = @"SELECT COUNT(*) AS count, AVG(CHAR_LENGTH(text)) AS avgLength FROM intalreports WHERE reporter_id = @reporter_id";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int count = reader.GetInt32("count");
                    double avgLength = reader.IsDBNull(reader.GetOrdinal("avgLength")) ? 0.0 : reader.GetDouble("avgLength");
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
        //return for an target the sum of mentions in the last 15 minutes
        public int GetTargetStats(string secretCode)
        {
            string query = @"SELECT COUNT(i.id) AS mentionsLast15Min
                             FROM people p
                             LEFT JOIN intalreports i 
                             ON i.target_id = p.id 
                             AND i.timestamp >= NOW() - INTERVAL 15 MINUTE
                             WHERE p.secret_code = @secret_code";
            int mentionsLast15Min = 0;
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code", secretCode);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mentionsLast15Min = reader.GetInt32("mentionsLast15Min");

                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving target stats: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return mentionsLast15Min;
        }
        //updates the person's status in the table
        public void UpdateStatus(string firstName, string lastName, string status)
        {
            string query = "UPDATE people SET type = @status WHERE first_name=@first_name AND last_name=@last_name";
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
        //return an list of all people they are potential agents
        public List<People> AllPotentialAgents()
        {
            string query = "SELECT * FROM people WHERE type = 'potential_agent'";
            List<People> potentialList = new List<People>();
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    People people = new People
                        (
                        reader.GetInt32("id"),
                        reader.GetString("first_name"),
                        reader.GetString("last_name"),
                        reader.GetString("secret_code"),
                        reader.GetString("type"),
                        reader.GetInt32("num_reports"),
                        reader.GetInt32("num_mentions"),
                        reader.GetBoolean("is_dangerous")
                        );
                    potentialList.Add(people);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selected list of all potential agents" + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return potentialList;
        }
        //update a person as dangerous (from true to false) by column is_tanger
        public void MarkAsDangerous(string secret_code)
        {
            string query = @"UPDATE people 
                     SET is_dangerous = TRUE 
                     WHERE secret_code = @secret_code 
                     AND num_mentions >= 20 
                     AND is_dangerous = FALSE";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@secret_code", secret_code);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error update status to danger: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
        //return an list of all people they dangers people
        public List<People> AllDangersPeople()
        {
            string query = "SELECT * FROM people WHERE is_dangerous = '1'";
            List<People> dangerslList = new List<People>();
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    People people = new People
                        (
                        reader.GetInt32("id"),
                        reader.GetString("first_name"),
                        reader.GetString("last_name"),
                        reader.GetString("secret_code"),
                        reader.GetString("type"),
                        reader.GetInt32("num_reports"),
                        reader.GetInt32("num_mentions"),
                        reader.GetBoolean("is_dangerous")
                        );
                    dangerslList.Add(people);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selected list of all potential agents" + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
            return dangerslList;
        }
        //insert a worning alert of target in table alerts
        public void InsertAlertOfTarget(int target_id)
        {
            string query = @"INSERT INTO alerts(target_id,alert)
                           VALUES(@target_id,@alert)";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                cmd.Parameters.AddWithValue("@target_id", target_id);
                cmd.Parameters.AddWithValue("@alert", "Warning! Over 3 reports have been issued on this target in the last fifteen minutes!");
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding alert: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
        public void PrintAllAlerts()
        {
            string query = @"SELECT a.alert, p.first_name, p.last_name
                             FROM alerts a
                             JOIN people p 
                             ON a.target_id = p.id";
            try
            {
                this.Conn.Open();
                var cmd = this.Command(query);
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string first_name = reader.GetString("first_name");
                    string last_name = reader.GetString("last_name");
                    string alert = reader.GetString("alert");
                    Console.WriteLine("========================================");
                    Console.WriteLine($"ALERT ON: {first_name} {last_name}");
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine(alert);
                    Console.WriteLine("========================================\n");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error printing alerts: " + ex.Message);
            }
            finally
            {
                this.Conn.Close();
            }
        }
    }
}