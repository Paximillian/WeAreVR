using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlashSpell : GeneratedSpell
{
    public override void Cast(SpellWeaver i_Caster)
    {
        base.Cast(i_Caster);

        GameObject bubble = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bubble.transform.position = i_Caster.transform.position;
        bubble.transform.localScale = new Vector3(0.5f, 0.01f, 0.2f);
        bubble.GetComponent<Renderer>().material = i_Caster.GetComponentInChildren<Renderer>().material;
        Rigidbody bubbleBody = bubble.AddComponent<Rigidbody>();
        bubbleBody.useGravity = false;
        bubbleBody.velocity = Camera.main.transform.forward * 6;
        bubble.GetComponent<Collider>().isTrigger = true;
        bubble.AddComponent<DamagingSpell>().Element = i_Caster.CurrentElement;
    }
}