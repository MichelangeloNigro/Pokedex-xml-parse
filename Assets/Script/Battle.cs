using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
 public Pokedex pokedex;
 private int PokemonToSearchID;
 private int PokemonToSearchIDCPU;
 private bool nameFound;
 private bool nameFoundCPU;
 private bool fightStarted;
 private ScriptablePokemon yourScriptable;
 private ScriptablePokemon cpuScriptable;
 private bool finish;
 private bool isYourTurn;
 private float yourHP;
 private float CPUHP;
 private float yourATK;
 private float CPUATK;
 private float yourDEF;
 private float CPUDEF;
 private float yourSDF;
 private float CPUSDF;
 private float yourSAT;
 private float CPUSAT;
 private float atkTrue;
 private float defTrue;
 private float defCurrent; 
 private float atkTrueCPU;
 private float defTrueCPU;
 private float defCurrentCPU;
 static public bool pokemonCreated;
 
 [ShowIf("pokemonCreated"),ValidateInput("ValidateNameSearch", "Name not Found, name must be in CAPSLOCK"),OnValueChanged("NameOff")]
 public string yourPokemon;
 [ShowIf("pokemonCreated"),ValidateInput("ValidateNameSearchCPU", "Name not Found, name must be in CAPSLOCK"),OnValueChanged("NameOffCPU")]
 public string cpuPokemon;
 [HorizontalGroup("group1"),VerticalGroup("group1/sx"),PreviewField(150),ReadOnly]
 [ShowIf("pokemonCreated"), ShowIf("nameFound"), ShowIf("nameFoundCPU")]
 public Sprite yourSprite;
 [HorizontalGroup("group1"),PreviewField(150),ReadOnly]
 [ShowIf("pokemonCreated"), ShowIf("nameFound"), ShowIf("nameFoundCPU"),VerticalGroup("group1/dx")]
 public Sprite cpuSprite;
 [ProgressBar(0, "$yourHP", ColorMember = "GetColor"),VerticalGroup("group1/sx"),LabelWidth(50),ReadOnly]
 public float CurrentHp;
 [ProgressBar(0, "$CPUHP", ColorMember = "GetColor"),VerticalGroup("group1/dx"),LabelWidth(50),ReadOnly ]
 public float CurrentHPCPU;

 [ShowIf("pokemonCreated"), ShowIf("nameFound"), ShowIf("nameFoundCPU")]
 [Button("Fight")] 
 public void StartFight()
 {
  fightStarted = true;
  yourATK = yourScriptable.ATK/10f;
  yourHP = yourScriptable.Hp;
  yourDEF = yourScriptable.DEF/100f;
  yourSAT = yourScriptable.SAT/10f;
  yourSDF = yourScriptable.SDF/100f;
  CPUATK = cpuScriptable.ATK/10f;
  CPUHP = cpuScriptable.Hp;
  CPUDEF = cpuScriptable.DEF/100f;
  CPUSAT = cpuScriptable.SAT/10f;
  CPUSDF = cpuScriptable.SDF/100f;
  atkTrue = yourATK + yourSAT;
  defTrue = yourDEF + yourSDF; 
  atkTrueCPU = CPUATK + CPUSAT;
  defTrueCPU = CPUDEF + CPUSDF;
  defCurrent = defTrue;
  defTrueCPU = defCurrentCPU;
  CurrentHp = yourHP;
  CurrentHPCPU = CPUHP;
  isYourTurn = true;
 }

 [ShowIf("fightStarted"), EnableIf("isYourTurn"),DisableIf("finish")]
 [Button("Attack")]
 private void Attack()
 {
  CurrentHPCPU -= (atkTrue-defCurrentCPU);
  if (CurrentHPCPU<=0)
  {
   finish = true;
   return;
  }

  isYourTurn = false;
  resetCPUDef();
  CPUTurn();
 }
 [ShowIf("fightStarted"), EnableIf("isYourTurn"),DisableIf("finish")]
 [Button("Defend")]
 private void defend()
 {
  defCurrent = defTrue * 3;
  isYourTurn = false;
  resetCPUDef();
  CPUTurn();
 }

 private void CPUTurn()
 {
  int temp = Random.Range(0, 2);

  if (temp==1)
  {
   Debug.Log("Cpu Attack");
   CurrentHp -= (atkTrueCPU-defCurrent);
   if (CurrentHp<=0)
   {
    finish = true;
    return;
   }
   
   resetYourDef();
   isYourTurn = true;
  }
  else
  {
   Debug.Log("Cpu Defend");
   defCurrentCPU = defTrueCPU * 3;
   isYourTurn = true;
   resetYourDef();
  }
 }

 private void resetYourDef()
 {
  defCurrent = defTrue;
 } 
 private void resetCPUDef()
 {
  defCurrentCPU = defTrueCPU;
 }
 private bool ValidateNameSearch(string name) {
  foreach (var VARIABLE in pokedex.pokemonss) {
   if (VARIABLE.name==name) {
    PokemonToSearchID = VARIABLE.id-1;
    nameFound = true;
    yourSprite = VARIABLE.sprite;
    yourScriptable = VARIABLE;
    return true;
   }
  }
  return false;
 }
 private bool ValidateNameSearchCPU(string name) {
  foreach (var VARIABLE in pokedex.pokemonss) {
   if (VARIABLE.name==name) {
    PokemonToSearchIDCPU = VARIABLE.id-1;
    nameFoundCPU = true;
    cpuSprite = VARIABLE.sprite;
    cpuScriptable = VARIABLE;
    return true;
   }
  }
  return false;
 }
 private void NameOff()
 {
  nameFound = false;
 } 
 private void NameOffCPU()
 {
  nameFoundCPU = false;
 }
 private Color GetColor(int value)
 {
  return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
 }
[Button("Reset")]
 public void reset()
 {
  fightStarted = false;
  yourATK = 0;
  yourHP = 0;
  yourDEF =0;
  yourSAT = 0;
  yourSDF = 0;
  CPUATK = 0;
  CPUHP = 0;
  CPUDEF = 0;
  CPUSAT = 0;
  CPUSDF =0;
  atkTrue = 0;
  defTrue = 0; 
  atkTrueCPU =0;
  defTrueCPU =0;
  defCurrent = 0;
  defTrueCPU = 0;
  nameFound = false;
  nameFoundCPU = false;
  PokemonToSearchID = 0;
  PokemonToSearchIDCPU = 0;
  yourPokemon = "";
  cpuPokemon = "";
  finish = false;
  CurrentHp = 0;
  CurrentHPCPU = 0;
 }
}
