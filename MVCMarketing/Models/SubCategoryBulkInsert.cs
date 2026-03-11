using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class SubCategoryBulkInsert
    {
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("SubCategory Name")]
        public string SubCategoryName { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}