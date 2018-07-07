using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBubbleSpell : Spell
{
    public override void Cast(SpellWeaver i_Caster)
    {
        GameObject bubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bubble.transform.position = i_Caster.transform.position;
        bubble.transform.localScale = Vector3.one * 0.1f;
        bubble.GetComponent<Renderer>().material = i_Caster.GetComponentInChildren<Renderer>().material;
        Rigidbody bubbleBody = bubble.AddComponent<Rigidbody>();
        bubbleBody.useGravity = false;
        bubbleBody.velocity = Camera.main.transform.forward;
    }
}
