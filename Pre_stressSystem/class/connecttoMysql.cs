using System;
using System.Windows;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Pre_stressSystem
{
    class ConnecttoMysql

    {
        public static bool hasSearched = false;
        public  static  Dictionary<int, KeyValuePair<string ,string>> input = new Dictionary<int, KeyValuePair<string, string>>();       

        public static MySqlConnection getMySqlCon()
        {
            String mysqlStr = "server=127.0.0.1;Database=prestress;User Id=root;Password=921123;port=3306";
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            mysql.Open();
            return mysql;
        }

        public static void getLoginResult()
        {
            clearData();
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            string order = "select * from user_tb";
            MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            
            try
            {
                while (reader.Read()) //read by lows
                {
                    //test

                    int a = reader.GetOrdinal("employee_name"); //1
                    int g = reader.GetOrdinal("gender");//3
                    int b = reader.Depth; //===0
                    int c = reader.FieldCount; //11
                    Type d= reader.GetFieldType(1); //string
                    Type d2 = reader.GetFieldType(0);//Uint32
                    Type d3 = reader.GetFieldType(3);//string
                    string e = reader.GetName(1); //employee_id
                    object f = reader.GetValue(1);//王志文 tovey ....
                    Type ff = f.GetType();                   
                    //end test

                    string name= reader.GetString(reader.GetOrdinal("employee_name"));
                    int id = reader.GetInt32(reader.GetOrdinal("employee_id"));
                    string pwd = reader.GetString(reader.GetOrdinal("employee_pwd"));
                    KeyValuePair<string, string> namepwd = new KeyValuePair<string, string>(name,pwd);
                    input.Add(id, namepwd);
                }
                hasSearched = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("登陆失败！" + e.Message);
            }
            finally
            {
                reader.Close();
                mysql.Close();
            }
            
        }

        public static Dictionary<string, object> getUserInfo()
        {
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            string order = "select * from user_tb where employee_id=" + UserInfo.employee_id;
            MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);
            try
            {
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read()) //read by lows
                {
                    int number = reader.GetInt32(reader.GetOrdinal("employee_id"));
                    string name = reader.GetString(reader.GetOrdinal("employee_name"));             
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
                  //  break;
                }
                return userInfo;

            }
            catch (Exception e)
            {
                MessageBox.Show("数据库查询失败！" + e.Message);
                return null;
            }
            finally
            {
                
                mysql.Close();
            }
        }

        public static void clearData()
        {
            input.Clear();
        }

        public static List<Dictionary<string, object>> query(string order)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);         
            try
            {
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read()) //read by lows
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    int col = reader.FieldCount; //11
                    for (int i = 0; i < col; i++)
                    {
                        dic.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    list.Add(dic);
                    dic = null;
                }
                return list ;
            }
            catch(Exception e)
            {
                MessageBox.Show("访问数据库出错!"+e.Message);
                return list;
            }
            finally
            {
                mysql.Close();

            }
            
        }

        public static bool insert(string order)
        {
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            MySqlCommand SqlCommand= new MySqlCommand(order, mysql);
            try
            {
                SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("插入数据失败！" + ex.Message);
                return false;
            }
            finally
            {
                mysql.Close();
            }
        }

        public static bool update(string order)
        {
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            MySqlCommand SqlCommand = new MySqlCommand(order, mysql);
            try
            {
                SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新数据失败！" + ex.Message);
                return false;
            }
            finally
            {
                mysql.Close();
            }
        }

        public static bool delete(string order)
        {
            MySqlConnection mysql = ConnecttoMysql.getMySqlCon();
            MySqlCommand SqlCommand = new MySqlCommand(order, mysql);
            try
            {
                SqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除数据失败！" + ex.Message);
                return false;
            }
            finally
            {
                mysql.Close();
            }
        }

    }
}
