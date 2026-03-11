using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class CategoryBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        //[DisplayName("Family")]
        //public string Family { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}