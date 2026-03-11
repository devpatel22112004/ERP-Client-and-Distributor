using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class OrderReturnBulkInsert
    {
        [DisplayName("Row Index")]

        public string RowIndex { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Distributor")]
        public string Distributor { get; set; }

        [DisplayName("Return Number")]
        public DateTime ReturnNumber { get; set; }

        [DisplayName("Itemcode")]
        public string ItemCode { get; set; }

        [DisplayName("GST")]
        public string GST { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Quantity")]
        public string Quantity { get; set; }

        [DisplayName("Rate")]
        public string Rate { get; set; }

        [DisplayName("Discount 1")]
        public string Discount1 { get; set; }

        [DisplayName("Discount 2")]
        public string Discount2 { get; set; }

        [DisplayName("Discount 3")]
        public string Discount3 { get; set; }

        [DisplayName("Total(Exc. GST)")]
        public string Total { get; set; }

        [DisplayName("Final(Inc. GST)")]
        public string TotalAmount { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

    }
}