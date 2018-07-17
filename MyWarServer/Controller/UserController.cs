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
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public void Login(string data, Client client, Server server)
        {
            UserDAO userDAO = new UserDAO();
            // User user = userDAO.VerifyUser()
        }
    }
}
