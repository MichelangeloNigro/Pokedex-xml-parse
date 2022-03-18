using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Recipe.Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return 70;
	}
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		
		SerializedProperty itemProperty = property.FindPropertyRelative("item");
		Item item = itemProperty.objectReferenceValue as Item;
		Sprite sprite = null;
		if (item) {
			if (item.sprite) {
				sprite = item.sprite;
			}
		}
		SerializedProperty amountProperty = property.FindPropertyRelative("amount");

		var itemRect = new Rect(position.x, position.y, position.width / 2.5f, position.height);
		var spriteRect = new Rect(position.x + position.width / 2.5f + 20, position.y + 10, 50, 50);
		float amountX = position.x + position.width / 2.5f + 90;
		var amountRect = new Rect(amountX, position.y, position.width / 3, position.height);
		float buttonX = amountX + 30;
		var buttonUpRect = new Rect(buttonX, position.y + 2, 30, 30);
		var buttonDownRect = new Rect(buttonX, position.y + 35, 30, 30);
		
		EditorGUI.PropertyField(itemRect, itemProperty, GUIContent.none);
		if (sprite) {
			DrawSprite(spriteRect, sprite);
		}
		GUI.Label(amountRect, "x" + amountProperty.intValue);
		
		if (GUI.Button(buttonUpRect, "+")) {
			amountProperty.intValue++;
		}
		if (GUI.Button(buttonDownRect, "-")) {
			amountProperty.intValue--;
		}

		EditorGUI.EndProperty();
	}
	
	private void DrawSprite(Rect rect, Sprite sprite) {
		GUI.DrawTextureWithTexCoords(rect, sprite.texture, 
			new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, 
				sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height), true);
	}
}
