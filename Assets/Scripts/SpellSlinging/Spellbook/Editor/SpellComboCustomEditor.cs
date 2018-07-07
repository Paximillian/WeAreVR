#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpellCombo))]
public class SpellComboCustomEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? base.GetPropertyHeight(property, label) * 7 + 25 : base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            //Background
            Rect backgroundPosition = position;
            backgroundPosition.x += 15;
            backgroundPosition.width -= 15;
            backgroundPosition.height -= 15;
            EditorGUI.HelpBox(backgroundPosition, "", MessageType.None);


            //Foldout
            Rect foldoutPosition = position;
            foldoutPosition.x += 15;
            foldoutPosition.width = 30;
            foldoutPosition.height = position.height / 8;
            property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, "");


            //Name
            Rect namePosition = position;
            namePosition.x += 15;
            namePosition.height = position.height / 8;
            namePosition.width -= 20;
            Undo.RecordObject(property.serializedObject.targetObject, "Changed Spell Name");
            property.FindPropertyRelative("m_SpellName").stringValue = EditorGUI.TextField(namePosition, property.FindPropertyRelative("m_SpellName").stringValue);


            //Element Spell
            Rect elementPosition = namePosition;
            elementPosition.y += namePosition.height + 10;
            elementPosition.height = position.height / 7 - 3;
            Undo.RecordObject(property.serializedObject.targetObject, "Changed Element Spell");
            EditorGUI.ObjectField(elementPosition, property.FindPropertyRelative("m_ElementSpell"));


            //+ sign
            Rect plusPosition = elementPosition;
            plusPosition.x += plusPosition.width * 3 / 5;
            plusPosition.y += elementPosition.height;
            plusPosition.height = position.height / 8;
            EditorGUI.LabelField(plusPosition, "+", EditorStyles.boldLabel);


            //Form Spell
            Rect formPosition = plusPosition;
            formPosition.x = elementPosition.x;
            formPosition.y += plusPosition.height;
            formPosition.height = position.height / 7 - 3;
            Undo.RecordObject(property.serializedObject.targetObject, "Changed Form Spell");
            EditorGUI.ObjectField(formPosition, property.FindPropertyRelative("m_FormSpell"));


            //= sign
            Rect equalsPosition = formPosition;
            equalsPosition.x += equalsPosition.width * 3 / 5;
            equalsPosition.y += formPosition.height;
            equalsPosition.height = position.height / 8;
            EditorGUI.LabelField(equalsPosition, "=", EditorStyles.boldLabel);


            //Result Spell
            Rect resultPosition = equalsPosition;
            resultPosition.x = elementPosition.x;
            resultPosition.y += equalsPosition.height;
            resultPosition.height = position.height / 7 - 3;
            Undo.RecordObject(property.serializedObject.targetObject, "Changed Result Spell");
            EditorGUI.ObjectField(resultPosition, property.FindPropertyRelative("m_ResultSpell"));
        }
        else
        {
            //Foldout
            Rect foldoutPosition = position;
            foldoutPosition.x += 15;
            foldoutPosition.height = position.height / 8;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.FindPropertyRelative("m_SpellName").stringValue);
        }
    }
}
#endif