using Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class SildeDAO
    {
        private Web_MVC db = null;

        public SildeDAO()
        {
            db = new Web_MVC();
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
    }
}