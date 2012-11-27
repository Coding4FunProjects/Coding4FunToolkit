using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public class Merger
	{
		private readonly Dictionary<string, string> _nameSpaces = new Dictionary<string, string>();
		private readonly Dictionary<string, XmlNode> _styles = new Dictionary<string, XmlNode>();

        // if phone, we suck in default values.
		private readonly Dictionary<string, XmlNode> _resources = new Dictionary<string, XmlNode>();
		
		private readonly Dictionary<string, XmlNode> _resourcesWinStoreDefault = new Dictionary<string, XmlNode>();
		private readonly Dictionary<string, XmlNode> _resourcesWinStoreLight = new Dictionary<string, XmlNode>();
		private readonly Dictionary<string, XmlNode> _resourcesWinStoreHighContrast = new Dictionary<string, XmlNode>();

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
			
			_currentFile = fileName;

		    var isGenericFile = !fileName.EndsWith(GetFileTypeByTarget(_target), true, null);

            if(_target == SystemTarget.WindowsPhone7 || _target == SystemTarget.WindowsPhone8)
                isGenericFile &= !fileName.EndsWith(GetFileTypeByTarget(SystemTarget.WindowsPhone), true, null);

			doc.LoadXml(File.ReadAllText(fileName));

			var rootNode = doc.FirstChild;

            success &= ProcessNameSpaces(rootNode);

			foreach (XmlNode node in rootNode.ChildNodes)
			{
				switch (node.Name)
				{
					case Constants.StyleNode:
						success &= ProcessStyle(node, isGenericFile);
						break;
					case Constants.ThemeDictionariesNode:
                        success &= ProcessThemedDictionary(node, isGenericFile);
						break;
					default:
                        success &= ProcessResources(node, _resources, isGenericFile);
						break;
				}
			}

			return success;
		}

		private bool ProcessNameSpaces(XmlNode node)
		{
			if (node.Attributes == null)
				return false;

			foreach (XmlAttribute attribute in node.Attributes)
			{
				var key = attribute.Name;
				var value = attribute.Value;

				value = _nameSpaceRegEx.Replace(value, String.Empty);

                if (_nameSpaces.ContainsKey(key))
				{
                    if (_nameSpaces[key] != value)
					{
						WriteError("NameSpaces do not match" + Environment.NewLine +
							"file key: " + key + " :: file value: " + value + Environment.NewLine +
                            "sys key: " + key + " :: sys value: " + _nameSpaces[key]);

						return false;
					}
				}
				else
				{
                    if (_nameSpaces.ContainsValue(value))
					{
						WriteError("Contains Name Space but not Key" + Environment.NewLine +
							"file key: " + key + " :: file value: " + value + Environment.NewLine +
                            "sys key: " + key + " :: sys value: " + _nameSpaces[key]);

						return false;
					}

                    _nameSpaces.Add(key, value);
				}
			}

			return true;
		}

        private bool ProcessThemedDictionary(XmlNode root, bool isGenericFile)
	    {
            var success = true;

            var isWinStore = (_target == SystemTarget.WindowsStore);

            foreach (XmlNode node in root.ChildNodes)
            {
                switch (node.Attributes[Constants.KeyAttribute].Value)
                {
                    case Constants.DefaultTheme:
                        var store = isWinStore ? _resourcesWinStoreDefault : _resources;

                        success &= ProcessThemedResources(node, store, isGenericFile);
                        break;
                     case Constants.LightTheme:
                        if (_target != SystemTarget.WindowsStore)
                            continue;

                        success &= ProcessThemedResources(node, _resourcesWinStoreLight, isGenericFile);
                        break;
                    
                    case Constants.HighContrastTheme:
                        if (_target != SystemTarget.WindowsStore)
                            continue;

                        success &= ProcessThemedResources(node, _resourcesWinStoreHighContrast, isGenericFile);
                        break;
                   
                }
            }

            if (isWinStore)
            {
                if (_resourcesWinStoreDefault.Count != _resourcesWinStoreLight.Count ||
                    _resourcesWinStoreDefault.Count != _resourcesWinStoreHighContrast.Count)
                {
                    WriteError("Themed resources have a different quantity");

                    success = false;
                }
            }

            return success;
	    }

	    private bool ProcessThemedResources(XmlNode root, Dictionary<string, XmlNode> store, bool isGenericFile)
	    {
            var success = true;

	        foreach (XmlNode node in root.ChildNodes)
	        {
                success &= ProcessResources(node, store, isGenericFile);
	        }

	        return success;
	    }

	    private bool ProcessResources(XmlNode node, Dictionary<string, XmlNode> store, bool isGenericFile)
		{
            if (VerifyIsGeneric(node, isGenericFile))
                return false;

		    var key = node.Attributes[Constants.KeyAttribute].Value;

            return AddToDictionary(store, key, node);
		}

	    private bool ProcessStyle(XmlNode node, bool isGenericFile)
		{
            if (VerifyIsGeneric(node, isGenericFile))
                return false;

			var key = node.Attributes[Constants.TargetTypeAttribute].Value;

            return AddToDictionary(_styles, key, node);
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

        private bool VerifyIsGeneric(XmlNode node, bool isGenericFile)
        {
            if (isGenericFile && node.OuterXml.Contains(Constants.PhoneOnlyResource))
            {

                var key = "";

                try
                {
                    key = node.Attributes[Constants.KeyAttribute].Value;
                }
                catch { }

                if (string.IsNullOrEmpty(key))
                {
                    try
                    {
                        key = node.Attributes[Constants.TargetTypeAttribute].Value;
                    }
                    catch { }
                }
                
                WriteError("Phone only static resource.  Key: " + key);

                return true;
            }

            return false;
        }

		private void WriteError(string error)
		{
			Console.WriteLine(
				_currentFile + Environment.NewLine +
				error + Environment.NewLine);
		}

	    private string GetFileTypeByTarget(SystemTarget target)
	    {
	        switch (target)
	        {
                case SystemTarget.WindowsPhone:
	                return Constants.WindowsPhoneEndFileName;
                case SystemTarget.WindowsPhone7:
                    return Constants.WindowsPhone7EndFileName;
                case SystemTarget.WindowsPhone8:
                    return Constants.WindowsPhone8EndFileName;
                case SystemTarget.WindowsStore:
	                return Constants.WindowsStoreEndFileName;
	        }

	        throw new ArgumentOutOfRangeException("target", target, "cannot find target");
	    }
	}
}