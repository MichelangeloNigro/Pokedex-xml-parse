using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon_", menuName = "Pokemon/Create a Pokemon")]
public class ScriptablePokemon : ScriptableObject {
	public int id;
	public string name;
	public Sprite sprite;
	public string description;
	public List<string> types=new List<string>();
//	public type tipoEnum;
	public int Hp;
	public int ATK;
	public int DEF;
	public int SPD;
	public int SAT;
	public int SDF;
	public List<ScriptablePokemon> evolutions=new List<ScriptablePokemon>();
	public List<int> evolutionsID=new List<int>();

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