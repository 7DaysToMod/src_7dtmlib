using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace SDTM
{
	public class ModDataXML
	{
		private string dataName;

		private Dictionary<string, ModDataProperty> properties = new Dictionary<string, ModDataProperty> ();

		public ModDataXML (string _dataName)
		{
			dataName = _dataName;
		}

		public bool hasKey(string keyName){
			return properties.ContainsKey (keyName);
		}

		public string GetValue(string Propertyname){
			if (properties.ContainsKey (Propertyname)) {
				return properties [Propertyname].value;
			}

			return "";
		}

		public string GetParam1(string Propertyname){
			if (properties.ContainsKey (Propertyname)) {
				return properties [Propertyname].param1;
			}

			return "";
		}

		public string GetParam2(string Propertyname){
			if (properties.ContainsKey (Propertyname)) {
				return properties [Propertyname].param2;
			}

			return "";
		}

		public void SetValue(string propertyName, string propertyValue){
			if (properties.ContainsKey (propertyName)) {
				ModDataProperty prop = properties [propertyName];
				prop.value = propertyValue;
				properties [propertyName] = prop;
			} else {
				ModDataProperty prop = new ModDataProperty();
				prop.name = propertyName;
				prop.value = propertyValue;
				prop.param1 = "";
				prop.param2 = "";
				properties.Add (propertyName, prop);
			}
		}

		public void SetParam1(string propertyName, string paramValue){
			if (properties.ContainsKey (propertyName)) {
				ModDataProperty prop = properties [propertyName];
				prop.param1 = paramValue;

				properties [propertyName] = prop;
			} else {
				ModDataProperty prop = new ModDataProperty();
				prop.name = propertyName;
				prop.param1 = paramValue;
				prop.param2 = "";
				prop.value = "";
				properties.Add (propertyName, prop);
			}
		}

		public void SetParam2(string propertyName, string paramValue){
			if (properties.ContainsKey (propertyName)) {
				ModDataProperty prop = properties [propertyName];
				prop.param2 = paramValue;
				properties [propertyName] = prop;
			} else {
				ModDataProperty prop = new ModDataProperty();
				prop.name = propertyName;
				prop.param2 = paramValue;
				prop.param1 = "";
				prop.value = "";
				properties.Add (propertyName, prop);
			}
		}

		public void Remove(string propertyName){
			if(properties.ContainsKey(propertyName)){
				properties.Remove (propertyName);
			}
		}

		public List<string> Keys{
			get{
				List<string> keys = new List<string> ();
				foreach (string key in properties.Keys) {
					keys.Add (key);
				}
				return keys;
			}
		}

		public bool Load(){
			string modDataPath = SDTM.API.configDataPath;
			string[] splitter = { "/" };
			string[] parts = dataName.Split (splitter, StringSplitOptions.RemoveEmptyEntries);
			string dataFolder = modDataPath;

			if (parts.Length > 1) {
				dataFolder += "/" + parts [0];
			}

			if (FileUtils.ensureDirectoryExists (dataFolder)) {
				modDataPath = modDataPath + "/" + dataName + ".xml";

				if (!File.Exists (modDataPath)) {
					if (!Save ()) {
						Log.Error(string.Format("[SDTMLib] Could not Create Initial Data: {0}", modDataPath));
						return false;
					}
				}

				XmlDocument xmlDoc = new XmlDocument();
				try{
					xmlDoc.Load(modDataPath);
				}
				catch (XmlException e){
					Log.Error(string.Format("[SDTMLib] Could not Load Config: {0}", e.Message));
					return false;
				}

				XmlNode rootNode = xmlDoc.DocumentElement;

				foreach (XmlNode configNode in rootNode.ChildNodes) {
					ModDataProperty prop = new ModDataProperty ();
					prop.name = configNode.Name.ToLower ();
					prop.value = configNode.InnerText;

					if (configNode.Attributes.Count > 0) {
						foreach(XmlAttribute attr in configNode.Attributes){
							switch (attr.Name.ToLower ()) {
							case "param1":
								prop.param1 = attr.Value;
								break;
							case "param2":
								prop.param2 = attr.Value;
								break;
							}
						}
					}
					properties.Add (configNode.Name.ToLower (), prop);
				}

				return true;
			}

			return false;
		}

		public bool Save(){
			string modDataPath = SDTM.API.configDataPath;
			string[] splitter = { "/" };
			string[] parts = dataName.Split (splitter, StringSplitOptions.RemoveEmptyEntries);
			string dataFolder = modDataPath;

			if (parts.Length > 1) {
				dataFolder += "/" + parts [0];
			}

			if (FileUtils.ensureDirectoryExists (dataFolder)) {
				modDataPath = modDataPath + "/" + dataName + ".xml";
				StreamWriter sw = new StreamWriter (modDataPath);

				sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				sw.WriteLine("<ModData>");

				foreach (string key in properties.Keys) {
					ModDataProperty prop = properties[key];
					string entry = string.Format ("<{0} param1=\"{1}\" param2=\"{2}\">{3}</{0}>", prop.name, prop.param1, prop.param2, prop.value);
					sw.WriteLine ("\t"+entry);
				}

				sw.WriteLine("</ModData>");
				sw.Flush();
				sw.Close();

				return true;
			}

			return false;
		}

	}
}

