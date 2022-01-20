using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Xml;
using UnityEditor;

public class XmlFix : MonoBehaviour {
	private XmlDocument pokedexOriginal = new XmlDocument();
	private XmlDocument pokedexFix = new XmlDocument();
	public Action<ScriptablePokemon> PokemonCreated;
	public Sprite[] sprites;
	public string[] toBeDeleted;
	public ScriptablePokemon prova;

	IEnumerator Start() {
		yield return new WaitForSeconds(0.1f);
		sprites = Resources.LoadAll<Sprite>("pokedex");
		LoadPokedata();
		FixData();
		CreateScriptable();
	}

	private void FixData() {
		foreach (var tobedeleted in toBeDeleted) {
			// Debug.Log("Searching for entry to delete");
			// Debug.Log(pokedexFix.SelectNodes(("//" + tobedeleted)).Count + tobedeleted);
			if (pokedexFix.SelectNodes(("//" + tobedeleted)).Count > 0) {
				//Debug.Log("found node");
				foreach (XmlNode entry in pokedexFix.SelectNodes("//" + tobedeleted)) {
					entry.ParentNode.RemoveChild(entry);
					//Debug.Log("deleted node "+ entry.InnerText);
				}
			}
		}
		pokedexFix.Save(Application.dataPath + "/fixedData.xml");
		Debug.Log("saved in " + pokedexFix.Name);
	}

	private void LoadPokedata() {
		if (File.Exists(Application.dataPath + "/Xml/pokedata.xml")) {
			Debug.Log("file found");
			pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
			pokedexFix = pokedexOriginal;
			Debug.Log("loaded");
		}
	}

	public void CreateScriptable() {
		XmlNodeList list = pokedexFix.SelectNodes(("//pokemon"));
		foreach (XmlNode entry in list) { 
			Debug.Log(entry.SelectNodes("type")[0].InnerText);
			ScriptablePokemon temp=ScriptableObject.CreateInstance<ScriptablePokemon>() ;
			 temp.name = entry.SelectSingleNode("name").InnerText;
			 temp.id =  int.Parse(entry.SelectSingleNode("@id").InnerText);
			temp.description = entry.SelectSingleNode("description").InnerText;
			 temp.Hp = int.Parse(entry.SelectSingleNode("stats/HP").InnerText);
			 temp.ATK = int.Parse(entry.SelectSingleNode("stats/ATK").InnerText);
			 temp.DEF = int.Parse(entry.SelectSingleNode("stats/DEF").InnerText);
			 temp.SPD = int.Parse(entry.SelectSingleNode("stats/SPD").InnerText);
			 temp.SAT = int.Parse(entry.SelectSingleNode("stats/SAT").InnerText);
			 temp.SDF = int.Parse(entry.SelectSingleNode("stats/SDF").InnerText);
			 temp.sprite = sprites[int.Parse(entry.SelectSingleNode("@id").InnerText)-1];
			 foreach (XmlNode typeEntry in entry.SelectNodes("type")) {
				 temp.types.Add(typeEntry.InnerText);
			 }
			 AssetDatabase.CreateAsset(temp, "Assets/Pokemon/"+int.Parse(entry.SelectSingleNode("@id").InnerText).ToString("000")+"_"+entry.SelectSingleNode("name").InnerText+".asset");
			 AssetDatabase.SaveAssets();
			 AssetDatabase.Refresh();
			 PokemonCreated(prova);
			 
		}


	}
}
