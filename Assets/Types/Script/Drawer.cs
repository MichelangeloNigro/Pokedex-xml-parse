using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Type))]
public class Drawer : PropertyDrawer {

	// public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
	// 	const int amountOfVariables = 2;
	// 	float spaceForVariable = position.width / amountOfVariables;
	// 	var scriptableRect = new Rect(position.x, position.y, spaceForVariable, position.height);
	// 	var spriteRect = new Rect(scriptableRect.position.x + spaceForVariable, position.y, spaceForVariable, position.height);
	// 	var amountRect = new Rect(spriteRect.position.x + spaceForVariable, position.y, 50, 50);
	// 	var buttonsRect = new Rect(amountRect.position.x + spaceForVariable, position.y, spaceForVariable, position.height);
	//
	// 	SerializedProperty type = property.FindPropertyRelative("type");
	// 	EditorGUI.ObjectField(scriptableRect, type, typeof(Type), label);
	// 	if (type.objectReferenceValue != null) {
	// 		var item = new SerializedObject(type.objectReferenceValue);
	//
	// 		SerializedProperty ingredientSprite = item.FindProperty("sprite");
	// 		SerializedProperty ingredientName = item.FindProperty("name");
	// 		//GUI.Button(buttonsRect, "button");
	// 		// GUI.enabled = false;
	// 		if (ingredientSprite != null) {
	// 			var sprite = ingredientSprite.objectReferenceValue as Sprite;
	// 			DrawSprite(spriteRect, sprite);
	// 		}
	// 		if (ingredientName!=null) {
	// 			var name = ingredientName.stringValue;
	// 			GUI.Label(amountRect,name);
	// 		}
	// 		// GUI.enabled = true;
	// 	}
	// 	EditorGUI.EndProperty();
	// }
	//
	// private void DrawSprite(Rect rect, Sprite sprite) {
	// 	Texture tex = sprite.texture;
	// 	var previewRect = new Rect(sprite.rect.x / tex.width, sprite.rect.y / tex.height, sprite.rect.width / tex.width, sprite.rect.height / tex.height);
	// 	GUI.DrawTextureWithTexCoords(rect, tex, previewRect, true);
	// }
	//
	// public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
	// 	return 128;
	// }
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return 70;
		//larghezza elementi
	}
	
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		
		
		float typex = position.x;
		var rectSprite = new Rect(typex+20,position.y,position.width/3,position.height);
		float textX = typex + (position.width / 3) +20;
		var rectText= new Rect(textX,position.y,position.width/2,position.height);
		//EditorGUI.PropertyField(rect,property,GUIContent.none);
		Type questo=property.objectReferenceValue as Type;
		var sprite = questo.sprite;
		GUI.DrawTexture(rectSprite,sprite.texture);
		var centeredBoldStyleRed = new GUIStyle(GUI.skin.label)
		{
			alignment = TextAnchor.MiddleCenter,
			fontSize = 20,
			wordWrap = true,
			fontStyle = FontStyle.Bold,
			normal = new GUIStyleState
			{
				textColor = questo.color
			}
		};
		GUI.Label(rectText,questo.name,centeredBoldStyleRed);

		// recupera i dati serializzati, se errore, non seriallizzato
		// SerializedProperty spriteProperty = property.FindPropertyRelative("sprite");
		// Sprite sprite=spriteProperty.objectReferenceValue as Sprite;
		// GUI.DrawTexture(rect,sprite.texture);
	}
	
	// private void DrawSprite(Rect rect, Sprite sprite) {
	// 	GUI.DrawTextureWithTexCoords(rect, sprite.texture, 
	// 		new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, 
	// 			sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height), true);
	// }
}