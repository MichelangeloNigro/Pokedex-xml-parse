using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Type_", menuName = "Pokemon/Create a Type")]
public class Type : ScriptableObject {

    public string name;
    public Color color;
    [PreviewField]
    public Sprite sprite;
    public List<Type> weakAgainst=new List<Type>();
    public List<Type> strongAgainst=new List<Type>();
    public List<Type> normalEffectivness=new List<Type>();
    public List<Type> notEffective=new List<Type>();

}
