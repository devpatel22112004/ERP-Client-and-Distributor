using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMarketing.Models
{
    public class ParameterDisplayAttribute : Attribute
    {
        public string Display { get; set; }
        public string Header { get; set; }
    }
}