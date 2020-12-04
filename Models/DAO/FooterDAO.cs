﻿using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FooterDAO
    {
        Web_MVC db = null;
        public FooterDAO()
        {
            db = new Web_MVC();
        }
        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true); //lấy ra content đã được kích hoạt gắn vào footer trong class="footer"
        }
    }
}
