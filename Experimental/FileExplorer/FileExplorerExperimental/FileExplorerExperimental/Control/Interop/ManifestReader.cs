using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace FileExplorerExperimental.Control.Interop
{
    public static class ManifestReader
    {
        public static XDocument GetAppManifest()
        {
            return XDocument.Load("WMAppManifest.xml");
        }

        public static List<string> GetRegisteredExtensions()
        {
            List<string> returnable = new List<string>();

            XDocument appManifest = GetAppManifest();

            try
            {
                XElement element = appManifest.Root.Element("App").Element("Extensions").Element("FileTypeAssociation").Element("SupportedFileTypes");
                foreach (XElement extensionElement in element.Elements())
                {
                    returnable.Add(extensionElement.Value);
                }
            }
            catch
            {
                // Apparently there is no extension node.
            }

            return returnable;
        }
    }
}
