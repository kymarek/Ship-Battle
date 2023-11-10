using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public bool isAlive = true;
    public GameObject? stayAt;
    [SerializeField] private GameProcess _gameProcess;
    [SerializeField] private GameField _allyField;
    private EnemyTurn _enemyTurn;

    // Start is called before the first frame update
    void Start()
    {
        _enemyTurn = Camera.main.GetComponent<EnemyTurn>();
        _gameProcess = Camera.main.GetComponent<GameProcess>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (_allyField.team == shipTeam.enemy)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.red);
                collision.transform.position = new Vector3(-23, 2, -15);
                collision.rigidbody.velocity = Vector3.zero;
                isAlive = false;
                stayAt.GetComponent<FieldProperty>().isDamaged = true;
                stayAt.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.red);
                _gameProcess.gameState = GameState.PlayerTurn;
                GetComponentInParent<Ship>().health--;
                if (GetComponentInParent<Ship>().health == 0)
                {
                    foreach (Transform deck in GetComponentInParent<Ship>().decks)
                    {
                        foreach (Transform neighbor in deck.GetComponent<Deck>().stayAt.GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbor.GetComponent<FieldProperty>().isDamaged = true;
                            neighbor.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.red);
                        }
                    }
                }
            }

            if (_allyField.team == shipTeam.ally)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.green);
                collision.transform.position = new Vector3(-23, 2, -15);
                collision.rigidbody.velocity = Vector3.zero;
                isAlive = false;
                stayAt.GetComponent<FieldProperty>().isDamaged = true;
                stayAt.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
                _gameProcess.gameState = GameState.EnemyTurn;


                GetComponentInParent<Ship>().health--;
                if (GetComponentInParent<Ship>().health == 0)
                {
                    foreach (Transform deck in GetComponentInParent<Ship>().decks)
                    {
                        foreach (Transform neighbor in deck.GetComponent<Deck>().stayAt.GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbor.GetComponent<FieldProperty>().isDamaged = true;
                            neighbor.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
                        }
                    }
                }
                _enemyTurn.aliveCells = _enemyTurn.aliveCells.Where(x => !x.GetComponent<FieldProperty>().isDamaged).ToList();
                if (GetComponentInParent<Ship>().health != 0)
                {
                    _enemyTurn.isHit = true;
                    if ((GetComponentInParent<Ship>().shipLevel == ShipTier.fourDeck ||
                        GetComponentInParent<Ship>().shipLevel == ShipTier.threeDeck) &&
                        (_enemyTurn.wasHittenShip == GetComponentInParent<Transform>().gameObject))
                    {
                        List<Transform> _availiableTargets = GetComponentInParent<Ship>().decks.Where(x => x.GetComponent<Deck>().isAlive).ToList();
                        _enemyTurn.targetToShoot = _availiableTargets[GetRandomIntNumber(_availiableTargets.Count - 1)];
                    }
                    _enemyTurn.wasHitten = stayAt.transform;
                    _enemyTurn.wasHittenShip = GetComponentInParent<Transform>().gameObject;  
                }
                else
                {
                    _enemyTurn.targetToShoot = null;
                    _enemyTurn.isHit = false;
                }
                
                
            }
        }
    }

    private int GetRandomIntNumber(int max)
    {
        float rand = UnityEngine.Random.RandomRange(0, max);
        return (int)rand;
    }
}
