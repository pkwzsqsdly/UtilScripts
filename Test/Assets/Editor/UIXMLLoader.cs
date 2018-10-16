using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class UIXMLElement{
    public string Key;
    public string Content;
    public string UiType;
    public UIXMLElement(string all){

    }
}
public class UIXMLLoader  {
    private XmlDocument m_XMLDoc;
    private Dictionary<string,UIXMLElement> m_EleDic;
    public UIXMLLoader(){
        m_XMLDoc = new XmlDocument();
        m_EleDic = new Dictionary<string, UIXMLElement>();
    }

    public void Load(TextAsset tass){
        m_XMLDoc.LoadXml(tass.text);
        // XmlNode root = mXmlDoc.SelectSingleNode("");
        // var node = .SelectSingleNode("packageDescription/resources");
        XmlNodeList list = m_XMLDoc.GetElementsByTagName("*");//node.SelectNodes("*");
        for(int i = 0; i < list.Count; i++) {
            XmlNode row = list[i];
            var dd = row.Attributes["id"];
            if(dd != null){
                Debug.Log(row.Attributes.ToString());
                if(!m_EleDic.ContainsKey(dd.Value)){
                    // UIXMLElement ele = new UIXMLElement(); 
                    // m_EleDic.Add(dd,)
                }
            }
        }
    }
	
}
