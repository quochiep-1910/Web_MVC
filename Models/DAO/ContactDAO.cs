using Models.EF;
using System.Linq;

namespace Models.DAO
{
    public class ContactDAO
    {
        private Web_MVC db = null;

        public ContactDAO()
        {
            db = new Web_MVC();
        }

        /// <summary>
        /// Get Active Contact
        /// </summary>
        /// <returns></returns>
        public Contact GetContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

        /// <summary>
        /// Insert to FeedBack
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public int Insert(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
    }
}