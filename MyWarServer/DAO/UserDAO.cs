using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyWarServer.Model;

namespace MyWarServer.DAO
{
    class UserDAO
    {
        public User VerifyUser(string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                string sql = "select id,username,password from user where username = @username and password = @password";
                MySqlParameter[] param = {
                new MySqlParameter("@username", MySqlDbType.String),
                new MySqlParameter("@password", MySqlDbType.String)
            };
                param[0].Value = username;
                param[1].Value = password;
                reader = Tool.MySqlHelper.ExecuteReader(sql, param);
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    return new User(id, username, password);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("验证用户错误：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }
    }
}
