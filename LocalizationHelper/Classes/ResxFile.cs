using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LocalizationHelper.Classes
{
	internal class ResxFile
    {
        private string _filePath;
        public Dictionary<string, string> Strings = new Dictionary<string, string>();

        public ResxFile(string FilePath)
        {
            _filePath = FilePath;
            LoadFile();
        }

        private void LoadFile()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_filePath);
            XmlNodeList dataList = xDoc.GetElementsByTagName("data");
            foreach (XmlNode node in dataList)
            {
                foreach (XmlNode cNode in node.ChildNodes)
                {
                    if (cNode.Name.ToLower() == "value")
                    {
                        Strings.Add(node.Attributes["name"].Value, cNode.InnerText);
                        continue;
                    }
                }
            }
        }

        public void SetString(string key, string stringVal)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_filePath);
            XmlNode nod = (from XmlNode N in xDoc.GetElementsByTagName("data") where N.Attributes["name"] != null && N.Attributes["name"].Value == key select N).FirstOrDefault();
            if (nod != null)
            {
                XmlNode iNod = (from XmlNode N in nod.ChildNodes where N.Name == "value" select N).FirstOrDefault();
                if (iNod != null)
                {
                    iNod.InnerText = stringVal;
                    xDoc.Save(_filePath);
                }
            }
            else //Key must have been added
            {
                XmlNode format = xDoc.CreateNode(XmlNodeType.SignificantWhitespace, "", "");
                format.InnerText = "\r\n    ";
                XmlNode format2 = xDoc.CreateNode(XmlNodeType.SignificantWhitespace, "", "");
                format.InnerText = "\r\n";

                XmlNode newNod = xDoc.CreateNode(XmlNodeType.Element, "data", xDoc.NamespaceURI);

                XmlAttribute name = xDoc.CreateAttribute("name");
                name.Value = key;

                XmlAttribute space = xDoc.CreateAttribute("xml:space");
                space.Value = "preserve";

                newNod.Attributes.Append(name);
                newNod.Attributes.Append(space);

                XmlNode valNod = xDoc.CreateNode(XmlNodeType.Element, "value", xDoc.NamespaceURI);
                valNod.InnerText = stringVal;
                newNod.AppendChild(format);
                newNod.AppendChild(valNod);
                newNod.AppendChild(format2);
                xDoc.LastChild.AppendChild(newNod);
                xDoc.Save(_filePath);
            }
        }
    }
}
