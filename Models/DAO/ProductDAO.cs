using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class ProductDAO
    {
        private Web_MVC db = null;

        public ProductDAO()
        {
            db = new Web_MVC();
        }

        public List<Product> ListRow1(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList(); //lấy ra danh sách giảm dần
        }

        public List<Product> ListRow2(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRow3(int top)
        {
            //tophot khác null và còn hạn lớn hơn ngày hiện tại
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        /// <summary>
        /// lấy ra danh sách có id khác với id truyền vào và cùng danh mục
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<Product> ListRelatedProduct(long productID)//productID là lấy ra sản phẩm hiệm tại
        {
            var product = db.Products.Find(productID); //lay ra id gán vào product
            return db.Products.Where(x => x.ID != productID && x.CategoryID == product.CategoryID).ToList();//lấy ra danh sách có id khác với id truyền vào và cùng danh mục
        }

        public Product ViewDetail(long id)//lấy ra id để truyền lên ProductController
        {
            return db.Products.Find(id);
        }

        /// <summary>
        /// Get list product by category by pagelist
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryId(long categoryId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count(); //lấy ra được tổng số sản phẩm
            var model = db.Products.Where(x => x.CategoryID == categoryId)
               .OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        /// <summary>
        /// Lấy ra danh sách những từ khoá trùng với Product name
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        /// <summary>
        /// tìm kiếm theo tên nhập vào
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="totalRecord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Product> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = db.Products.Where(x => x.Name.Contains(keyword))
                .OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }
    }
}