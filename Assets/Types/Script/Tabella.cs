
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;

namespace UnityEditor
{
    public class Tabella : EditorWindow
    {
       
        private string pathToSearch;
        private Type[] allTypes;

        [MenuItem("Custom/SceneNavigator")] 
        private static void ShowWindow()
        {
            var windows = GetWindow<Tabella>();
            windows.titleContent = new GUIContent("Navigator");
            windows.Show();
        }

        private void OnGUI()
        {
            pathToSearch = GUILayout.TextField(pathToSearch);
            allTypes = GetAllInstances<Type>();
            foreach (var VARIABLE in allTypes) {
                Debug.Log(VARIABLE.name);
            }



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

