#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ElementalWeaknessChart))]
public class ElementalWeaknessChartInspector : Editor
{
    private ElementalWeaknessChart m_TargetChart;

    private Dictionary<ElementSelectionSpell, bool> m_IsDataExpanded = new Dictionary<ElementSelectionSpell, bool>();

    private void OnEnable()
    {
        m_TargetChart = target as ElementalWeaknessChart;

        initAttackers();
    }

    /// <summary>
    /// If any elements are found in the project that aren't already part of the table as attackers, we'll automatically add them.
    /// </summary>
    private void initAttackers()
    {
        foreach (string elementAssetGuid in AssetDatabase.FindAssets("t:ElementSelectionSpell"))
        {
            ElementSelectionSpell element = AssetDatabase.LoadAssetAtPath<ElementSelectionSpell>(AssetDatabase.GUIDToAssetPath(elementAssetGuid));

            if (!m_TargetChart.ElementalWeaknesses.ContainsKey(element))
            {
                Undo.RecordObject(m_TargetChart, "Added Attacking Element");
                m_TargetChart.ElementalWeaknesses.Add(element, new ElementList());
            }

            m_IsDataExpanded.Add(element, false);
        }
    }

    public override void OnInspectorGUI()
    {
        drawAttackingElements();
    }

    private void drawAttackingElements()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Weakness to:", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.indentLevel++;

            foreach (ElementSelectionSpell attackingElement in m_TargetChart.ElementalWeaknesses.Keys)
            {
                if (m_IsDataExpanded[attackingElement] = EditorGUILayout.Foldout(m_IsDataExpanded[attackingElement], attackingElement.ElementName))
                {
                    drawAttackedElementsFor(attackingElement);
                }
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
    }

    private void drawAttackedElementsFor(ElementSelectionSpell attackingElement)
    {
        SerializedProperty weaknessToListProperty = m_TargetChart.GetWeaknessesToSerializedProperty(attackingElement);
        SerializedProperty strongAgainstListProperty = m_TargetChart.GetStrongAgainstSerializedProperty(attackingElement);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUI.indentLevel++;

            //Draws the list of elements weak to this attacking type.
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(weaknessToListProperty,
                                              new GUIContent($"Elements Weak to {attackingElement.ElementName}"),
                                              includeChildren: true);
            }
            //Since the list we're working on is the serialization assistant list (Since dictionaries can't be serialized or draw in inspector)
            //If the list was changed, we need to deliver the changes back to the dictionary.
            if (EditorGUI.EndChangeCheck())
            {
                weaknessToListProperty.serializedObject.ApplyModifiedProperties();
            }

            //Draws the list of elements strong against this element type.
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(strongAgainstListProperty,
                                              new GUIContent($"Elements Strong Against {attackingElement.ElementName}"),
                                              includeChildren: true);
            }
            //Since the list we're working on is the serialization assistant list (Since dictionaries can't be serialized or draw in inspector)
            //If the list was changed, we need to deliver the changes back to the dictionary.
            if (EditorGUI.EndChangeCheck())
            {
                //Since the list we use as a serialization assistant is the one detailing elements weak to the given element, and not strong against it, we need to reconstruct that list before serialization.
                m_TargetChart.ApplyChangesFromStrongAgainstList(strongAgainstListProperty);
            }

            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
    }
}

public static class ElementalWeaknessChartExtensions
{
    /// <summary>
    /// Gets the SerializedProperty of the ElementList representing the types that i_AttackingElement is strong against.
    /// </summary>
    public static SerializedProperty GetStrongAgainstSerializedProperty(this ElementalWeaknessChart i_WeaknessChart, ElementSelectionSpell i_AttackingElement)
    {
        return i_WeaknessChart.getInnerElementListName(i_AttackingElement, i_ListFieldName: "m_ElementsIndexWeakToValues");
    }

    /// <summary>
    /// Gets the SerializedProperty of the ElementList representing the types that are weak to i_AttackingElement.
    /// </summary>
    public static SerializedProperty GetWeaknessesToSerializedProperty(this ElementalWeaknessChart i_WeaknessChart, ElementSelectionSpell i_AttackingElement)
    {
        return i_WeaknessChart.getInnerElementListName(i_AttackingElement, i_ListFieldName: "m_ElementsWeakToIndexValues");
    }

    private static SerializedProperty getInnerElementListName(this ElementalWeaknessChart i_WeaknessChart, ElementSelectionSpell i_AttackingElement, string i_ListFieldName)
    {
        SerializedObject serializedChart = new SerializedObject(i_WeaknessChart);
        SerializedProperty targetElementListProperty = null;

        //This fetches the serialization assistant objects that holds a list elements and a list of ElementList, 
        //Each ElementList represents the elements that are weak to the element in the corresponding index of the first list.
        SerializedProperty elementWeaknessKeys = serializedChart.FindProperty("m_ElementalWeaknessKeys");
        SerializedProperty elementWeaknessValues = serializedChart.FindProperty(i_ListFieldName);

        //Out of these list, we need to find the element that matches the attacking element, and return the ElementList corresponding to the same index in the other list.
        for (int i = 0; i < elementWeaknessKeys.arraySize; ++i)
        {
            ElementSelectionSpell elementAtIndex = elementWeaknessKeys.GetArrayElementAtIndex(i).objectReferenceValue as ElementSelectionSpell;

            if (elementAtIndex.Equals(i_AttackingElement))
            {
                targetElementListProperty = elementWeaknessValues.GetArrayElementAtIndex(i)?.FindPropertyRelative("Elements");
                break;
            }
        }

        return targetElementListProperty;
    }
}
#endif