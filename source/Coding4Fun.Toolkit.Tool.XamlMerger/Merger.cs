using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public class Merger
	{
		private Dictionary<string, string> _nameSpaceBase = new Dictionary<string, string>();
		private Dictionary<string, string> _nameSpaceWinStore = new Dictionary<string, string>();
		private Dictionary<string, string> _nameSpaceWinPhone7 = new Dictionary<string, string>();
		private Dictionary<string, string> _nameSpaceWinPhone8 = new Dictionary<string, string>();

		private Dictionary<string, XmlNode> _stylesWinStore = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _stylesWinPhone7 = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _stylesWinPhone8 = new Dictionary<string, XmlNode>();

		private Dictionary<string, XmlNode> _resourcesWinStore = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _resourcesWinPhone7 = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _resourcesWinPhone8 = new Dictionary<string, XmlNode>();

		private Dictionary<string, XmlNode> _resourcesWinStoreDefault = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _resourcesWinStoreLight = new Dictionary<string, XmlNode>();
		private Dictionary<string, XmlNode> _resourcesWinStoreHighContrast = new Dictionary<string, XmlNode>();

		private readonly Regex _nameSpaceRegEx =
			new Regex(String.Format("{0}{2}|{1}{2}", Constants.UsingNamespace, Constants.ClrNamespace, Constants.Colon));

		private readonly SystemTarget _target;
		private string _currentFile;

		public Merger(SystemTarget target)
		{
			_target = target;
		}

		public bool ProcessPath()
		{
			return true;
		}

		public bool ProcessFile(string fileName)
		{
			var doc = new XmlDocument();
			var success = true;
			var isGenericXamlFile = false;
			var target = isGenericXamlFile ? SystemTarget.All : _target;

			_currentFile = fileName;

			doc.LoadXml(File.ReadAllText(fileName));

			var rootNode = doc.FirstChild;

			success &= ProcessNameSpaces(rootNode);

			foreach (XmlNode node in rootNode.ChildNodes)
			{
				switch (node.Name)
				{
					case Constants.StyleNode:
						success &= ProcessStyle(node, target);
						break;
					case Constants.ThemeDictionariesNodeType:
						// handle this properly.
						break;
					default:
						success &= ProcessResources(node, target);
						break;
				}
			}

			return success;
		}

		private bool ProcessNameSpaces(XmlNode node)
		{
			if (node.Attributes == null)
				return false;

			var nameSpaces = GetNameSpaceDictionary();

			foreach (XmlAttribute attribute in node.Attributes)
			{
				var key = attribute.Name;
				var value = attribute.Value;

				if (_target == SystemTarget.All)
				{
					value = _nameSpaceRegEx.Replace(value, String.Empty);
				}

				if (nameSpaces.ContainsKey(key))
				{
					if (nameSpaces[key] != value)
					{
						WriteError("NameSpaces do not match" + Environment.NewLine +
							"file key: " + key + " :: file value: " + value + Environment.NewLine +
							"sys key: " + key + " :: sys value: " + nameSpaces[key]);

						return false;
					}
				}
				else
				{
					if (nameSpaces.ContainsValue(value))
					{
						WriteError("Contains Name Space but not Key" + Environment.NewLine +
							"file key: " + key + " :: file value: " + value + Environment.NewLine +
							"sys key: " + key + " :: sys value: " + nameSpaces[key]);

						return false;
					}

					nameSpaces.Add(key, value);
				}
			}

			return true;
		}

		private Dictionary<string, string> GetNameSpaceDictionary()
		{
			switch (_target)
			{
				case SystemTarget.WindowsStore:
					return _nameSpaceWinStore;
				case SystemTarget.WindowsPhone7:
					return _nameSpaceWinPhone7;
				case SystemTarget.WindowsPhone8:
					return _nameSpaceWinPhone8;
				case SystemTarget.All:
					return _nameSpaceBase;
			}

			return null;
		}

		private bool ProcessResources(XmlNode node, SystemTarget target)
		{
			var key = node.Attributes[Constants.KeyAttribute].Value;

			return AddResources(target, key, node);
		}

		private bool AddResources(SystemTarget target, string key, XmlNode value)
		{
			var successful = true;

			switch (target)
			{
				case SystemTarget.WindowsStore:
					successful &= AddToDictionary(_resourcesWinStore, key, value);
					break;
				case SystemTarget.WindowsPhone7:
					successful &= AddToDictionary(_resourcesWinPhone7, key, value);
					break;
				case SystemTarget.WindowsPhone8:
					successful &= AddToDictionary(_resourcesWinPhone8, key, value);
					break;
				case SystemTarget.All:
					successful &= AddResources(SystemTarget.WindowsStore, key, value);
					successful &= AddResources(SystemTarget.WindowsPhone7, key, value);
					successful &= AddResources(SystemTarget.WindowsPhone8, key, value);
					break;
			}

			return successful;
		}

		private bool ProcessStyle(XmlNode node, SystemTarget target)
		{
			var key = node.Attributes[Constants.TargetTypeAttribute].Value;

			return AddStyle(target, key, node);
		}

		private bool AddStyle(SystemTarget target, string key, XmlNode value)
		{
			var successful = true;

			switch (target)
			{
				case SystemTarget.WindowsStore:
					successful &= AddToDictionary(_stylesWinStore, key, value);
					break;
				case SystemTarget.WindowsPhone7:
					successful &= AddToDictionary(_stylesWinPhone7, key, value);
					break;
				case SystemTarget.WindowsPhone8:
					successful &= AddToDictionary(_stylesWinPhone8, key, value);
					break;
				case SystemTarget.All:
					successful &= AddStyle(SystemTarget.WindowsStore, key, value);
					successful &= AddStyle(SystemTarget.WindowsPhone7, key, value);
					successful &= AddStyle(SystemTarget.WindowsPhone8, key, value);
					break;
			}

			return successful;
		}

		private bool AddToDictionary(IDictionary<string, XmlNode> target, string key, XmlNode value)
		{
			if (target.ContainsKey(key))
			{
				WriteError("contains style for key: " + key);

				return false;
			}

			target.Add(key, value);

			return true;
		}

		private void WriteError(string error)
		{
			Debug.WriteLine(
				_currentFile + Environment.NewLine +
				error + Environment.NewLine);
		}
	}
}