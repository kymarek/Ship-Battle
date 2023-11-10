using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvailiableShips : MonoBehaviour
{
    [SerializeField] public List<Transform> ships;
    // Start is called before the first frame update
    void Start()
    {
        ships = gameObject.GetComponentsInChildren<Transform>()
            .Skip(1)
            .Where(x => x.tag == "Ship")
            .ToList<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
