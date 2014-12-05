using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Home.Models
{
    public class HomePart : ContentPart<HomePartRecord>
    {
        [Required]
        public string Name
        {
            get { return Record.name; }
            set { Record.name = value; }
        }
        [Required]
        public string Tooltip
        {
            get { return Record.tooltip; }
            set { Record.tooltip = value; }
        }
        [Required]
        public string LinkHref
        {
            get { return Record.linkref; }
            set { Record.linkref = value; }
        }
        [Required]
        public string PicHref
        {
            get { return Record.pichref; }
            set { Record.pichref = value; }
        }
    }
}