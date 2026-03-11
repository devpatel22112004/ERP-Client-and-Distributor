using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class InwardDirectBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Vendor Code")]
        public string VendorCode { get; set; }

        [DisplayName("Inward Date")]
        public DateTime LRDate { get; set; }

        [DisplayName("Inward No")]
        public string LRNumber { get; set; }

        [DisplayName("Challan")]
        public string Challan { get; set; }

        [DisplayName("Invoice Payment Date")]
        public DateTime InvoicePaymentDate { get; set; }

        [DisplayName("Transporter")]
        public string Transporter { get; set; }

        [DisplayName("ContactNo")]
        public string ContactNo { get; set; }

        [DisplayName("Freight")]
        public string Freight { get; set; }

        [DisplayName("Track Number")]
        public string TrackNumber { get; set; }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        [DisplayName("Manufacturer Date")]
        public string ManufacturerDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Qty")]
        public string Qty { get; set; }

        [DisplayName("Rate")]
        public string Rate { get; set; }

        [DisplayName("Discount 1")]
        public string Discount1 { get; set; }

        [DisplayName("Discount 2")]
        public string Discount2 { get; set; }

        [DisplayName("Discount 3")]
        public string Discount3 { get; set; }

        [DisplayName("Total")]
        public string Total { get; set; }

        [DisplayName("GST")]
        public string GST { get; set; }

        [DisplayName("Total Amount")]
        public string TotalAmount { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Serial No")]
        public string SerialNo { get; set; }
    }
}