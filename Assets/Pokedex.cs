using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokedex : MonoBehaviour {
  public List<ScriptablePokemon> pokemonss=new List<ScriptablePokemon>();
  private int currentRegsitered=0;
  public XmlFix fix;

  private void Start() {
    fix.PokemonCreated += RegisterPokemon;
  }

  public void RegisterPokemon(ScriptablePokemon toBeRegistered) {
    pokemonss.Add(toBeRegistered);
    currentRegsitered++;
  }
}
