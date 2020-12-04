using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContentDAO
    {
        Web_MVC db = null;
        public ContentDAO()
        {
            db = new Web_MVC();
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
    }
}
