using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class JsonProperty
    {
        public string isStatus { get; set; }
        public string message { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<Array> list { get; set; }
    }

}