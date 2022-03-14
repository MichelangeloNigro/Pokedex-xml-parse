using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;

namespace UnityEditor {

	public class Tabella : EditorWindow {

		private string pathToSearch;
		private Type[] allTypes;

		[MenuItem("Custom/TypeTable")]
		private static void ShowWindow() {
			var windows = GetWindow<Tabella>();
			windows.titleContent = new GUIContent("TypeTable");
			windows.Show();
		}

		private void OnGUI() {
			var centeredBoldStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontSize = 14,
				wordWrap = true,
				fontStyle = FontStyle.Bold
			};
			var centeredBoldStyleRed = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontSize = 14,
				wordWrap = true,
				fontStyle = FontStyle.Bold,
				normal = new GUIStyleState
				{
					textColor = Color.red
				}
			};
			var centeredBoldStyleGreen = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontSize = 14,
				wordWrap = true,
				fontStyle = FontStyle.Bold,
				normal = new GUIStyleState
				{
					textColor = Color.green
				}
			};

			pathToSearch = GUILayout.TextField(pathToSearch);
			allTypes = GetAllInstances<Type>();
			float columnPosition = position.width / (allTypes.Length + 1);
			float rowPosition = position.height / (allTypes.Length + 1);
			for (int i = 0; i < allTypes.Length; i++) {
				var rowCell = new Rect(0, (i + 1) * rowPosition, columnPosition, rowPosition);
				var columnCell = new Rect((i + 1) * columnPosition, 0, columnPosition, rowPosition);
				var tex=new Texture2D(2,2);
				SetColor(tex,allTypes[i].color);
				centeredBoldStyle.normal.background=tex;
				EditorGUI.LabelField(rowCell, allTypes[i].name, centeredBoldStyle);
				EditorGUI.LabelField(columnCell, allTypes[i].name, centeredBoldStyle);
				for (int j = 0; j < allTypes.Length; j++) {
					Rect tableCell = new Rect((j + 1) * columnPosition, (i + 1) * rowPosition, columnPosition, rowPosition);
					Debug.Log(allTypes[j].name);
					if (allTypes[i].strongAgainst.Contains(allTypes[j]))
					{
						EditorGUI.LabelField(tableCell, $"X 2", centeredBoldStyleGreen);
					}
					else if (allTypes[i].weakAgainst.Contains(allTypes[j]))
					{
						EditorGUI.LabelField(tableCell, $"X 0.5", centeredBoldStyleRed);
					}
					else if (allTypes[i].notEffective.Contains(allTypes[j]))
					{
						EditorGUI.LabelField(tableCell, $"X 0", centeredBoldStyleRed);
					}
					else
					{
						centeredBoldStyle.normal.background=GUI.skin.label.normal.background;
						EditorGUI.LabelField(tableCell, "X 1", centeredBoldStyle);
					}
					
					// if (VARIABLE.notEffective.Contains(type)) {
					// 	Debug.Log($"{VARIABLE} is not effective against {type}");
					// }
					// else if (VARIABLE.weakAgainst.Contains(type)) {
					// 	Debug.Log($"{VARIABLE} is weak against {type}");
					// }
					// else if (VARIABLE.normalEffectivness.Contains(type)) {
					// 	Debug.Log($"{VARIABLE} is normal against {type}");
					// }
					// else if (VARIABLE.strongAgainst.Contains(type)) {
					// 	Debug.Log($"{VARIABLE} is strong against {type}");
					// }
					// else {
					// 	Debug.Log($"A Relation has not ben set up with {type} and {VARIABLE}");
					// }
				}
			}
		}

		public  void SetColor(Texture2D tex2, Color32 color)
		{
			var fillColorArray = tex2.GetPixels32();

			for (var i = 0; i < fillColorArray.Length; ++i)
			{
				fillColorArray[i] = color;
			}

			tex2.SetPixels32(fillColorArray);

			tex2.Apply();
		}

		
		public static T[] GetAllInstances<T>() where T : ScriptableObject {
			string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
			var a = new T[guids.Length];
			for (int i = 0; i < guids.Length; i++) {
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
			}
			return a;
		}
	}

}


// 	private void OnGUI()
// 	{
// 		PokemonType[] types = GetAllInstances<PokemonType>();
// 		TypeDatabase[] database = GetAllInstances<TypeDatabase>();
// 		float columnPosition = position.width / (types.Length + 1);
// 		float rowPosition = position.height / (types.Length + 1);
// 		
// 		for (int i = 0; i < types.Length; i++)
// 		{
// 			var rowCell = new Rect(0, (i + 1) * rowPosition, columnPosition, rowPosition);
// 			var columnCell = new Rect((i + 1) * columnPosition, 0, columnPosition, rowPosition);
// 			var tex=new Texture2D(2,2);
// 			SetColor(tex,types[i].BackgroundColor);
// 			centeredBoldStyle.normal.background=tex;
// 			EditorGUI.LabelField(rowCell, types[i].TypeName, centeredBoldStyle);
// 			EditorGUI.LabelField(columnCell, types[i].TypeName, centeredBoldStyle);
// 			for (int j = 0; j < types.Length; j++)
// 			{
// 				Rect tableCell = new Rect((j + 1) * columnPosition, (i + 1) * rowPosition, columnPosition, rowPosition);
// 				if (types[i].StrongAgainst.Contains(types[j]))
// 				{
// 					EditorGUI.LabelField(tableCell, $"X {database[0].stongAgainstMultiplier}", centeredBoldStyleGreen);
// 				}
// 				else if (types[i].WeakAgainst.Contains(types[j]))
// 				{
// 					EditorGUI.LabelField(tableCell, $"X {database[0].weakAgainstMultiplier}", centeredBoldStyleRed);
// 				}
// 				else
// 				{
// 					centeredBoldStyle.normal.background=GUI.skin.label.normal.background;
// 					EditorGUI.LabelField(tableCell, "X 1", centeredBoldStyle);
// 				}
// 			}
// 		}
// 	}
// }
