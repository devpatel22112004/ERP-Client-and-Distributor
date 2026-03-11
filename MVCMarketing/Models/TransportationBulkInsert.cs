using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class TransportationBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Transport Name")]
        public string Transport { get; set; }

        [DisplayName("Route Name")]
        public string RouteName { get; set; }

        [DisplayName("Vehicle Number")]
        public string VehicleNumber { get; set; }

        [DisplayName("Transportation Type")]
        public string TransportType { get; set; }

        [DisplayName("Transport Name 1")]
        public string PersonName1 { get; set; }

        [DisplayName("Contact 1")]
        public string Contact1 { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("City")]
        public string CityName { get; set; }

        [DisplayName("GST Number/ ID")]
        public string GSTNumber { get; set; }

        [DisplayName("Transport Name 2")]
        public string PersonName2 { get; set; }

        [DisplayName("Contact 2")]
        public string Contact2 { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}