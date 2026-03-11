using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMarketing.Models
{
    public class PaymentBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Date")]
        public string Date { get; set; }

        [DisplayName("Bank")]
        public string BankName { get; set; }

        [DisplayName("Receipt No")]
        public string ReceiptNo { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Distributor")]
        public string Name { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Payment Mode")]
        public string PaymentMode { get; set; }

        [DisplayName("Amount")]
        public string Amount { get; set; }

        [DisplayName("Details")]
        public string Details { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Cheque No")]
        public string ChequeNo { get; set; }

        [DisplayName("Cheque Name")]
        public float ChequeName { get; set; }

        [DisplayName("Issue By")]
        public float IssueBy { get; set; }

    }
}