using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

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

		private readonly Regex _nameSpaceRegEx = new Regex(String.Format("{0}{2}|{1}{2}", Constants.UsingNamespace, Constants.ClrNamespace, Constants.Colon));

		private readonly SystemTarget _target;
		private readonly string _rootFolderPath;
        private string _currentFile;

		public Merger(SystemTarget target, bool isTestMode)
		{
			_target = target;

			_rootFolderPath = !isTestMode ? 
				FilePaths.GenerateGenericFilePath(_target) :
				FilePaths.GetExecutingAssemblyFilePath();
		}

	    public string GenerateGenericXamlFile()
	    {
            var nameSpace = "";
            switch (_target)
            {
                case SystemTarget.WindowsPhone7:
                case SystemTarget.WindowsPhone8:
                    nameSpace = Constants.ClrNamespace;
                    break;
                case SystemTarget.WindowsStore:
                    nameSpace = Constants.UsingNamespace;
                    break;
            }

	        var nameSpaces = new List<XAttribute>();
	        XNamespace defaultNameSpace = "";

	        foreach (var ns in _nameSpaces)
	        {
	            var key = ns.Key;

	            if (key.StartsWith(Constants.Xmlns))
	            {
	                key = key.TrimStart(Constants.Xmlns).TrimStart(Constants.Colon);
	            }

	            var value = ns.Value.StartsWith(Constants.Http, StringComparison.InvariantCultureIgnoreCase) ? ns.Value :  string.Format("{0}{1}{2}", nameSpace, Constants.Colon, ns.Value);

	            if (string.IsNullOrEmpty(key))
	            {
                    defaultNameSpace = value;
                    nameSpaces.Add(new XAttribute(Constants.Xmlns, value));
	            }
	            else
	            {
                    nameSpaces.Add(new XAttribute(XNamespace.Xmlns + key, value));
	            }
	        }
	        
            var root = new XElement(defaultNameSpace + Constants.ResourceDictionaryNode);
	        foreach (var ns in nameSpaces)
	        {
	            root.Add(ns);
	        }

            return new XDocument(root).ToString();
	    }

	    public bool ProcessXamlFiles()
		{
			var di = new DirectoryInfo(_rootFolderPath);
			var files = new List<FileInfo>(di.GetFiles("*.xaml", SearchOption.AllDirectories).AsEnumerable());

            // purging non-platform files
            switch (_target)
            {
                case SystemTarget.WindowsPhone7:
                    UpdateFileList(files, Constants.WindowsStoreEndFileName);
                    UpdateFileList(files, Constants.WindowsPhone8EndFileName);
                    break;
                case SystemTarget.WindowsPhone8:
                    UpdateFileList(files, Constants.WindowsStoreEndFileName);
                    UpdateFileList(files, Constants.WindowsPhone7EndFileName);
                    break;
                case SystemTarget.WindowsStore:
                    UpdateFileList(files, Constants.WindowsPhoneEndFileName);
                    UpdateFileList(files, Constants.WindowsPhone7EndFileName);
                    UpdateFileList(files, Constants.WindowsPhone8EndFileName);
                    break;
            }

            var success = true;
            success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleThemeXaml);

            switch (_target)
            {
                case SystemTarget.WindowsPhone7:
                    success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleWinPhoneThemeXaml);
                    success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleWinPhone7ThemeXaml);
                    break;
                case SystemTarget.WindowsPhone8:
                    success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleWinPhoneThemeXaml);
                    success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleWinPhone8ThemeXaml);
                    break;
                case SystemTarget.WindowsStore:
                    success &= ProcessAndUpdateFileListForCommonStyle(files, Constants.CommonStyleWinStoreThemeXaml);
                    break;
            }

		    success &= files.Aggregate(true, (current, file) => current & ProcessFile(file.FullName));

		    return success;
		}

	    private bool ProcessAndUpdateFileListForCommonStyle(List<FileInfo> files, string targetFile)
	    {
            var success = true;

            var commonStyles = files.FirstOrDefault(fi => fi.Name.ToLower() == targetFile);

	        if (commonStyles != null)
	        {
	            success &= ProcessFile(commonStyles.FullName);

	            files.Remove(commonStyles);
	        }

	        return success;
	    }

        private void UpdateFileList(List<FileInfo> files, string targetToRemove)
        {
            files.RemoveAll(file => files.All(fi => file.Name.ToLower().EndsWith(targetToRemove, true, null)));
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
			    if (node.NodeType != XmlNodeType.Element)
			        continue;

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
	        return root.ChildNodes.Cast<XmlNode>().Aggregate(true, (current, node) => current & ProcessResources(node, store, isGenericFile));
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
            var hasWinPhoneStyle = node.OuterXml.Contains(Constants.WinPhoneOnlyResource);
            var hasWinStoreStyle = node.OuterXml.Contains(Constants.WinStoreOnlyResource);

            if (isGenericFile)
            {
                if (hasWinPhoneStyle)
                {
                    HasTargetedStyleError(node, "Phone");

                    return true;
                }

                if(hasWinStoreStyle)
                {
                     HasTargetedStyleError(node, "Store");

                    return true;
                }

            }
            else
            {
                if (_target == SystemTarget.WindowsStore)
                {
                    if (hasWinPhoneStyle)
                    {
                        HasTargetedStyleError(node, "Phone");

                        return true;
                    }
                }
                else // can assume WinPhone at this point
                {
                    if (hasWinStoreStyle)
                    {
                        HasTargetedStyleError(node, "Store");

                        return true;
                    }
                }
            }

            return false;
        }

	    private void HasTargetedStyleError(XmlNode node, string platform)
	    {
	        var key = "";
            
	        try
	        {
	            key = node.Attributes[Constants.KeyAttribute].Value;
	        }
	        catch
	        {
	        }

	        if (string.IsNullOrEmpty(key))
	        {
	            try
	            {
	                key = node.Attributes[Constants.TargetTypeAttribute].Value;
	            }
	            catch
	            {
	            }
	        }

	        WriteError(string.Format("{0} only static resource.  Key: {1}", platform, key));
	    }

	    private void WriteError(string error)
		{
			Console.WriteLine(
				_currentFile + Environment.NewLine +
				error + Environment.NewLine);
		}

	    private static string GetFileTypeByTarget(SystemTarget target)
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