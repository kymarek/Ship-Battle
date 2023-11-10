using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Enums;
using System.Linq;

public class FieldProperty : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isDamaged;
    public bool isBusy;
    public List<Transform> _adjacentCells;
    [SerializeField] private GameField _allyField;
    public short row, col;
    [SerializeField] private GameProcess _gameProcess;
    private EnemyTurn _enemyTurn;


    void Start()
    {
        _enemyTurn = Camera.main.GetComponent<EnemyTurn>();
        _gameProcess = Camera.main.GetComponent<GameProcess>();
        _allyField = gameObject.GetComponentInParent<GameField>();
        row = GetRow(transform.localPosition.z);
        col = GetCol(transform.localPosition.x);
        switch (row)
          {
              case 1:
                  for (int i = 0; i < 20; i++)
                  {
                      if ((Vector3.Distance(transform.position, _allyField.cells[i].position) < 3) &&
                          (Vector3.Distance(transform.localPosition, _allyField.cells[i].localPosition) != 0))
                    {
                          _adjacentCells.Add(_allyField.cells[i]);
                      }
                  }
                  break;


            case >= 2 and <= 9:
                  adjacentCells(row);
                  break;


            case 10:
                  for (int i = 79; i < 100; i++)
                  {
                      if ((Vector3.Distance(transform.localPosition, _allyField.cells[i].localPosition) < 3) &&
                          (Vector3.Distance(transform.localPosition, _allyField.cells[i].localPosition) != 0))
                      {
                          _adjacentCells.Add(_allyField.cells[i]);
                      }
                  }
                  break;
          };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private short GetRow(float _z) 
    {
        return _z switch
        {
            >= 2 => 10,
            >= 0 => 9,
            >= -2 => 8,
            >= -4 => 7,
            >= -6 => 6,
            >= -8 => 5,
            >= -10 => 4,
            >= -12 => 3,
            >= -14 => 2,
            >= -16 => 1,
            _ => 0
        };
    }

    private short GetCol(float _x)
    {
        return _x switch
        {
            >= 3 => 10,
            >= 1 => 9,
            >= -1 => 8,
            >= -3 => 7,
            >= -5 => 6,
            >= -7 => 5,
            >= -9 => 4,
            >= -11 => 3,
            >= -13 => 2,
            >= -15 => 1,
            _ => 0
        };
    }

    private void adjacentCells(short _currRow)
    {
        for (int i = (_currRow-1) * 10 - 10; i < (_currRow-1) * 10 + 20; i++)
        {
            if ((Vector3.Distance(transform.localPosition, _allyField.cells[i].localPosition) < 3) && 
                (Vector3.Distance(transform.localPosition, _allyField.cells[i].localPosition) != 0)) 
            {
                _adjacentCells.Add(_allyField.cells[i]);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (_allyField.team == shipTeam.enemy)
            {
                gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.red);
                collision.transform.position = new Vector3(-23, 2, -15);
                collision.rigidbody.velocity = Vector3.zero;
                isDamaged = true;
                _gameProcess.gameState = GameState.EnemyTurn;
            }

            if (_allyField.team == shipTeam.ally)
            {
                gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
                collision.transform.position = new Vector3(-23, 2, -15);
                collision.rigidbody.velocity = Vector3.zero;
                isDamaged = true;
                _gameProcess.gameState = GameState.PlayerTurn;
                //Camera.main.GetComponent<EnemyTurn>().aliveCells.Remove(transform);
                _enemyTurn.aliveCells = _enemyTurn.aliveCells.Where(x => !x.GetComponent<FieldProperty>().isDamaged).ToList();
                //_enemyTurn.wasHitten = transform;
            }
        }



        if (collision.gameObject.layer == 3)
        {
           Destroy(collision.gameObject.GetComponent<Rigidbody>());
        }
    }
}
