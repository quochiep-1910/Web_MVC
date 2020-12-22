using Common;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class UserDAO
    {
        private Web_MVC db = null;

        public UserDAO()
        {
            db = new Web_MVC();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity); //truyền vào tham số rồi tự động add vào data thông qua SaveChanges
            db.SaveChanges();
            return entity.ID; //tự sinh ra ID
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity); //truyền vào tham số rồi tự động add vào data thông qua SaveChanges
                db.SaveChanges();
                return entity.ID; //tự sinh ra ID
            }
            else
            {
                return user.ID;
            }
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID); //var ra thực thể gán vào đối tượng
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<User> ListAllPaging(string search, int page, int pageSize) //lấy ra danh sách trong User
        {
            //có thể thêm 1 vài tham số tìm kiếm bằng cách thêm else if
            IQueryable<User> model = db.Users; //IQueryable là duyệt phần tử theo chiều tiến lên theo thứ tự
            if (!string.IsNullOrEmpty(search))//kiểm tra rỗng hoặc null
            {
                model = model.Where(x => x.UserName.Contains(search) || x.Name.Contains(search));    //tìm theo username và name( gần đúng or đúng)
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);    //lấy ra số bản ghi của database và sắp xếp theo thứ tự ngày tạo  và sắp xếp giảm dần(OrderByDescending)
        }

        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName); //lấy 1 bản ghi đơn theo Username truyền vào
            //tạo 1 biến x
            //gán x.UserName ==userName
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        public int Login(string username, string password, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username); //biểu thức lamda expression
            if (result == null) // kiểm tra tài khoản có tồn tại hay không tồn tài
            {
                return 0; //tài khoản không tồn tại
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;//Tài khoản đang bị khoá
                        }
                        else
                        {
                            if (result.Password == password)
                                return 1; //chạy tiếp
                            else
                                return -2; //Mật Khẩu không đúng
                        }
                    }
                    else
                    {
                        return -3; //Tài khoản không có quần đăng nhập
                    }
                }
                if (result.Status == false)  // tài khoản chưa active
                {
                    return -1; //tài khoản bị khoá
                }
                else
                {
                    if (result.Password == password) //kiểm tra mật khẩu
                        return 1; //mật khẩu đúng
                    else
                        return -2; //mật khẩu sai
                }
            }
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status; //cứ giá trị nào trong status là true thì chuyển thành false (đảo ngược giá trị)
            db.SaveChanges();
            return user.Status;
        }

        /// <summary>
        /// check trùng user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0; // >0 tồn tại user trùng
        }

        /// <summary>
        /// check trùng email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0; // >0 tồn tại user trùng
        }

        //cái này làm cho biết
        /// <summary>
        /// lấy ra chứng nhận (sự cho phép truy cập vào quyền xoá sửa)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }
    }
}