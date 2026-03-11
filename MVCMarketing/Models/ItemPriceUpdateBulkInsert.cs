using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class ItemPriceUpdateBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("ItemId")]
        public string ItemId { get; set; }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Sub Category")]
        public string SubCategoryName { get; set; }

        [DisplayName("Brand")]
        public string Label { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Packing Unit")]
        public string Unit { get; set; }

        [DisplayName("Packing")]
        public string Packing { get; set; }

        [DisplayName("HSN")]
        public string HSN { get; set; }

        [DisplayName("GST")]
        public string GST { get; set; }

        [DisplayName("Purchase Rate")]
        public string Purchase_Rate { get; set; }

        [DisplayName("Sales Rate")]
        public string Sales_Rate { get; set; }

        [DisplayName("Purchase Discount 1")]
        public string Purchase_Discount1 { get; set; }

        [DisplayName("Purchase Discount 2")]
        public string Purchase_Discount2 { get; set; }

        [DisplayName("Purchase Discount 3")]
        public string Purchase_Discount3 { get; set; }

        [DisplayName("Sales Discount 1")]
        public string Sales_Discount1 { get; set; }

        [DisplayName("Sales Discount 2")]
        public float Sales_Discount2 { get; set; }

        [DisplayName("Sales Discount 3")]
        public float Sales_Discount3 { get; set; }

        [DisplayName("Length")]
        public float Length { get; set; }

        [DisplayName("Width")]
        public string Width { get; set; }

        [DisplayName("Height")]
        public string Height { get; set; }

        [DisplayName("Depth")]
        public string Depth { get; set; }

        [DisplayName("Stock Limit")]
        public string StockLimit { get; set; }

        [DisplayName("Opening Stock")]
        public string OpeningStock { get; set; }

        [DisplayName("Warranty")]
        public string Warranty { get; set; }
    }
}