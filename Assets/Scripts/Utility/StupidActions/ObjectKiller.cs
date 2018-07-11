using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKiller : MonoBehaviour
{
    public void KillMe()
    {
        Destroy(transform.root.gameObject);
    }
}