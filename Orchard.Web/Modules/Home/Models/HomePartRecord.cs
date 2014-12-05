using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Home.Models
{
    public class HomePartRecord:ContentPartRecord{


        public virtual string name { get; set; }
        public virtual string tooltip { get; set; }
        public virtual string linkref { get; set; }
        public virtual string pichref { get; set; }

    }
    
    
}