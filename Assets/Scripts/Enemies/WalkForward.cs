using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkForward : MonoBehaviour
{
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }
}
