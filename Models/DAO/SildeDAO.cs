using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class SildeDAO
    {
        Web_MVC db = null;
        public SildeDAO()
        {
            db = new Web_MVC();
        }
        public List<Slide>ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
       
    }
}
