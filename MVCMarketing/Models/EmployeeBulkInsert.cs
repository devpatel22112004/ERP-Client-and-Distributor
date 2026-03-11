using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class loyeeBulkInsertModel
    {
        [DisplayName("Row Index")]
        public string RowIndex { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Address Permit")]
        public string AddressPermit { get; set; }

        [DisplayName("Address Current")]
        public string AddressCurrent { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Phone Other")]
        public string PhoneOther { get; set; }

        [DisplayName(" Email")]
        public string Email { get; set; }

        [DisplayName(" Education")]
        public string Education { get; set; }

        [DisplayName("Phone Company")]
        public string PhoneCompany { get; set; }

        [DisplayName("Email Company")]
        public string EmailCompany { get; set; }

        [DisplayName(" User +3.Id")]
        public string UserId { get; set; }

        [DisplayName(" Password")]
        public string Password { get; set; }

        [DisplayName(" Designation")]
        public string Designation { get; set; }

        [DisplayName(" Status")]
        public string Status { get; set; }

        [DisplayName("DOB")]
        public string DOB { get; set; }

        [DisplayName("Joining Date")]
        public string JoiningDate { get; set; }

        [DisplayName(" Signature")]
        public string Signature { get; set; }

        [DisplayName("Leaving Date")]
        public string LeavingDate { get; set; }

        [DisplayName("Tag")]
        public string Tag { get; set; }
    }
}