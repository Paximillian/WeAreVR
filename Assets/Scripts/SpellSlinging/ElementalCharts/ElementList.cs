using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ElementList
{
    [SerializeField]
    public List<ElementSelectionSpell> Elements = new List<ElementSelectionSpell>();

    public ElementList(IEnumerable<ElementSelectionSpell> i_ElementList = null)
    {
        Elements = i_ElementList?.ToList() ?? new List<ElementSelectionSpell>();
    }

    public bool Contains(ElementSelectionSpell i_Element) => Elements.Contains(i_Element);
}