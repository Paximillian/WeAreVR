using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spellbook.asset", menuName = "Game/Spellbook", order = 0)]
public class Spellbook : ScriptableObject
{
    [SerializeField]
    private List<SpellCombo> Spells = new List<SpellCombo>();

    public Spell GetSpell(ElementSelectionSpell i_Element, FormSelectionSpell i_SpellToCombine)
    {
        foreach (SpellCombo combo in Spells)
        {
            if (combo.ElementSpell == i_Element && combo.FormSpell == i_SpellToCombine)
            {
                return combo.ResultSpell;
            }
        }

        return null;
    }
}
