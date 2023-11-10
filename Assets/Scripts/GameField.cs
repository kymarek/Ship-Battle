using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using Assets.Enums;

public class GameField : MonoBehaviour
{

    public Transform[] cells;
    public shipTeam team;
    // Start is called before the first frame update
    void Start()
    {
        cells = gameObject.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        for (int i = 0; i < 100; i++)
        {
            cells[i].name = $"WaterCube {i}";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
