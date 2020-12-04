using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MenuDAO
    {
        Web_MVC db = null;
        public MenuDAO()
        {
            db = new Web_MVC();
        }
        public List<Menu> ListByGroupID(int group)
        {
            //lấy ra kiểu id và trạng thái và sắp xếp theo display
            return db.Menus.Where(x => x.TypeID == group &&x.Status==true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
