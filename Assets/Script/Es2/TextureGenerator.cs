using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor { }

public class TextureGenerator : EditorWindow {
	private Sprite border;
	private Sprite main;
	private Sprite background;
	private Sprite result;
	private int numberOftext = 3;

	[MenuItem("Custom/Texture Generator")]
	private static void ShowWindow() {
		var windows = GetWindow<TextureGenerator>();
		windows.titleContent = new GUIContent("TextureGenerator");
		windows.Show();
	}

	// Update is called once per frame
	void OnGUI() {
		var rectMetà = new Rect(0, 0, position.width / 3, position.height);
		var rectSinistra1 = new Rect(rectMetà.position.x, rectMetà.position.y, rectMetà.width, rectMetà.height / numberOftext);
		var rectSinistra2 = new Rect(rectSinistra1.position.x, rectSinistra1.height, rectMetà.width, rectMetà.height / numberOftext);
		var rectSinistra3 = new Rect(rectSinistra2.position.x, rectSinistra2.height * 2, rectMetà.width, rectMetà.height / numberOftext);
		var rectMetaMetaSinistra1 = new Rect(rectSinistra1.position.x, rectSinistra1.position.y, rectMetà.width, rectSinistra1.height * 0.6f);
		var rectMetaMetaSinistra12 = new Rect(rectSinistra1.position.x, rectMetaMetaSinistra1.height, rectMetà.width, rectSinistra1.height * 0.3f);
		var rectMetaMetaSinistra2 = new Rect(rectSinistra2.position.x, rectSinistra2.position.y, rectMetà.width, rectSinistra2.height * 0.6f);
		var rectMetaMetaSinistra22 = new Rect(rectSinistra2.position.x, rectMetaMetaSinistra2.height+rectMetaMetaSinistra2.position.y, rectMetà.width, rectSinistra2.height * 0.3f);
		var rectMetaMetaSinistra3 = new Rect(rectSinistra3.position.x, rectSinistra3.position.y, rectMetà.width, rectSinistra3.height * 0.6f);
		var rectMetaMetaSinistra32 = new Rect(rectSinistra3.position.x, rectMetaMetaSinistra3.height+rectMetaMetaSinistra3.position.y, rectMetà.width, rectSinistra3.height * 0.3f);
		var rectMetaDestra = new Rect((position.width/3)+70,0,position.width*0.6f,position.height);
		var rectMetaDestra1 = new Rect(rectMetaDestra.position.x, rectMetaDestra.position.y,rectMetaDestra.width,rectMetaDestra.height*0.67f);
		var rectMetaDestra2 = new Rect(rectMetaDestra.position.x, rectMetaDestra1.height,rectMetaDestra.width,rectMetaDestra.height*0.3f);
		var rectmetadDestra21 = new Rect(rectMetaDestra2.position.x,rectMetaDestra2.position.y,rectMetaDestra2.width/2,rectMetaDestra2.height);
		var rectmetadDestra22 = new Rect(rectmetadDestra21.width+rectmetadDestra21.position.x,rectMetaDestra2.position.y,rectMetaDestra2.width/2,rectMetaDestra2.height);
	//creo text, cambio i pixel in base a backround poi coso poi coso
		background = EditorGUI.ObjectField(rectMetaMetaSinistra1, background, typeof(Sprite), false) as Sprite;
		border = EditorGUI.ObjectField(rectMetaMetaSinistra2, border, typeof(Sprite), false) as Sprite;
		main = EditorGUI.ObjectField(rectMetaMetaSinistra3, main, typeof(Sprite), false) as Sprite;
		if (GUI.Button(rectMetaMetaSinistra12, "clear")) {
			background = null;
		}
		if (GUI.Button(rectMetaMetaSinistra22, "clear")) {
			border = null;
		}
		if (GUI.Button(rectMetaMetaSinistra32, "clear")) {
			main = null;
		}
		if (GUI.Button(rectmetadDestra21,"clear all")) {
			main = null;
			border = null;
			background = null;
			
		}
		if (GUI.Button(rectmetadDestra22,"save")) {
			// var activeWindow = focusedWindow;
			// var colors = InternalEditorUtility.ReadScreenPixel(new Vector2(activeWindow.position.x+rectMetaDestra1.position.x,activeWindow.position.y+rectMetaDestra1.position.y),(int)rectMetaDestra1.width, (int)rectMetaDestra1.height);
			// var result = new Texture2D((int) rectMetaDestra1.width, (int) rectMetaDestra1.height);
			// result.SetPixels(colors);
			// result.Apply();
			// string path = EditorUtility.SaveFilePanel("Save Image", "Assets/sprite", "pippo","png");
			// var bytes = result.EncodeToPNG();
			// Object.DestroyImmediate(result);
			// File.WriteAllBytes(Application.dataPath+"/sprite/newSprite.png",bytes);
			// AssetDatabase.Refresh();
			
			Color[] borderPixels = border.texture.GetPixels();
			Color[] backgroundPixels = background.texture.GetPixels();
			Color[] mainPixels = main.texture.GetPixels();
			result = background;
			int k = 0;
			for (int i = 0; i < border.texture.width; i++) {
				for (int j = 0; j < border.texture.height; j++) {
					result.texture.SetPixel(i,j,borderPixels[k]);
					k++;
				}
			}
			result.texture.Apply();
			k = 0;
			for (int i = 0; i < main.texture.width; i++) {
				for (int j = 0; j < main.texture.height; j++) {
					result.texture.SetPixel(i,j,mainPixels[k]);
					k++;
				}
			}
			result.texture.Apply();
			var bytes = result.texture.EncodeToPNG();
			string path = EditorUtility.SaveFilePanel("Save Image", "Assets/sprite", "pippo","png");
			File.WriteAllBytes(path,bytes);
		}
		if (background!=null) {
			DrawSprite(rectMetaDestra1,background);
		}
		if (border!=null) {
			DrawSprite(rectMetaDestra1,border);
		}
		if (main!=null) {
			DrawSprite(rectMetaDestra1,main);
		}
		
	}
	private void DrawSprite(Rect rect, Sprite sprite) {
		GUI.DrawTextureWithTexCoords(rect, sprite.texture, 
			new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, 
				sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height), true);
	}
}