using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratedSpell : Spell
{
    protected ElementSelectionSpell SpellElement { get; private set; }

    public override void Cast(SpellWeaver i_Caster)
    {
        SpellElement = i_Caster.CurrentElement;
    }
}
