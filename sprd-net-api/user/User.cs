using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sprd_net_api.client;

namespace sprd_net_api.user
{
    public class User
    {
        private const string SESSION_PATH = "http://api.spreadshirt.net/api/v1/sessions";

        public void CreateSession(string username, string password)
        {
            ApiClient.Instance.Post(SESSION_PATH, new loginDTO() {username = username, password = password}).Wait();
        }
    }
}
