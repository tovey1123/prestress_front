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
       // public class mylist<A, B, C, D> { };
        public static MySqlConnection getMySqlCon()
        {
            String mysqlStr = "server=127.0.0.1;Database=prestress;User Id=root;Password=921123;port=3306";
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            mysql.Open();
            return mysql;
        }
        public static void getResultset(MySqlCommand mySqlCommand)
        {
            
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


    }
}
