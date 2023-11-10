using Assets.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceShip : MonoBehaviour
{
    [SerializeField] private bool _isPlacingPeriod = true;
    [SerializeField] private GameField _field;
    [SerializeField] private List<Transform> _availableShips;
    [SerializeField] private bool _isHorizontal = true;
    [SerializeField] private int _row, _column;


    // Start is called before the first frame update
    void Start()
    {
        _availableShips = gameObject.GetComponent<AvailiableShips>().ships;
    }

    // Update is called once per frame
    void Update()
    {

        if (_availableShips.Count != 0)
        {
            FollowCursor(_availableShips[0]);
        }
        else
        {
            Camera.main.GetComponent<GameProcess>().gameState = GameState.PlayerTurn; 
        }
    }

    private void FollowCursor(Transform obj)
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (_isHorizontal)
            {
                _isHorizontal = false;
            }
            else
            {
                _isHorizontal = true;
            }
        }


        GetMouseCellPosition(out _column, out _row);


        if ((_row - 1) * 10 + (_column - 1) >= 0 &&
            (_row - 1) * 10 + (_column - 1) < 100)
        {
            
            switch (obj.GetComponent<Ship>().shipLevel)
            {
                case ShipTier.fourDeck:
                    PlacingFourDeckPosition(obj);
                    break;
                case ShipTier.threeDeck:
                    PlacingThreeDeckPosition(obj);
                    break;
                case ShipTier.doubleDeck:
                    PlacingDoubleDeckPosition(obj);
                    break;
                case ShipTier.singleDeck:
                    PlacingSingleDeckPosition(obj);
                    break;
            }
        }
        else 
        {
            obj.position = new Vector3(-15, 10, -1.5f);
        }
        

        if (_isHorizontal && obj.GetComponent<Ship>().shipLevel != ShipTier.singleDeck)
        {
            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            obj.rotation = rotation;
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            obj.rotation = rotation;
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
        Transform[] cell = _field.GetComponent<GameField>().cells;
        float _startPosX = cell[0].position.x + 1;
        float _startPosZ = cell[0].position.z + 1;
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
                !_stopRow)
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

    private void PlacingFourDeckPosition(Transform obj)
    {
        if ((!_isHorizontal && _column > 3) ||
                ((_isHorizontal && _row < 8)))
        {
            int _choosen = (_row - 1) * 10 + (_column - 1);
            Transform[] cells = _field.GetComponent<GameField>().cells;
            obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            if (Input.GetMouseButtonUp(0))
            {
                _availableShips.Remove(obj);
                obj.AddComponent<Rigidbody>();
                if (_availableShips.Count == 0)
                    _availableShips[0].position = new Vector3(-25, 10, -1.5f);

                

                if (!_isHorizontal)
                {
                    for (int i = 3; i >= 0; i--)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen - i].gameObject;
                        cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen + i * 10].gameObject;
                        cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }

                obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            }
        }
        else
        {
            obj.position = new Vector3(-25, 10, -1.5f);
        }
    }

    private void PlacingThreeDeckPosition(Transform obj)
    {
        int _choosen = (_row - 1) * 10 + (_column - 1);
        Transform[] cells = _field.GetComponent<GameField>().cells;
        bool _isPlacible = true;

        if (!_isHorizontal)
        {
            for (int i = 2; i >= 0; i--)
            {
                if (cells[_choosen - i].GetComponent<FieldProperty>().isBusy)
                {
                    _isPlacible = false;
                    obj.position = new Vector3(-25, 10, -1.5f);
                    break;
                }
            }
        }
        else 
        {
            for (int i = 0; i < 3; i++)
            {
                if (cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy || _row > 8)
                {
                    _isPlacible = false;
                    obj.position = new Vector3(-25, 10, -1.5f);
                    break;
                }
            }
        }

        if (((!_isHorizontal && _column > 2) ||
                    ((_isHorizontal && _row < 9))) &&
                    _isPlacible)
        {
            obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            if (Input.GetMouseButtonUp(0))
            {
                _availableShips.Remove(obj);
                obj.AddComponent<Rigidbody>();
                obj.position = cells[_choosen].position + new Vector3(0, 1, 0);

                if (!_isHorizontal)
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen - i].gameObject;
                        cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen + i * 10].gameObject;
                        cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }


            }
        }
        else
        {
            obj.position = new Vector3(-25, 10, -1.5f);
        }
    }

    private void PlacingDoubleDeckPosition(Transform obj)
    {
        int _choosen = (_row - 1) * 10 + (_column - 1);
        Transform[] cells = _field.GetComponent<GameField>().cells;
        bool _isPlacible = true;

        if (!_isHorizontal)
        {
            for (int i = 1; i >= 0; i--)
            {
                if (cells[_choosen - i].GetComponent<FieldProperty>().isBusy || _column < 1)
                {
                    _isPlacible = false;
                    obj.position = new Vector3(-15, 6, -1.5f);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy || _row > 9)
                {
                    _isPlacible = false;
                    obj.position = new Vector3(-15, 10, -1.5f);
                    break;
                }
            }
        }

        if (((!_isHorizontal && _column > 1) ||
                    ((_isHorizontal && _row < 10))) &&
                    _isPlacible)
        {
            obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            if (Input.GetMouseButtonUp(0))
            {
                _availableShips.Remove(obj);
                obj.AddComponent<Rigidbody>();
                obj.position = cells[_choosen].position + new Vector3(0, 1, 0);

                if (!_isHorizontal)
                {
                    for (int i = 1; i >= 0; i--)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen - i].gameObject;
                        cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        obj.GetComponentsInChildren<Deck>()[i].stayAt = cells[_choosen + i * 10].gameObject;
                        cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }


            }
        }
        else
        {
            obj.position = new Vector3(-25, 10, -1.5f);
        }
    }

    private void PlacingSingleDeckPosition(Transform obj)
    {
        int _choosen = (_row - 1) * 10 + (_column - 1);
        Transform[] cells = _field.GetComponent<GameField>().cells;
        List<Transform> _freeCells = cells.Where(x => !x.GetComponent<FieldProperty>().isBusy).ToList();

       

        if (!cells[_choosen].GetComponent<FieldProperty>().isBusy)
        {
            obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            if (Input.GetMouseButtonUp(0))
            {
                _availableShips.Remove(obj);
                obj.AddComponent<Rigidbody>();

                obj.GetComponentsInChildren<Deck>()[0].stayAt = cells[_choosen].gameObject;
                cells[_choosen].GetComponent<FieldProperty>().isBusy = true;

                foreach (Transform neighbors in cells[_choosen].GetComponent<FieldProperty>()._adjacentCells)
                {
                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                }
                obj.position = cells[_choosen].position + new Vector3(0, 1, 0);
            }
        }
        else
        {
            obj.position = new Vector3(-25, 10, -1.5f);
        }
    }
}
