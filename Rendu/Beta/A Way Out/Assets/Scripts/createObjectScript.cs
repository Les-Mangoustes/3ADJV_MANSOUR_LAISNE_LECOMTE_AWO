using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class createObjectScript : MonoBehaviour {

    private string XMLPath;
    private Dictionary<string, objectClass> objectList;

	// Use this for initialization
	void Start () {
        XMLPath = Application.dataPath + "/AWObject.xml";
        objectList = new Dictionary<string, objectClass>();
        createListFromXML();
	}

    private void createListFromXML()
    {
        if(File.Exists(XMLPath))
        {
            GameObject prefab;
            float timeSpawn;
            float buffValue;
            float buffTime;
            XmlDocument doc = new XmlDocument();
            doc.Load(XMLPath);
            XmlNode database = doc.GetElementsByTagName("databaseObject")[0];
            foreach(XmlNode node in database)
            {
                //prefab = (GameObject)Resources.Load(node["PrefabPath"].InnerText,typeof(GameObject));
                //Debug.Log(prefab);
                timeSpawn = float.Parse(node["TimeSpawn"].InnerText);
                buffTime = float.Parse(node["BuffTime"].InnerText);
                buffValue = float.Parse(node["BuffValue"].InnerText);
                //objectList.Add("toto", new objectClass(prefab, timeSpawn, buffValue, buffTime));
                //Debug.Log("name : " + prefab.name + " // timeSpawn : " + objectList[prefab.name].TimeSpawn + " // buffValue : " + objectList[prefab.name].BuffValue + " // buffTime : " + objectList[prefab.name].BuffTime);
            }
        }
    }

}
