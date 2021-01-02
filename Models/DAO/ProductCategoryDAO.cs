using Models.EF;
using PagedList;
using System;
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

        public long Insert(ProductCategory entity)
        {
            entity.CreatedDate = DateTime.Now;
            db.ProductCategories.Add(entity);//truyền vào tham số rồi tự động add vào data thông qua SaveChanges
            db.SaveChanges();
            return entity.ID; //tự sinh ra ID
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> ListAllPaging(string search, int page, int pageSize) //lấy ra danh sách trong ProductCategory
        {
            //có thể thêm 1 vài tham số tìm kiếm bằng cách thêm else if
            IQueryable<ProductCategory> model = db.ProductCategories; //IQueryable là duyệt phần tử theo chiều tiến lên theo thứ tự
            if (!string.IsNullOrEmpty(search))//kiểm tra rỗng hoặc null
            {
                model = model.Where(x => x.Name.Contains(search));    //tìm theo username và name( gần đúng or đúng)
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);    //lấy ra số bản ghi của database và sắp xếp theo thứ tự ngày tạo  và sắp xếp giảm dần(OrderByDescending)
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool Delete(int id)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(ProductCategory entity)
        {
            try
            {
                var productcategory = db.ProductCategories.Find(entity.ID); //var ra thực thể gán vào đối tượng
                productcategory.Name = entity.Name;
                productcategory.MetaTitle = entity.MetaTitle;
                productcategory.ParentID = entity.ParentID;
                productcategory.DisplayOrder = entity.DisplayOrder;
                productcategory.SeoTitle = entity.SeoTitle;
                productcategory.ModifiedDate = DateTime.Now;
                productcategory.MetaKeywords = entity.MetaKeywords;
                productcategory.MetaDescriptions = entity.MetaDescriptions;
                productcategory.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var productCategory = db.ProductCategories.Find(id);
            productCategory.Status = !productCategory.Status; //cứ giá trị nào trong status là true thì chuyển thành false (đảo ngược giá trị)
            db.SaveChanges();
            return productCategory.Status;
        }
    }
}