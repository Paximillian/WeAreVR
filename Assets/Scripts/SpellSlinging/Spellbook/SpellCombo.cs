using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellCombo
{
    [SerializeField]
    private string m_SpellName;
    public string SpellName { get { return m_SpellName; } set { m_SpellName = value; } }

    [SerializeField]
    private ElementSelectionSpell m_ElementSpell;
    public ElementSelectionSpell ElementSpell { get { return m_ElementSpell; } set { m_ElementSpell = value; } }

    [SerializeField]
    private FormSelectionSpell m_FormSpell;
    public FormSelectionSpell FormSpell { get { return m_FormSpell; } set { m_FormSpell = value; } }

    [SerializeField]
    private Spell m_ResultSpell;
    public Spell ResultSpell { get { return m_ResultSpell; } set { m_ResultSpell = value; } }
}
