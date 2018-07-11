using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ElementalWeaknessChart m_ElementalWeaknessChart;

    [SerializeField]
    private ElementSelectionSpell m_Element;
    
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        ElementSelectionSpell damagingSpellElement = other.GetComponent<DamagingSpell>()?.Element;

        if (damagingSpellElement)
        {
            if (m_ElementalWeaknessChart.CheckWeakness(m_Element, damagingSpellElement))
            {
                m_Animator.SetTrigger("Death");
            }
        }
    }
}
