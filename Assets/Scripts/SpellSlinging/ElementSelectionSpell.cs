using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementSelectionSpell : Spell
{
    public string ElementName { get { return name.Replace("Spell", "").Replace("Element", ""); } }
}
