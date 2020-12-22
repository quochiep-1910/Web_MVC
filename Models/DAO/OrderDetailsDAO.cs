using Models.EF;

namespace Models.DAO
{
    public class OrderDetailsDAO
    {
        private Web_MVC db = null;

        public OrderDetailsDAO()
        {
            db = new Web_MVC();
        }

        public bool Insert(OrderDetail details)
        {
            try
            {
                db.OrderDetails.Add(details);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}