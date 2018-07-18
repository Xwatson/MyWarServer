using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWarCommon;
using MyWarServer.Servers;
using MyWarServer.Model;
using MyWarServer.DAO;

namespace MyWarServer.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] datas = data.Split(',');
            User user = userDAO.VerifyUser(datas[0], datas[1]);
            if (user != null)
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}
