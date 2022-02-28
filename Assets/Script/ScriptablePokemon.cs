using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon_", menuName = "Pokemon/Create a Pokemon")]
public class ScriptablePokemon : ScriptableObject
{
    [HorizontalGroup("header")]
    [VerticalGroup("header/InfoAndStats")]
    [BoxGroup("header/InfoAndStats/Infos"),LabelWidth(50f)] public int id;
    [BoxGroup("header/InfoAndStats/Infos"),LabelWidth(50f)] public string name;

    [BoxGroup("header/InfoAndStats/Stats")]
    [HorizontalGroup("header/InfoAndStats/Stats/Horizontal")]
    [ReadOnly,LabelWidth(50f), ProgressBar(0, "$MaxHP", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/1")]
    public int Hp;

    [ReadOnly, LabelWidth(50f),ProgressBar(0, "$MaxATK", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/1")]
    public int ATK;

    [ReadOnly,LabelWidth(50f) ,ProgressBar(0, "$MaxSAT", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/1")]
    public int SAT;

    [ReadOnly,LabelWidth(50f), ProgressBar(0, "$MaxDEF", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/2")]
    public int DEF;

    [ReadOnly, LabelWidth(50f),ProgressBar(0, "$MaxSPD", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/2")]
    public int SPD;

    [ReadOnly, LabelWidth(50f),ProgressBar(0, "$MaxSDF", ColorMember = "GetColor"), VerticalGroup("header/InfoAndStats/Stats/Horizontal/2")]
    public int SDF;

    [HorizontalGroup("header"),PreviewField(150,ObjectFieldAlignment.Center),HideLabel]
    public Sprite sprite;

    [TextArea] public string description;

    [ListDrawerSettings(Expanded = true,IsReadOnly = true)]
    public List<string> types = new List<string>();
    //	public type tipoEnum;
    [ListDrawerSettings(Expanded = true,IsReadOnly = true)]
    public List<ScriptablePokemon> evolutions = new List<ScriptablePokemon>();
    [HideInInspector] public List<int> evolutionsID = new List<int>();

    public static int MaxHP;
    public static int MaxHPId;
    public static int MaxATK;
    public static int MaxATKId;
    public static int MaxDEF;
    public static int MaxDEFId;
    public static int MaxSPD;
    public static int MaxSPDId;
    public static int MaxSAT;
    public static int MaxSATId;
    public static int MaxSDF;
    public static int MaxSDFId;
    public static int BestPokemon;
    public static int BestPokemonID;

    private Color GetColor(int value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}

public enum type
{
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