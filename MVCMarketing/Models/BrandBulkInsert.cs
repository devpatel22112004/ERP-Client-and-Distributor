using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class BrandBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Label")]
        public string Label { get; set; }
        
        /*[DisplayName("Family")]
        public string Family { get; set; }*/

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}