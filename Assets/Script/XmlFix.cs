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
using Random = UnityEngine.Random;

public class XmlFix : MonoBehaviour
{
    private XmlDocument pokedexOriginal = new XmlDocument();
    private XmlDocument pokedexFix = new XmlDocument();
    public Action<ScriptablePokemon> pokemonCreated;
    public Action xmlFixed;
    public Action endedCreation;
    public Action odinButton;
    public Pokedex pokedex;
    static public bool arePokemonCreated;
    public Slider slider;
    [FormerlySerializedAs("SavePath")] public List<string> savePath = new List<string>();
    public Sprite[] sprites;
    public string[] toBeDeleted;

    [FormerlySerializedAs("PercentageTreshold")] [Title("Progress Bar")]
    public int percentageTreshold;

    private int maxint;
    [Space] public TMPro.TMP_Text name;
    public TMPro.TMP_Text hp;
    public Slider hpSlider;
    public TMPro.TMP_Text att;
    public Slider attSlider;
    public TMPro.TMP_Text def;
    public Slider defSlider;
    public TMPro.TMP_Text sat;
    public Slider satSlider;
    [FormerlySerializedAs("spf")] public TMPro.TMP_Text SDF;
    public Slider sdfSlider;
    public TMPro.TMP_Text spd;
    public Slider spdSlider;
    public TMPro.TMP_Text type;
    public Image sprite;
    private bool nameFound;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        sprites = Resources.LoadAll<Sprite>("pokedex");
        LoadPokedata();
        FixData();
        CreateScriptable();
    }

    private void FixData()
    {
        foreach (var tobedeleted in toBeDeleted)
        {
            // Debug.Log("Searching for entry to delete");
            // Debug.Log(pokedexFix.SelectNodes(("//" + tobedeleted)).Count + tobedeleted);
            if (pokedexFix.SelectNodes(("//" + tobedeleted)).Count > 0)
            {
                //Debug.Log("found node");
                foreach (XmlNode entry in pokedexFix.SelectNodes("//" + tobedeleted))
                {
                    entry.ParentNode.RemoveChild(entry);
                    //Debug.Log("deleted node "+ entry.InnerText);
                }
            }
        }

        pokedexFix.Save(Application.dataPath + "/fixedData.xml");
        Debug.Log("saved in " + pokedexFix.Name);
        xmlFixed();
    }

    private void LoadPokedata()
    {
        if (File.Exists(Application.dataPath + "/Xml/pokedata.xml"))
        {
            Debug.Log("file found");
            pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
            pokedexFix = pokedexOriginal;
            Debug.Log("loaded");
        }
    }

    public Color GetColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100, 2));
    }

    private void CreateScriptable()
    {
        XmlNodeList list = pokedexFix.SelectNodes(("//pokemon"));
        foreach (XmlNode entry in list)
        {
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
            foreach (XmlNode typeEntry in entry.SelectNodes("type"))
            {
                temp.types.Add(typeEntry.InnerText);
            }

            AssetDatabase.CreateAsset(temp,
                "Assets/Pokemon/" + int.Parse(entry.SelectSingleNode("@id").InnerText).ToString("000") + "_" +
                entry.SelectSingleNode("name").InnerText + ".asset");
            savePath.Add("Assets/Pokemon/" + int.Parse(entry.SelectSingleNode("@id").InnerText).ToString("000") + "_" +
                         entry.SelectSingleNode("name").InnerText + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            pokemonCreated(temp);
            bar++;
            //non ne ho idea
            foreach (XmlNode variable in entry.SelectNodes("evolutions/evolution/@id"))
            {
                temp.evolutionsID.Add(int.Parse(variable.InnerText));
            }
        }

        endedCreation();
        arePokemonCreated = true;
        Battle.pokemonCreated = true;
        hpSlider.maxValue = ScriptablePokemon.MaxHP;
        attSlider.maxValue = ScriptablePokemon.MaxATK;
        defSlider.maxValue = ScriptablePokemon.MaxDEF;
        satSlider.maxValue = ScriptablePokemon.MaxSAT;
        sdfSlider.maxValue = ScriptablePokemon.MaxSDF;
        spdSlider.maxValue = ScriptablePokemon.MaxSPD;
        slider.value = 1;
    }

    private void OnApplicationQuit()
    {
        AssetDatabase.DeleteAssets(savePath.ToArray(), savePath);
    }

    private void WithButton()
    {
        odinButton += () =>
        {
            sprites = Resources.LoadAll<Sprite>("pokedex");
            LoadPokedata();
            setValuseForInspector();
            FixData();
            CreateScriptable();
            SetPokemonsBasedOnStat();
        };
    }

    [Title("OdinProve")]
    [Button("Create Pokemons")]
    private void CreatePokemons()
    {
        Debug.Log("Start");
        pokedex.ButtonCreation();
        WithButton();
        odinButton();
    }

    [ReadOnly] [ProgressBar(0, "$maxint", ColorMember = "GetColor", Height = 40)]
    public int bar;

    [Button("set max index")]
    private void MaxIndex()
    {
        pokedexOriginal.Load(Application.dataPath + "/Xml/pokedata.xml");
        maxint = pokedexOriginal.SelectNodes("//pokemon").Count;
    }

    [Button("Reset")]
    private void Res()
    {
        AssetDatabase.DeleteAssets(savePath.ToArray(), savePath);
        bar = 0;
        maxint = 0;
        pokedex.highestHP = null;
        pokedex.highestATK = null;
        pokedex.highestDEF = null;
        pokedex.highestSAT = null;
        pokedex.highestSDF = null;
        pokedex.highestSPD = null;
        pokedex.pokemonss = null;
        savePath = null;
        slider.value = 0;
        pokedex.highestHP = null;
        arePokemonCreated = false;
        Battle.pokemonCreated = false;
        name.text = "";
        sprite.sprite = null;
        type.text = "";
        hp.text = "HP:";
        att.text = "ATK:";
        def.text = "DEF:";
        sat.text = "SAT:";
        SDF.text = "SDF:";
        spd.text = "SPD:";
        hpSlider.maxValue = 1;
        attSlider.maxValue = 1;
        defSlider.maxValue = 1;
        satSlider.maxValue = 1;
        sdfSlider.maxValue = 1;
        spdSlider.maxValue = 1;
        hpSlider.value = 1;
        attSlider.value = 1;
        defSlider.value = 1;
        satSlider.value = 1;
        sdfSlider.value = 1;
        spdSlider.value = 1;
        PokemonToSearchID = 0;
        nameFound = false;
        NameToSearch = "";
    }

    public void StampPokemon(ScriptablePokemon pokemon)
    {
        hpSlider.value = pokemon.Hp;
        attSlider.value = pokemon.ATK;
        defSlider.value = pokemon.DEF;
        satSlider.value = pokemon.SAT;
        sdfSlider.value = pokemon.SDF;
        spdSlider.value = pokemon.SPD;
        name.text = $"#{pokemon.id.ToString("000")}  {pokemon.name}";
        sprite.sprite = pokemon.sprite;
        type.text = "";
        hp.text = "HP:";
        att.text = "ATK:";
        def.text = "DEF:";
        sat.text = "SAT:";
        SDF.text = "SDF:";
        spd.text = "SPD:";
        foreach (var tipo in pokemon.types)
        {
            type.text += $" {tipo} ";
        }

        hp.text += $" {pokemon.Hp}";
        att.text += $" {pokemon.ATK}";
        def.text += $" {pokemon.DEF}";
        sat.text += $" {pokemon.SAT}";
        SDF.text += $" {pokemon.SDF}";
        spd.text += $" {pokemon.SPD}";
    }

    private bool ValidateNameSearch(string name)
    {
        foreach (var VARIABLE in pokedex.pokemonss)
        {
            if (VARIABLE.name == name)
            {
                PokemonToSearchID = VARIABLE.id - 1;
                nameFound = true;
                return true;
            }
        }

        return false;
    }

    private void NameOff()
    {
        nameFound = false;
    }

    [Title("Pokemon Creation")]
    [Button("Random Pokemon"), ShowIf("arePokemonCreated")]
    public void stampRandomPokemon()
    {
        int temp = Random.Range(0, pokedex.pokemonss.Length);
        StampPokemon(pokedex.pokemonss[temp]);
    }

    [ShowIf("arePokemonCreated")]
    [ValidateInput("ValidateNameSearch", "Name not Found, name must be in CAPSLOCK"), OnValueChanged("NameOff")]
    public string NameToSearch;

    private int PokemonToSearchID;

    [Button("Search Pokemon"), ShowIf("arePokemonCreated"), EnableIf("nameFound")]
    public void stampSearchedPokemon()
    {
        StampPokemon(pokedex.pokemonss[PokemonToSearchID]);
    }

    [Button("Highest HP"), ShowIf("arePokemonCreated")]
    public void stampHighestHPPokemon()
    {
        StampPokemon(pokedex.highestHP);
    }

    [Button("Highest ATK"), ShowIf("arePokemonCreated")]
    public void stampHighestATKPokemon()
    {
        StampPokemon(pokedex.highestATK);
    }

    [Button("Highest DEF"), ShowIf("arePokemonCreated")]
    public void stampHighestDEFPokemon()
    {
        StampPokemon(pokedex.highestDEF);
    }

    [Button("Highest SAT"), ShowIf("arePokemonCreated")]
    public void stampHighestSATPokemon()
    {
        StampPokemon(pokedex.highestSAT);
    }

    [Button("Highest SDF"), ShowIf("arePokemonCreated")]
    public void stampHighestSDFPokemon()
    {
        StampPokemon(pokedex.highestSDF);
    }

    [Button("Highest SPD"), ShowIf("arePokemonCreated")]
    public void stampHighestSPDPokemon()
    {
        StampPokemon(pokedex.highestSPD);
    }

    [Button("Best Pokemon"), ShowIf("arePokemonCreated")]
    public void StampBestPokemon()
    {
        StampPokemon(pokedex.highestStats);
    }

    public void setValuseForInspector()
    {
        MaxIndex();
        XmlNodeList list = pokedexFix.SelectNodes(("//pokemon"));
        foreach (XmlNode node in list)
        {
            if (int.Parse(node.SelectSingleNode("stats/HP").InnerText) > ScriptablePokemon.MaxHP)
            {
                ScriptablePokemon.MaxHP = int.Parse(node.SelectSingleNode("stats/HP").InnerText);
                ScriptablePokemon.MaxHPId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if (int.Parse(node.SelectSingleNode("stats/ATK").InnerText) > ScriptablePokemon.MaxATK)
            {
                ScriptablePokemon.MaxATK = int.Parse(node.SelectSingleNode("stats/ATK").InnerText);
                ScriptablePokemon.MaxATKId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if (int.Parse(node.SelectSingleNode("stats/DEF").InnerText) > ScriptablePokemon.MaxDEF)
            {
                ScriptablePokemon.MaxDEF = int.Parse(node.SelectSingleNode("stats/DEF").InnerText);
                ScriptablePokemon.MaxDEFId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if (int.Parse(node.SelectSingleNode("stats/SAT").InnerText) > ScriptablePokemon.MaxSAT)
            {
                ScriptablePokemon.MaxSAT = int.Parse(node.SelectSingleNode("stats/SAT").InnerText);
                ScriptablePokemon.MaxSATId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if (int.Parse(node.SelectSingleNode("stats/SDF").InnerText) > ScriptablePokemon.MaxSDF)
            {
                ScriptablePokemon.MaxSDF = int.Parse(node.SelectSingleNode("stats/SDF").InnerText);
                ScriptablePokemon.MaxSDFId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if (int.Parse(node.SelectSingleNode("stats/SPD").InnerText) > ScriptablePokemon.MaxSPD)
            {
                ScriptablePokemon.MaxSPD = int.Parse(node.SelectSingleNode("stats/SPD").InnerText);
                ScriptablePokemon.MaxSPDId = int.Parse(node.SelectSingleNode("@id").InnerText);
            }

            if ((int.Parse(node.SelectSingleNode("stats/SPD").InnerText) +
                 int.Parse(node.SelectSingleNode("stats/SDF").InnerText) +
                 int.Parse(node.SelectSingleNode("stats/SAT").InnerText) +
                 int.Parse(node.SelectSingleNode("stats/DEF").InnerText) +
                 int.Parse(node.SelectSingleNode("stats/ATK").InnerText) +
                 int.Parse(node.SelectSingleNode("stats/HP").InnerText) >
                 ScriptablePokemon.BestPokemon))
            {
                ScriptablePokemon.BestPokemon =
                    (int.Parse(node.SelectSingleNode("stats/SPD").InnerText) +
                     int.Parse(node.SelectSingleNode("stats/SDF").InnerText) +
                     int.Parse(node.SelectSingleNode("stats/SAT").InnerText) +
                     int.Parse(node.SelectSingleNode("stats/DEF").InnerText) +
                     int.Parse(node.SelectSingleNode("stats/ATK").InnerText) +
                     int.Parse(node.SelectSingleNode("stats/HP").InnerText));
                ScriptablePokemon.BestPokemonID = int.Parse(node.SelectSingleNode("@id").InnerText);
            }
        }
    }

    public void SetPokemonsBasedOnStat()
    {
        pokedex.highestHP = pokedex.pokemonss[ScriptablePokemon.MaxHPId - 1];
        pokedex.highestATK = pokedex.pokemonss[ScriptablePokemon.MaxATKId - 1];
        pokedex.highestDEF = pokedex.pokemonss[ScriptablePokemon.MaxDEFId - 1];
        pokedex.highestSAT = pokedex.pokemonss[ScriptablePokemon.MaxSATId - 1];
        pokedex.highestSDF = pokedex.pokemonss[ScriptablePokemon.MaxSDFId - 1];
        pokedex.highestSPD = pokedex.pokemonss[ScriptablePokemon.MaxSPDId - 1];
        pokedex.highestStats = pokedex.pokemonss[ScriptablePokemon.BestPokemonID - 1];
    }
}