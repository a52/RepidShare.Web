using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class HomeModel
    {
        public int ErrorLogID { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStack { get; set; }
        public string ErrorPage { get; set; }
        public string ErrorFunction { get; set; }
    }
}
