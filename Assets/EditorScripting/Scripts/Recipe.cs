using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Recipe")]
public class Recipe : ScriptableObject {
	[System.Serializable]
	public struct Ingredient {
		public Item item;
		public int amount;
	}
	
	public string recipeName;
	public string description;
	public Ingredient[] ingredients;
	//public Ingredient product;
}
