using Models.EF;
using System.Linq;

namespace Models.DAO
{
    public class FooterDAO
    {
        private Web_MVC db = null;

        public FooterDAO()
        {
            db = new Web_MVC();
        }

        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true); //lấy ra content đã được kích hoạt gắn vào footer trong class="footer"
        }
    }
}