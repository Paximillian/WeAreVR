using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Enemy m_Enemy;

    private void Awake()
    {
        StartCoroutine(spawn());
    }

    private IEnumerator spawn()
    {
        while (true)
        {
            Enemy enemy = Instantiate(m_Enemy, transform.position, Quaternion.identity);
            enemy.transform.LookAt(Camera.main.transform);
            enemy.transform.forward = Vector3.ProjectOnPlane(enemy.transform.forward, Vector3.up);
            yield return new WaitForSeconds(5);
        }
    }
}
