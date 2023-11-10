using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyTurn : MonoBehaviour
{

    public List<Transform> aliveCells;
    [SerializeField] GameObject _field;
    [SerializeField] private GameProcess _gameProcess;
    [SerializeField] private Transform _canon;
    [SerializeField] private Transform _bullet;
    [SerializeField] private float _angle;
    [SerializeField] private int _choosen;
    public bool isHit = false;
    public Transform wasHitten;
    public GameObject wasHittenShip;
    public Transform targetToShoot;


    // Start is called before the first frame update
    void Start()
    {
        _gameProcess = Camera.main.GetComponent<GameProcess>();
        aliveCells = _field.GetComponent<GameField>().cells.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            //TODO: ƒоделать выстрел противника,написать метод ShotAfrterHit
            if (targetToShoot != null)
            {
                ShootInRandomCell(_canon, targetToShoot.GetComponent<Deck>().stayAt.transform, _bullet, _angle);
            }
            else
            {
                ShootInRandomCell(_canon, ChooseNeighbor(), _bullet, _angle);
            }  
        }
        else
        { 
            ShootInRandomCell(_canon, ChooseRandomTarget(), _bullet, _angle);
        }
    }


    Transform ChooseNeighbor() 
    {
        List<Transform> _notDamagedAdjacentCells = wasHitten.GetComponent<FieldProperty>()._adjacentCells.Where(x => !x.GetComponent<FieldProperty>().isDamaged).ToList();
        List<Transform> _canBeChoosenCells = new();
        foreach (Transform obj in _notDamagedAdjacentCells) 
        {
            if (
                (obj.GetComponent<FieldProperty>().row == wasHitten.GetComponent<FieldProperty>().row + 1
                && obj.GetComponent<FieldProperty>().col == wasHitten.GetComponent<FieldProperty>().col) ||
                (obj.GetComponent<FieldProperty>().row == wasHitten.GetComponent<FieldProperty>().row - 1
                && obj.GetComponent<FieldProperty>().col == wasHitten.GetComponent<FieldProperty>().col) ||
                (obj.GetComponent<FieldProperty>().row == wasHitten.GetComponent<FieldProperty>().row
                && obj.GetComponent<FieldProperty>().col == wasHitten.GetComponent<FieldProperty>().col + 1) ||
                (obj.GetComponent<FieldProperty>().row == wasHitten.GetComponent<FieldProperty>().row
                && obj.GetComponent<FieldProperty>().col == wasHitten.GetComponent<FieldProperty>().col - 1)
                )
            {
            _canBeChoosenCells.Add(obj);
            }
        }
        if (_canBeChoosenCells.Count != 0)
        {
            int _choosen = GetRandomIntNumber(_canBeChoosenCells.Count - 1);
            return _canBeChoosenCells[_choosen];
        }
        else
        {
            return ChooseRandomTarget();
        }
        
        
    }

    Transform ChooseRandomTarget() 
    {
        _choosen = GetRandomIntNumber(aliveCells.Count - 1);
        return aliveCells[_choosen];
    }

    private void ShootInRandomCell(Transform canon, Transform target, Transform bullet, float angle)
    {


        Vector3 fromTo = new Vector3(-1, 1, 0) + target.position - canon.transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        canon.transform.LookAt(target.position);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        canon.Rotate(new Vector3(-angle, 0, 0));

        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float angleRadians = angle * Mathf.PI / 180;

        float v2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(angleRadians) * x) * Mathf.Pow(Mathf.Cos(angleRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        bullet.position = canon.position;
        bullet.GetComponent<Rigidbody>().velocity = canon.forward * v;

        _gameProcess.gameState = GameState.WaitingState;
    }

    private int GetRandomIntNumber(int max)
    {
        float rand = UnityEngine.Random.RandomRange(0, max);
        return (int)rand;
    }

    private void ShootAfterHit(Transform canon, Transform target, Transform bullet, float angle) 
    {
    
    }
}
