using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementSpell : ElementSelectionSpell
{
    [SerializeField]
    private Material m_FireElementMaterial;

    public override void Cast(SpellWeaver i_Caster)
    {
        foreach (Renderer renderer in i_Caster.GetComponentsInChildren<Renderer>())
        {
            renderer.material = m_FireElementMaterial;
        }
    }
}
