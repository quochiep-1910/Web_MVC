using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserDAO
    {
        Web_MVC db = null;
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
            catch(Exception ex)
            {
                return false;
            }
           
        }
        public IEnumerable<User> ListAllPaging(string search, int page  ,int pageSize ) //lấy ra danh sách trong User
        {
            //có thể thêm 1 vài tham số tìm kiếm bằng cách thêm else if
            IQueryable<User> model = db.Users; //IQueryable là duyệt phần tử theo chiều tiến lên theo thứ tự
            if (!string.IsNullOrEmpty(search))//kiểm tra rỗng hoặc null
            {
                model = model.Where(x => x.UserName.Contains(search) || x.Name.Contains(search));    //tìm theo username và name( gần đúng or đúng)
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page,pageSize);    //lấy ra số bản ghi của database và sắp xếp theo thứ tự ngày tạo  và sắp xếp giảm dần(OrderByDescending)
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
        public int Login(string username, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username); //biểu thức lamda expression
            if (result == null) // kiểm tra tài khoản có tồn tại hay không tồn tài
            {
                return 0; //tài khoản không tồn tại
            }
            else
            {
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
    }
}
