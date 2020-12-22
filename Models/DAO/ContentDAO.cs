using Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class ContentDAO
    {
        public static string USER_SESSION = "USER_SESSION";
        private Web_MVC db = null;

        public ContentDAO()
        {
            db = new Web_MVC();
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }

        public long Create(Content content)
        {
            //xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name); //chuyển từ có dấu sang không dấu
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0; //mặc định số người đã xem là 0
            db.Contents.Add(content);
            db.SaveChanges();

            //xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))//string khác rỗng
            {
                string[] tags = content.Tags.Split(','); //lấy ra danh sách tag đã nhập vào
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);
                    if (!existedTag)//nếu giá trị ko tồn tại thì insert
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //insert content tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }
            return content.ID;
        }

        public long Edit(Content content)
        {
            //Xử lý alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(','); //lấy từng đối tượng ra cách nhau dấu ,
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId); //nếu check tag false thì insert, nếu có rồi thì update

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }
            return content.ID;
        }

        /// <summary>
        /// xoá đi thẻ tag của đối tượng truyền vào
        /// </summary>
        /// <param name="contentId"></param>
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }

        /// <summary>
        /// Insert giá trị tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        /// <summary>
        /// Insert vào bảng Content Tag (để list ra danh sách cùng loại với nó)
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="tagId"></param>
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        /// <summary>
        /// kiểm tra xem trong tag có giá trị này hay chưa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        /// <summary>
        /// lấy ra danh sách từ 2 bảng Tag và ContentTag có trùng tag với nhau thông qua Tagid
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }

        /// <summary>
        /// lấy ra danh sách trong Content
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(string search, int page, int pageSize) //lấy ra danh sách trong User
        {
            //có thể thêm 1 vài tham số tìm kiếm bằng cách thêm else if
            IQueryable<Content> model = db.Contents; //IQueryable là duyệt phần tử theo chiều tiến lên theo thứ tự
            if (!string.IsNullOrEmpty(search))//kiểm tra rỗng hoặc null
            {
                model = model.Where(x => x.Name.Contains(search) || x.Name.Contains(search));    //tìm theo Name và name( gần đúng or đúng)
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);    //lấy ra số bản ghi của database và sắp xếp theo thứ tự ngày tạo  và sắp xếp giảm dần(OrderByDescending)
        }

        /// <summary>
        /// lấy ra danh sách tag được join từ 2 bảng
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllByTag(string tag, ref int totalRecord, int page, int pageSize)
        {
            totalRecord = db.Contents.Where(x => x.Tags == tag).Count(); //lấy ra được tổng số sản phẩm
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             ID = a.ID
                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }

        /// <summary>
        /// Liệt kê tất cả nội dung cho khách hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}