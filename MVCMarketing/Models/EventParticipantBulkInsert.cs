
using System.ComponentModel;

namespace MVCMarketing.Models
{

    public class EventParticipantBulkInsert
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Event Title")]
        public string Groupname { get; set; }

        [DisplayName("Date")]
        public string Date { get; set; }

        [DisplayName("Time")]
        public string Time { get; set; }

        [DisplayName("Venue")]
        public string Venue { get; set; }

        [DisplayName("Place")]
        public string Place { get; set; }

        [DisplayName("Person Name")]
        public string PersonName { get; set; }

        [DisplayName("Contact (Whatsapp No)")]
        public string Contact { get; set; }

        [DisplayName("Firm")]
        public string Name { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Area")]
        public string Area { get; set; }

        [DisplayName("Pincode")]
        public string Pincode { get; set; }

        [DisplayName("State")]
        public string StateName { get; set; }

        [DisplayName("City")]
        public string CityName { get; set; }

        [DisplayName("Email")]
        public float Email { get; set; }
    }
}