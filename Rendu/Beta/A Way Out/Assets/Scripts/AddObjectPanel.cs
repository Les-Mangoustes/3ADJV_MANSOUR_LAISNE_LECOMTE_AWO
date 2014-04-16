using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Xml;
using System.IO;

#if UNITY_EDITOR
public class AddObjectPanel : EditorWindow {

    GameObject prefab;

    static float spawnTime;
    static float buffValue;
    static float buffTime;

    [MenuItem("Editor Menu/AddObjectPanel")]
    public static void CreateWindow()
    {
        var window = new AddObjectPanel();
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab : ");
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Spawn Time (secondes) : ");
        spawnTime = EditorGUILayout.FloatField(spawnTime);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Buff value (Speed if > 0) : ");
        buffValue = EditorGUILayout.FloatField(buffValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Slow Time (must be positif) : ");
        buffTime = EditorGUILayout.FloatField(buffTime);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create"))
        {
            createObject();
        }
    }

    public void createObject()
    {
        string file_path = Application.dataPath + "/AWObject.xml";
        XmlDocument doc = new XmlDocument();
        XmlNode database;
        if (File.Exists(file_path))
        {
            doc.Load(file_path);
            database = doc.GetElementsByTagName("databaseObject")[0];
        }
        else
        {
            database = doc.CreateElement("databaseObject");
        }
        XmlNode new_obj = doc.CreateElement("Object");
        XmlNode prefabXML = doc.CreateElement("PrefabPath");
        prefabXML.InnerText = Application.dataPath + "/Prefabs/" + prefab.name + ".prefab";
        new_obj.AppendChild(prefabXML);
        XmlNode spawnXML = doc.CreateElement("TimeSpawn");
        spawnXML.InnerText = spawnTime.ToString();
        new_obj.AppendChild(spawnXML);
        XmlNode buffTimeXML = doc.CreateElement("BuffTime");
        buffTimeXML.InnerText = buffTime.ToString();
        new_obj.AppendChild(buffTimeXML);
        XmlNode buffValueXML = doc.CreateElement("BuffValue");
        buffValueXML.InnerText = buffValue.ToString();
        new_obj.AppendChild(buffValueXML);
        database.AppendChild(new_obj);
        doc.AppendChild(database);
        doc.Save(file_path);
    }
}
#endif
