using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using sprd_net_api.user;

namespace sprd_net_api_tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CreateSession()
        {
            var user = new User();
            user.CreateSession("username", "password");
        }
    }
}
