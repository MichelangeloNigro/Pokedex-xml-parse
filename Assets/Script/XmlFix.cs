using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class XmlFix : MonoBehaviour {
	private XmlDocument pokedexOriginal = new XmlDocument();
	private XmlDocument pokedexFix = new XmlDocument();
	public Action<ScriptablePokemon> pokemonCreated;
	public Action xmlFixed;
	public Action endedCreation;
	public Action OdinButton;
	public Pokedex pokedex;
	public Slider slider;
	[FormerlySerializedAs("SavePath")] public List<string> savePath = new List<string>();
	public Sprite[] sprites;
	public string[] toBeDeleted;
	[Title("Progress Bar")]
	public int PercentageTreshold;
	private int maxint;

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
		xmlFixed();
	}

	private void LoadPokedata() {
		if (File.Exists(Application.dataPath + "/Xml/pokedata.xml")) {
			Debug.Log("file found");
			pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
			pokedexFix = pokedexOriginal;
			Debug.Log("loaded");
		}
	}

	public Color GetColor(float value) {
		return Color.Lerp(Color.red, Color.green,Mathf.Pow(value / 100, 2));
	}
	private void CreateScriptable() {
		XmlNodeList list = pokedexFix.SelectNodes(("//pokemon"));
		foreach (XmlNode entry in list) {
			// if ((int.Parse(entry.SelectSingleNode("@id").InnerText)) % ((pokedexOriginal.SelectNodes("//pokemon").Count * PercentageTreshold / 100)) == 0) {
			// 	slider.value = float.Parse(entry.SelectSingleNode("@id").InnerText) / pokedexOriginal.SelectNodes("//pokemon").Count;
			// 	yield return null;
			// }
			ScriptablePokemon temp = ScriptableObject.CreateInstance<ScriptablePokemon>();
			temp.name = entry.SelectSingleNode("name").InnerText;
			temp.id = int.Parse(entry.SelectSingleNode("@id").InnerText);
			temp.description = entry.SelectSingleNode("description").InnerText;
			temp.Hp = int.Parse(entry.SelectSingleNode("stats/HP").InnerText);
			temp.ATK = int.Parse(entry.SelectSingleNode("stats/ATK").InnerText);
			temp.DEF = int.Parse(entry.SelectSingleNode("stats/DEF").InnerText);
			temp.SPD = int.Parse(entry.SelectSingleNode("stats/SPD").InnerText);
			temp.SAT = int.Parse(entry.SelectSingleNode("stats/SAT").InnerText);
			temp.SDF = int.Parse(entry.SelectSingleNode("stats/SDF").InnerText);
			temp.sprite = sprites[int.Parse(entry.SelectSingleNode("@id").InnerText) - 1];
			foreach (XmlNode typeEntry in entry.SelectNodes("type")) {
				temp.types.Add(typeEntry.InnerText);
			}
			AssetDatabase.CreateAsset(temp, "Assets/Pokemon/" + int.Parse(entry.SelectSingleNode("@id").InnerText).ToString("000") + "_" + entry.SelectSingleNode("name").InnerText + ".asset");
			savePath.Add("Assets/Pokemon/" + int.Parse(entry.SelectSingleNode("@id").InnerText).ToString("000") + "_" + entry.SelectSingleNode("name").InnerText + ".asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			pokemonCreated(temp);
			bar++;
			//non ne ho idea
			foreach (XmlNode variable in entry.SelectNodes("evolutions/evolution/@id")) {
				temp.evolutionsID.Add(int.Parse(variable.InnerText));
			}
		}
		endedCreation();
		slider.value = 1;
	}

	private void OnApplicationQuit() {
		AssetDatabase.DeleteAssets(savePath.ToArray(), savePath);
	}

	private void WithButton() {
		OdinButton += () => {
			sprites = Resources.LoadAll<Sprite>("pokedex");
			LoadPokedata();
			setValuseForInspector();
			FixData();
			CreateScriptable();
		};
	}

	[Title("OdinProve")]
	[Button("Create Pokemons")]
	private void createPokemons() {
		Debug.Log("Start");
		pokedex.ButtonCreation();
		WithButton();
		OdinButton();
	}
	[ReadOnly]
	[ProgressBar(0, "$maxint", ColorMember = "GetColor",Height = 40)]
	public int bar;

	[Button("set max index")]
	private void MaxIndex() {
		pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
		maxint = pokedexOriginal.SelectNodes("//pokemon").Count;
	}

	[Button("Reset")]
	private void res() {
		AssetDatabase.DeleteAssets(savePath.ToArray(), savePath);
		bar = 0;
		maxint = 0;
		pokedex.pokemonss = null;
		savePath = null;
		slider.value = 0;
	}

	public void setValuseForInspector() {
		MaxIndex();
		XmlNodeList list = pokedexFix.SelectNodes(("//pokemon"));
		foreach (XmlNode node in list ) {
			if (int.Parse(node.SelectSingleNode("stats/HP").InnerText)>ScriptablePokemon.MaxHP) {
				ScriptablePokemon.MaxHP = int.Parse(node.SelectSingleNode("stats/HP").InnerText);
			}
		}
	}
	
}