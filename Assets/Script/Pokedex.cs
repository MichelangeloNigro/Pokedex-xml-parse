using System.Xml;
using UnityEngine;

public class Pokedex : MonoBehaviour {
	private XmlDocument pokedexOriginal = new XmlDocument();
	public ScriptablePokemon[] pokemonss;
	public XmlFix fix;

	private void Start() {
		fix.xmlFixed = () => {
			pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
			pokemonss = new ScriptablePokemon[pokedexOriginal.SelectNodes("//pokemon").Count];
			for (int i = 0; i < pokemonss.Length; i++) {
				pokemonss[i] = ScriptableObject.CreateInstance<ScriptablePokemon>();
			}
		};
		fix.pokemonCreated += RegisterPokemon;
		fix.endedCreation+=assignEvolution;
	}

	public void RegisterPokemon(ScriptablePokemon toBeRegistered) {
		pokemonss[toBeRegistered.id-1] = toBeRegistered;
	}

	public void assignEvolution() {
		foreach (var poke in pokemonss) {
			foreach (var evoID in poke.evolutionsID) {
				poke.evolutions.Add(pokemonss[evoID-1]);
			}
		}
	}
}