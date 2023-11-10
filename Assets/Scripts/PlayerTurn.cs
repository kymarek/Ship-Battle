using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform[] cells;
    [SerializeField] GameObject _field;
    [SerializeField] private int _row, _column;
    [SerializeField] private Transform _canon;
    [SerializeField] private Transform _bullet;
    [SerializeField] private float _angle;
    [SerializeField] private GameProcess _gameProcess;
    private bool _isPlacible;


    // Start is called before the first frame update
    void Start()
    {
        _gameProcess = Camera.main.GetComponent<GameProcess>();
        cells = _field. GetComponent<GameField>().cells;
    }

    // Update is called once per frame
    void Update()
    {
        FollowCursor(_target);
        if (Input.GetMouseButtonUp(0) &&
            _isPlacible)
        {
            Shoot(_canon, _target, _bullet, _angle);
            _gameProcess.gameState = GameState.WaitingState;
        }
            

    }

    private void FollowCursor(Transform obj)
    {

        GetMouseCellPosition(out _column, out _row);

        int _choosen = (_row - 1) * 10 + (_column - 1);

        if (_choosen >= 0 &&
            _choosen < 100 &&
            !cells[_choosen].GetComponent<FieldProperty>().isDamaged)
        {
            obj.position = cells[(_row - 1) * 10 + (_column - 1)].position + new Vector3(0, 0.5f, 0);
            _isPlacible = true;
        }
        else
        {
            obj.localPosition = new Vector3(-15, 4, -1.5f);
            _isPlacible = false;
        }
    }


    private Vector3 GetMousePosition()
    {
        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouse, out RaycastHit hit, Mathf.Infinity))
            return hit.point;
        else
            return new Vector3(50, 50, 50);
    }

    private void GetMouseCellPosition(out int column, out int row)
    {
        Vector3 mouse = GetMousePosition();
        float _startPosX = cells[0].position.x + 1;
        float _startPosZ = cells[0].position.z + 1;
        column = 0;
        row = 0;
        bool _stopRow = false;
        bool _stopColumn = false;
        for (int i = 1; i < 11; i++)
        {
            if (mouse.x <= _startPosX &&
                !_stopColumn)
            {
                _stopColumn = true;
                column = i;
            }
            else
            {
                _startPosX += 2;
            }

            if (mouse.z <= _startPosZ &&
                !_stopRow &&
                mouse.x > cells[0].position.x - 1)
            {
                _stopRow = true;
                row = i;
            }
            else
            {
                _startPosZ += 2;
            }
        }
    }

    private void Shoot(Transform canon, Transform target, Transform bullet, float angle)
    {
        

        Vector3 fromTo = new Vector3(1, 0, 0) + target.position - canon.transform.position;
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
    }
}
