using Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class ProductCategoryDAO
    {
        private Web_MVC db = null;

        public ProductCategoryDAO()
        {
            db = new Web_MVC();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}