using System.Xml;
using UnityEngine;

public class Pokedex : MonoBehaviour {
	private XmlDocument pokedexOriginal = new XmlDocument();
	public ScriptablePokemon[] pokemonss;
	public XmlFix fix;
	public ScriptablePokemon highestHP;
	public ScriptablePokemon highestATK;
	public ScriptablePokemon highestDEF;
	public ScriptablePokemon highestSPD;
	public ScriptablePokemon highestSAT;
	public ScriptablePokemon highestSDF;
	public ScriptablePokemon highestStats;
	

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

  public void  ButtonCreation() {
		fix.odinButton += () => {
			fix.xmlFixed = () => {
				pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
				pokemonss = new ScriptablePokemon[pokedexOriginal.SelectNodes("//pokemon").Count];
				for (int i = 0; i < pokemonss.Length; i++) {
					pokemonss[i] = ScriptableObject.CreateInstance<ScriptablePokemon>();
				}
			};
			fix.pokemonCreated += RegisterPokemon;
			fix.endedCreation+=assignEvolution;
			
		};
	}
}