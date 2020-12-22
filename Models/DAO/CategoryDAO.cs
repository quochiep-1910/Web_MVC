using Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class CategoryDAO
    {
        private Web_MVC db = null;

        public CategoryDAO()
        {
            db = new Web_MVC();
        }

        /// <summary>
        /// lấy ra thông tin của category
        /// </summary>
        /// <returns></returns>
        public List<Category> listAll()
        {
            return db.Categories.OrderByDescending(x => x.ID).ToList(); //lấy ra các mục đã được active show lên
        }

        /// <summary>
        /// Insert Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }
    }
}