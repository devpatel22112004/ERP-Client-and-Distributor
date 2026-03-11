using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class MatrixItem
    {
        public string MatrixItemsId { get; set; }
        public string MatrixId { get; set; }
        public string Floor { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string Location { get; set; }
        public string Item { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public string GST { get; set; }
        public string Assemble { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Controller { get; set; }
        public string Remark { get; set; }
        public string Position { get; set; }
    }
}