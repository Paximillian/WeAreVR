using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elemental Weakness Chart", menuName = "Game/Elemental Weakness Chart", order = 0)]
public class ElementalWeaknessChart : ScriptableObject
{
    private Dictionary<ElementSelectionSpell, List<ElementSelectionSpell>> m_ElementWeaknesses = new Dictionary<ElementSelectionSpell, List<ElementSelectionSpell>>();

    public List<ElementSelectionSpell> GetWeaknesses(ElementSelectionSpell i_Element)
    {
        return m_ElementWeaknesses.ContainsKey(i_Element) ? m_ElementWeaknesses[i_Element] : new List<ElementSelectionSpell>();
    }

    public bool CheckWeakness(ElementSelectionSpell i_AttackedElement, ElementSelectionSpell i_AttackingElement)
    {
        //return GetWeaknesses(i_AttackedElement).Contains(i_AttackingElement);
        return true;
    }
}
