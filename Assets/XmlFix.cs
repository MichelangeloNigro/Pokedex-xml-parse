using System.IO;
using UnityEngine;
using System.Xml;
using UnityEditor;

public class XmlFix : MonoBehaviour {
  private XmlDocument PokedexOriginal=new XmlDocument();
  private XmlDocument PokedexFix=new XmlDocument();
  public string[] toBeDeleted;
  private void Awake() {
    loadPokedata();
    fixData();
  }

  private void fixData() {
    foreach (var tobedeleted in toBeDeleted) {
      Debug.Log("Searching for entry to delete");
      Debug.Log(PokedexFix.SelectNodes(("//"+tobedeleted)).Count + tobedeleted);
      if (PokedexFix.SelectNodes(("//"+tobedeleted)).Count>0) {
        //Debug.Log("found node");
        foreach (XmlNode entry in PokedexFix.SelectNodes("//"+tobedeleted)) {
          entry.ParentNode.RemoveChild(entry);
          //Debug.Log("deleted node "+ entry.InnerText);
        }
      }
    }
    PokedexFix.Save(Application.dataPath+"/fixedData.xml");
    Debug.Log("saved in "+PokedexFix.Name);
  }

  private void loadPokedata() {
    if (File.Exists(Application.dataPath+"/pokedata.xml")) {
      Debug.Log("file found");
      PokedexOriginal.Load(Application.dataPath+"/pokedata.xml");
      PokedexFix = PokedexOriginal;
      Debug.Log("loaded");
    }
  }
}