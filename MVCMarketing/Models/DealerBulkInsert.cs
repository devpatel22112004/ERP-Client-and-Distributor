
using System.ComponentModel;

namespace MVCMarketing.Models
{

    public class DealerBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Dealer Name")]
        public string Name { get; set; }

        [DisplayName("Area")]
        public string Area { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("City")]
        public string CityName { get; set; }

        [DisplayName("State")]
        public string StateName { get; set; }

        [DisplayName("Pin code")]
        public string Pincode { get; set; }

        [DisplayName("Person")]
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
        public float PAN { get; set; }

        [DisplayName("Tax Mode")]
        public string TaxMode { get; set; }

        [DisplayName("Credit Limit Amount")]
        public string CreditLimitAmount { get; set; }

        [DisplayName("Due Days")]
        public string DueDays { get; set; }

        [DisplayName("Opening Balance")]
        public float OpeningBalance { get; set; }
    }
}