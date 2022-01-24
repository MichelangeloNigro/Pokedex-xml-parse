using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon_", menuName = "Pokemon/Create a Pokemon")]
public class ScriptablePokemon : ScriptableObject {
	public int id;
	public string name;
	public Sprite sprite;
	public string description;
	public List<string> types = new List<string>();
	//	public type tipoEnum;
	[ReadOnly,ProgressBar(0,"$MaxHP",ColorMember = "GetColor")]
	public int Hp;
	public int ATK;
	public int DEF;
	public int SPD;
	public int SAT;
	public int SDF;
	public List<ScriptablePokemon> evolutions = new List<ScriptablePokemon>();
	[HideInInspector] public List<int> evolutionsID = new List<int>();

	public static int MaxHP;
	private Color GetColor(int value) {
		return Color.Lerp(Color.red, Color.green,Mathf.Pow(value / 100, 2));
	}
}

public enum type {
	normal,
	fight,
	flying,
	poison,
	ground,
	rock,
	bug,
	ghost,
	steel,
	fire,
	water,
	grass,
	electric,
	psychic,
	ice,
	dragon,
	dark
}