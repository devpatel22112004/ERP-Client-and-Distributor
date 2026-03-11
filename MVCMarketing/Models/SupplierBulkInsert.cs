using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class SupplierBulkInsert
    {
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Pincode")]
        public string Pincode { get; set; }

        [DisplayName("Person Name1")]
        public string PersonName1 { get; set; }

        [DisplayName("Contact 1")]
        public string Contact1 { get; set; }

        [DisplayName("Contact 2")]
        public string Contact2 { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("GST")]
        public string GST { get; set; }

        [DisplayName("PAN")]
        public string PAN { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}