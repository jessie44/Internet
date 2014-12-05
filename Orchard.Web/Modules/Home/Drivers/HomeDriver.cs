using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Home.Models;

namespace Home.Drivers
{
    public class HomeDriver : ContentPartDriver<HomePart>

    {
        protected override DriverResult Display(HomePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Home_Product", () => shapeHelper.Home_Product(Name: part.Name, LinkHref: part.LinkHref, PicHref: part.PicHref, Tooltip: part.Tooltip));
        }

        protected override DriverResult Editor(HomePart part, dynamic shapeHelper)
        {
            return ContentShape("Home_Product_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Product", Model: part, Prefix: Prefix));
        }
        protected override DriverResult Editor(HomePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}