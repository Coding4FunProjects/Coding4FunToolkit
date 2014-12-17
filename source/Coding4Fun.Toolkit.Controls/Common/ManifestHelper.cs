using System;
using System.Xml;

namespace Coding4Fun.Toolkit.Controls.Common
{
    public class ManifestHelper
    {
        const string AppManifestName = "AppxManifest.xml";
        const string DisplayNameNode = "DisplayName";
        const string IdentityNode = "Identity";
        const string VersionAttr = "Version";

        public static string GetDisplayName()
        {
			if (ApplicationSpace.IsDesignMode)
				return "";

            try
            {
                var settings = new XmlReaderSettings { };

                using (var rdr = XmlReader.Create(AppManifestName, settings))
                {
                    rdr.ReadToDescendant(DisplayNameNode);
                    if (!rdr.IsStartElement())
                    {
                        throw new FormatException(AppManifestName + " is missing " + DisplayNameNode);
                    }

                    return rdr.ReadInnerXml();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetVersion()
        {
            if (ApplicationSpace.IsDesignMode)
                return "";

            try
            {
                var settings = new XmlReaderSettings { };

                using (var rdr = XmlReader.Create(AppManifestName, settings))
                {
                    rdr.ReadToDescendant(IdentityNode);
                    if (!rdr.IsStartElement())
                    {
                        throw new FormatException(AppManifestName + " is missing " + DisplayNameNode);
                    }

                    return rdr.GetAttribute(VersionAttr);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
