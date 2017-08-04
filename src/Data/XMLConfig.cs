using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace SDTM
{
	public class XMLConfig
	{
		private string configName;

		private Dictionary<string, string> properties = new Dictionary<string, string> ();

		public XMLConfig (string _configName)
		{
			configName = _configName;
		}

		public string Get(string Propertyname){
			if (properties.ContainsKey (Propertyname)) {
				return properties [Propertyname];
			}

			return "";
		}

		public void Set(string propertyName, string propertyValue){
			if (properties.ContainsKey (propertyName)) {
				properties [propertyName] = propertyValue;
			} else {
				properties.Add (propertyName, propertyValue);
			}
		}

		public void remove(string propertyName){
			if(properties.ContainsKey(propertyName)){
				properties.Remove (propertyName);
			}
		}

		public bool Load(){
			string configPath = SDTM.API.configDataPath;
			string[] splitter = { "/" };
			string[] parts = configName.Split (splitter, StringSplitOptions.RemoveEmptyEntries);
			string dataFolder = configPath;

			if (parts.Length > 1) {
				dataFolder += "/" + parts [0];
			}

			if (FileUtils.ensureDirectoryExists (dataFolder)) {
				configPath = configPath + "/" + configName + ".xml";

				if (!File.Exists (configPath)) {
					if (!Save ()) {
						Log.Error(string.Format("[SDTMLib] Could not Create Initial Config: {0}", configPath));
						return false;
					}
				}

				XmlDocument xmlDoc = new XmlDocument();
				try{
					xmlDoc.Load(configPath);
				}
				catch (XmlException e){
					Log.Error(string.Format("[SDTMLib] Could not Load Config: {0}", e.Message));
					return false;
				}

				XmlNode rootNode = xmlDoc.DocumentElement;

				foreach (XmlNode configNode in rootNode.ChildNodes) {
					properties.Add (configNode.Name, configNode.InnerText);
				}

				return true;
			}

			return false;
		}

		public bool Save(){
			string configPath = SDTM.API.configDataPath;
			if (FileUtils.ensureDirectoryExists (configPath)) {
				configPath = configPath + "/" + configName + ".xml";
				StreamWriter sw = new StreamWriter (configPath);

				sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				sw.WriteLine("<Config>");

				foreach (string key in properties.Keys) {
					string val = properties[key];
					sw.WriteLine ("\t<"+key+">" + val + "</"+key+">");
				}

				sw.WriteLine("</Config>");
				sw.Flush();
				sw.Close();

				return true;
			}

			return false;
		}

	}
}

