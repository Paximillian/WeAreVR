using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirElementSpell : ElementSelectionSpell
{
    [SerializeField]
    private Material m_AirElementMaterial;

    public override void Cast(SpellWeaver i_Caster)
    {
        foreach (Renderer renderer in i_Caster.GetComponentsInChildren<Renderer>())
        {
            renderer.material = m_AirElementMaterial;
        }
    }
}