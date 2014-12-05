using System;
using Orchard.DisplayManagement.Implementation;

namespace NogginBox.MenuItemLinkAlternates
{
	public class MenuItemLinkAlternatesFactory : ShapeDisplayEvents
	{
		public override void Displaying(ShapeDisplayingContext context) {
			context.ShapeMetadata.OnDisplaying(displayedContext => {
				String zoneName;
				switch (displayedContext.ShapeMetadata.Type) {
					case "Parts_MenuWidget":
						zoneName = displayedContext.Shape.ContentItem.WidgetPart.Zone;
						displayedContext.Shape.Menu.Zone = zoneName;
						break;
					case "MenuItemLink":
						zoneName = displayedContext.Shape.Parent.Zone;
						const String shapeName = "MenuItemLink";
						//var contentTypeName = "Notsureyet";//contentItem.ContentType;
						displayedContext.ShapeMetadata.Alternates.Add(shapeName + "__" + zoneName);
						//displayedContext.ShapeMetadata.Alternates.Add(shapeName + "__" + contentTypeName + "__" + zoneName);
						break;
				}
			});
		}
	}
}