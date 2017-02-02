using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Pre_stressSystem
{
    class connecttoMysql

    {
        public static bool hasSearched = false;
        public  static  Dictionary<int, KeyValuePair<string ,string>> input = new Dictionary<int, KeyValuePair<string, string>>();
       // private static Dictionary<string, object>[] info;
        public static Dictionary<string,object> userInfo = new Dictionary<string, object>();

        public static MySqlConnection getMySqlCon()
        {
            String mysqlStr = "server=127.0.0.1;Database=prestress;User Id=root;Password=921123;port=3306";
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            mysql.Open();
            return mysql;
        }

        public static void getLoginResult(MySqlCommand mySqlCommand)
        {
            cleardata();           
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            
            try
            {
                while (reader.Read()) //read by lows
                {
                    string id= reader.GetString(reader.GetOrdinal("employee_ID"));
                    int number = reader.GetInt32(reader.GetOrdinal("employee_Number"));
                    string pwd = reader.GetString(reader.GetOrdinal("employee_pwd"));
                    KeyValuePair<string, string> namepwd = new KeyValuePair<string, string>(id,pwd);
                    input.Add(number, namepwd);
                }
                hasSearched = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据库查询失败！" + e.ToString());
            }
            finally
            {
                reader.Close();
            }
        }

        public static void getUserInfo(MySqlCommand mySqlCommand)
        {
            cleardata();
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            try
            {
                while (reader.Read()) //read by lows
                {
                    int number = reader.GetInt32(reader.GetOrdinal("employee_Number"));
                    //if (number!= GlobalVariable.userNumber)
                    //{
                    //    continue;
                    //}
                    string name = reader.GetString(reader.GetOrdinal("employee_ID"));             
                    string pwd = reader.GetString(reader.GetOrdinal("employee_pwd"));
                    string gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : reader.GetString(reader.GetOrdinal("gender"));
                    string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                    string birthday = reader.IsDBNull(reader.GetOrdinal("birthday")) ? null : reader.GetString(reader.GetOrdinal("birthday"));
                    string department = reader.IsDBNull(reader.GetOrdinal("department")) ? null : reader.GetString(reader.GetOrdinal("department"));
                    string Email = reader.IsDBNull(reader.GetOrdinal("Email"))?null: reader.GetString(reader.GetOrdinal("Email"));
                    string lever = reader.IsDBNull(reader.GetOrdinal("lever")) ? null : reader.GetString(reader.GetOrdinal("lever"));
                    string address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address"));
                    userInfo.Add("name", name);
                    userInfo.Add("number", number);
                    userInfo.Add("pwd", pwd);
                    userInfo.Add("gender", gender);
                    userInfo.Add("phone", phone);
                    userInfo.Add("birthday", birthday);
                    userInfo.Add("department", department);
                    userInfo.Add("Email", Email);
                    userInfo.Add("lever", lever);
                    userInfo.Add("address", address);
                    break;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("数据库查询失败！" + e.ToString());
            }
            finally
            {
                reader.Close();
            }
        }

        public static void cleardata()
        {
            input.Clear();
            userInfo.Clear();
        }
    }
}
