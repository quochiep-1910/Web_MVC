using Models.EF;
using System.Data.SqlClient;
using System.Linq;

namespace Models
{
    public class AccountModel
    {
        private Web_MVC context = null;

        public AccountModel()
        {
            context = new Web_MVC();
        }

        public bool Login(string username, string password)
        {
            object[] sqlParams =
              {
                new SqlParameter("@UserName",username),
               new SqlParameter("@Password",password),
            };
            var res = context.Database.SqlQuery<bool>("Sp_Account_login_Admin @UserName, @Password", sqlParams).SingleOrDefault();
            return res;
        }
    }
}