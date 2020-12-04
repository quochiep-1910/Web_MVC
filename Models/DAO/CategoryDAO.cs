using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CategoryDAO
    {
        Web_MVC db = null;
        public CategoryDAO()
        {
            db = new Web_MVC();
        }
        public List<Category> listAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList(); //lấy ra các mục đã được active show lên
        }
      
    }
}
