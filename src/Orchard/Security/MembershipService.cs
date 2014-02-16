using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Security
{
    public class MembershipService : IMembershipService
    {
        public MembershipSettings GetSettings()
        {
            return new MembershipSettings();
        }

        public IUser CreateUser(CreateUserParams createUserParams)
        {
            return null;
        }

        public IUser GetUser(string username)
        {
            return null;
        }

        public IUser ValidateUser(string userNameOrEmail, string password)
        {
            return null;
        }

        public void SetPassword(IUser user, string password)
        {            
        }
    }
}
