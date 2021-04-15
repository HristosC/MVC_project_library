using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MVC_Project.Models
{
    public class display_anafores
    {
        public string auth_name { get; set; }
        public string auth_lastname { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string order_id { get; set; }
        public string store_name { get; set; }
        public string title_name { get; set; }
        public List<display_anafores> info { get; set; }

    }
}