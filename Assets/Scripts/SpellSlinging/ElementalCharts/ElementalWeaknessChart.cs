using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Elemental Weakness Chart", menuName = "Game/Elemental Weakness Chart", order = 0)]
public class ElementalWeaknessChart : ScriptableObject, ISerializationCallbackReceiver
{
    public Dictionary<ElementSelectionSpell, ElementList> ElementalWeaknesses { get; private set; } = new Dictionary<ElementSelectionSpell, ElementList>();

    [EditorBrowsable(EditorBrowsableState.Never)]
    [SerializeField]
    private List<ElementSelectionSpell> m_ElementalWeaknessKeys = new List<ElementSelectionSpell>();
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SerializeField]
    private List<ElementList> m_ElementsWeakToIndexValues = new List<ElementList>();

    /// <summary>
    /// This isn't technically used to serialize the dictionary, but rather used to help us show to opposite weakness route in the inspector.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SerializeField]
    private List<ElementList> m_ElementsIndexWeakToValues = new List<ElementList>();
    /// <summary>
    /// If false, the m_ElementsIndexWeakToValues won't be serialized in OnBeforeSerialize.
    /// </summary>
    private bool m_ShouldSerializeStrongAgainstList = true;

    public ElementList GetWeaknessesTo(ElementSelectionSpell i_Element)
    {
        return ElementalWeaknesses.ContainsKey(i_Element) ? ElementalWeaknesses[i_Element] : new ElementList();
    }

    public bool CheckWeakness(ElementSelectionSpell i_AttackedElement, ElementSelectionSpell i_AttackingElement)
    {
        return GetWeaknessesTo(i_AttackedElement).Contains(i_AttackingElement);
    }

    public void OnBeforeSerialize()
    {
        m_ElementalWeaknessKeys = ElementalWeaknesses.Select(kvp => kvp.Key).ToList();
        m_ElementsWeakToIndexValues = ElementalWeaknesses.Select(kvp => kvp.Value).ToList();

        if (m_ShouldSerializeStrongAgainstList)
        {
            //Creates an opposite list of weakness, detailing the elements that are strong against each type in the given index.
            m_ElementsIndexWeakToValues = new List<ElementList>();
            for (int i = 0; i < m_ElementalWeaknessKeys.Count; ++i)
            {
                m_ElementsIndexWeakToValues.Add(new ElementList());

                for (int j = 0; j < m_ElementalWeaknessKeys.Count; ++j)
                {
                    if (CheckWeakness(m_ElementalWeaknessKeys[j], m_ElementalWeaknessKeys[i]))
                    {
                        m_ElementsIndexWeakToValues[i].Elements.Add(m_ElementalWeaknessKeys[j]);
                    }
                }
            }
        }
    }

    public void OnAfterDeserialize()
    {
        ElementalWeaknesses = m_ElementalWeaknessKeys.Zip(m_ElementsWeakToIndexValues, (key, val) => new KeyValuePair<ElementSelectionSpell, ElementList>(key, val))
                                                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

#if UNITY_EDITOR
    /// <summary>
    /// Reconstructs the elemental weakness list from the opposite list of elements that are strong against each element.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void ApplyChangesFromStrongAgainstList(UnityEditor.SerializedProperty i_StrongAgainstListSerializedProperty)
    {
        m_ShouldSerializeStrongAgainstList = false;
        i_StrongAgainstListSerializedProperty.serializedObject.ApplyModifiedProperties();

        m_ElementsWeakToIndexValues = new List<ElementList>();
        for (int i = 0; i < m_ElementalWeaknessKeys.Count; ++i)
        {
            m_ElementsWeakToIndexValues.Add(new ElementList());

            for (int j = 0; j < m_ElementalWeaknessKeys.Count; ++j)
            {
                if (m_ElementsIndexWeakToValues[j].Contains(m_ElementalWeaknessKeys[i]))
                {
                    m_ElementsWeakToIndexValues[i].Elements.Add(m_ElementalWeaknessKeys[j]);
                }
            }
        }

        OnAfterDeserialize();
        i_StrongAgainstListSerializedProperty.serializedObject.ApplyModifiedProperties();
        m_ShouldSerializeStrongAgainstList = true;
    }
#endif
}
