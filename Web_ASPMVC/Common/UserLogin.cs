using System;

namespace Web_ASPMVC.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string GroupID { set; get; }
    }
}