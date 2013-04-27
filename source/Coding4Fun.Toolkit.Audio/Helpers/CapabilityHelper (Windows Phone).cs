using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Coding4Fun.Toolkit.Audio.Helpers
{
	// based off of http://www.nickharris.net/2011/01/check-if-a-capability-is-enabled-in-wmappmanifest-on-windows-phone-7/
	internal static class CapabilityHelper
	{
		const string AppManifestName = "WMAppManifest.xml";

		private const string CapabilitiesXmlNode = "Capabilities";
		private const string NameXmlAttribute = "Name";

		private const string IdCapNetworking = "ID_CAP_NETWORKING";
		private const string IdCapIdentityDevice = "ID_CAP_IDENTITY_DEVICE";
		private const string IdCapIdentityUser = "ID_CAP_IDENTITY_USER";
		private const string IdCapLocation = "ID_CAP_LOCATION";
		private const string IdCapSensors = "ID_CAP_SENSORS";
		private const string IdCapMicrophone = "ID_CAP_MICROPHONE";
		private const string IdCapMedialib = "ID_CAP_MEDIALIB";
		private const string IdCapGamerservices = "ID_CAP_GAMERSERVICES";
		private const string IdCapPhonedialer = "ID_CAP_PHONEDIALER";
		private const string IdCapPushNotification = "ID_CAP_PUSH_NOTIFICATION";
		private const string IdCapWebbrowsercomponent = "ID_CAP_WEBBROWSERCOMPONENT";

		static CapabilityHelper()
		{
			var settings = new XmlReaderSettings { XmlResolver = new XmlXapResolver() };

			using (var rdr = XmlReader.Create(AppManifestName, settings))
			{
				var xml = XElement.Load(rdr);
				var capabilities = xml.Descendants(CapabilitiesXmlNode).Elements().ToList();

				IsNetworkingCapability = CheckCapability(capabilities, IdCapNetworking);
				IsDeviceIdentityCapability = CheckCapability(capabilities, IdCapIdentityDevice);
				IsUserIdentityCapability = CheckCapability(capabilities, IdCapIdentityUser);
				IsLocationCapability = CheckCapability(capabilities, IdCapLocation);
				IsSensorsCapability = CheckCapability(capabilities, IdCapSensors);
				IsMicrophoneCapability = CheckCapability(capabilities, IdCapMicrophone);
				IsMediaLibCapability = CheckCapability(capabilities, IdCapMedialib);
				IsGamerServicesCapability = CheckCapability(capabilities, IdCapGamerservices);
				IsPhoneDialerCapability = CheckCapability(capabilities, IdCapPhonedialer);
				IsPushNotificationCapability = CheckCapability(capabilities, IdCapPushNotification);
				IsWebBrowserComponentCapability = CheckCapability(capabilities, IdCapWebbrowsercomponent);
			}
		}

		public static bool IsNetworkingCapability { get; set; }
		public static bool IsDeviceIdentityCapability { get; set; }
		public static bool IsUserIdentityCapability { get; set; }
		public static bool IsLocationCapability { get; set; }
		public static bool IsSensorsCapability { get; set; }
		public static bool IsMicrophoneCapability { get; set; }
		public static bool IsMediaLibCapability { get; set; }
		public static bool IsGamerServicesCapability { get; set; }
		public static bool IsPhoneDialerCapability { get; set; }
		public static bool IsPushNotificationCapability { get; set; }
		public static bool IsWebBrowserComponentCapability { get; set; }

		private static bool CheckCapability(IEnumerable<XElement> capabilities, string capabilityName)
		{
			var capability = capabilities.FirstOrDefault(n =>
				{
					var attr = n.Attribute(NameXmlAttribute);
					return attr != null && attr.Value.Equals(capabilityName);
				});

			return capability != null;
		}
	}

}
