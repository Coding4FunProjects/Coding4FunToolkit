using System.Xml;
using System.Xml.Linq;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public static class XmlExtentions
	{
		public static XElement ToXElement(this XmlNode node)
		{
			var xDoc = new XDocument();

			using (var xmlWriter = xDoc.CreateWriter())
			{
				node.WriteTo(xmlWriter);
			}

			return xDoc.Root;
		}

		public static XmlNode ToXmlNode(this XElement element)
		{
			var xmlDoc = new XmlDocument();

			using (var xmlReader = element.CreateReader())
			{
				xmlDoc.Load(xmlReader);
			}

			return xmlDoc;
		}
	}
}