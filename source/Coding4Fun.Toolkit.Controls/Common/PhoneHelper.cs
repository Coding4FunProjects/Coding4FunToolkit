using System;
using System.Xml;

namespace Coding4Fun.Toolkit.Controls.Common
{
    public class PhoneHelper
    {
        const string AppManifestName = "WMAppManifest.xml";
        const string AppNodeName = "App";

        /// <summary>
        /// Gets the value from the WMAppManifest in runtime
        /// Example: PhoneHelper.GetAppAttribute("Title");
        /// 
        /// http://stackoverflow.com/questions/3411377/get-the-windows-phone-7-application-title-from-code
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAppAttribute(string attributeName)
        {
			if (DevelopmentHelpers.IsDesignMode)
				return "";

            try
            {
                var settings = new XmlReaderSettings { XmlResolver = new XmlXapResolver() };

                using (var rdr = XmlReader.Create(AppManifestName, settings))
                {
                    rdr.ReadToDescendant(AppNodeName);
                    if (!rdr.IsStartElement())
                    {
                        throw new FormatException(AppManifestName + " is missing " + AppNodeName);
                    }

                    return rdr.GetAttribute(attributeName);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
