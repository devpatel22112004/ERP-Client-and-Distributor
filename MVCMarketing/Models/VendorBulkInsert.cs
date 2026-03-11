using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
     public class VendorBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Vendor Code")]
        public string VendorCode { get; set; }

        [DisplayName("Vendor")]
        public string Name { get; set; }

        [DisplayName("Area")]
        public string Area { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Pincode")]
        public string Pincode { get; set; }

        [DisplayName("Person")]
        public string PersonName1 { get; set; }

        [DisplayName("Contact 1")]
        public string Contact1 { get; set; }

        [DisplayName("Contact 2")]
        public string Contact2 { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("GST Class")]
        public string GSTClass { get; set; }

        [DisplayName("GST")]
        public string GST { get; set; }

        [DisplayName("PAN")]
        public string PAN { get; set; }

        [DisplayName("Bank AC No")]
        public string BankACNo { get; set; }

        [DisplayName("Bank IFSC")]
        public string BankIFSC { get; set; }

        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }

        [DisplayName("Due Days")]
        public string DueDays { get; set; }

        [DisplayName("Credit Limit Amount")]
        public string CreditLimitAmount { get; set; }

        [DisplayName("Opening Balance")]
        public string OpeningBalance { get; set; }
    }
}