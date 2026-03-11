using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class BulkInsertModel
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public string ClientCode { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string HSN { get; set; }
        public string Unit { get; set; }
        public float GST { get; set; }
        public float Sales_Price { get; set; }
        public float Purchase_Price { get; set; }
        public string PriceType { get; set; }
        public float Discount { get; set; }
        public float WiderWidth { get; set; }
        public float Custom_Sales_Price { get; set; }
        public float Custom_Purchase_Price { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }
        public float StockLimit { get; set; }
        public float OpeningStock { get; set; }
        public string Status { get; set; }
    }
}