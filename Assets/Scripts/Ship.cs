using Assets.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipTier shipLevel;
    [SerializeField] private shipTeam shipTeam;
    public List<Transform> decks;
    [SerializeField] private bool isPlaced;
    public bool isHorizontal;
    public short health;



    // Start is called before the first frame update
    void Start()
    {
        decks = GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
