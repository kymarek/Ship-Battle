using Assets.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnemyShips : MonoBehaviour
{
    [SerializeField] private Transform[] _cells;
    [SerializeField] private List<Transform> _freeCells;
    [SerializeField] private GameObject _field;
    private List<Transform> _availableShips;
    // Start is called before the first frame update
    void Start()
    {
        _availableShips = GetComponent<AvailiableShips>().ships;
        _cells = _field.GetComponent<GameField>().cells;
        _freeCells = _cells.ToList();

        SpawnFourDeckShip();

        for (int i = 0; i < 2; i++)
        {
            SpawnThreeDeckShip();
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnDoubleDeckShip();
        }

            SpawnSingleDeckShip();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool SetRandomBoolState() {
        float rand = UnityEngine.Random.value;
        if (rand <= 0.5f)
        {
            return false;
        }
        else return true;
    }

    private int GetRandomIntNumber(int max) 
    {
        float rand = UnityEngine.Random.RandomRange(0, max);
        return (int)rand;
    }

    private void RemoveBusyCells()
    {
        _freeCells = _freeCells.Where(x => !x.GetComponent<FieldProperty>().isBusy).ToList();
    }

    private void SpawnFourDeckShip()
    {
        foreach (Transform ship in _availableShips)
        {
            if (ship.GetComponent<Ship>()?.shipLevel == ShipTier.fourDeck)
            {
                Ship _shipComponent = ship.GetComponent<Ship>();
                _shipComponent.isHorizontal = SetRandomBoolState();
                if (!_shipComponent.isHorizontal)
                {
                    ship.transform.Rotate(new Vector3(0, 90, 0));
                }

                int _choosen = GetRandomIntNumber(_cells.Length - 1);

                if (_shipComponent.isHorizontal)
                {
                    if (_choosen % 10 < 3)
                    {
                        _choosen = _choosen / 10 * 10 + 3;
                    }

                    for (int i = 3; i >= 0; i--)
                    {
                        ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen - i].gameObject;
                        _cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in _cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }
                else
                {
                    if (_choosen / 10 > 6)
                    {
                        _choosen = 60 + _choosen % 10;
                    }

                    for (int i = 0; i < 4; i++)
                    {

                        ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen + i * 10].gameObject;
                        _cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                        foreach (Transform neighbors in _cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                        {
                            neighbors.GetComponent<FieldProperty>().isBusy = true;
                        }
                    }
                }

                ship.transform.position = new Vector3(_cells[_choosen].position.x, _cells[_choosen].position.y + 3, _cells[_choosen].position.z);

                _availableShips.Remove( ship );
                break;
            }
        }
    }

    private void SpawnThreeDeckShip()
    {
        bool _isPlaceable = false;
        foreach (Transform ship in _availableShips)
        {
            if (ship.GetComponent<Ship>()?.shipLevel == ShipTier.threeDeck)
            {
                Ship _shipComponent = ship.GetComponent<Ship>();
                _shipComponent.isHorizontal = SetRandomBoolState();
                if (!_shipComponent.isHorizontal)
                {
                    ship.transform.Rotate(new Vector3(0, 90, 0));
                }

                int _choosen = GetRandomIntNumber(_cells.Length - 1);

                while(!_isPlaceable)
                {
                    if (_shipComponent.isHorizontal)
                    {
                        if (_choosen % 10 < 2)
                        {
                            _choosen = _choosen / 10 * 10 + 2;
                        }

                        if (_cells[_choosen].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen - 1].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen - 2].GetComponent<FieldProperty>().isBusy ||
                            ((_choosen % 10) < 2 && _cells[_choosen / 10 + 2 ].GetComponent<FieldProperty>().isBusy)
                            )
                        {
                            _choosen = GetRandomIntNumber(_cells.Length - 1);
                            continue;
                        }
                        else 
                        {
                            for (int i = 2; i >= 0; i--)
                            {
                                ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen - i].gameObject;
                                _cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                                foreach (Transform neighbors in _cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                                {
                                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (_choosen / 10 > 7)
                        {
                            _choosen = 70 + _choosen % 10;
                        }

                        if (_cells[_choosen].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen + 10].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen + 20].GetComponent<FieldProperty>().isBusy ||
                            ((_choosen / 10) > 7 && _cells[_choosen / 10 * 10 + 2].GetComponent<FieldProperty>().isBusy)
                            )
                        {
                            _choosen = GetRandomIntNumber(_cells.Length - 1);
                            continue;
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen + i * 10].gameObject;
                                _cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                                foreach (Transform neighbors in _cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                                {
                                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                                }
                            }
                            break;
                        }
                    }
                }

                ship.transform.position = new Vector3(_cells[_choosen].position.x, _cells[_choosen].position.y + 3, _cells[_choosen].position.z);

                GetComponent<AvailiableShips>().ships.Remove(ship);
                break;
            }
        }
    }

    private void SpawnDoubleDeckShip()
    {
        bool _isPlaceable = false;
        foreach (Transform ship in _availableShips)
        {
            if (ship.GetComponent<Ship>()?.shipLevel == ShipTier.doubleDeck)
            {
                Ship _shipComponent = ship.GetComponent<Ship>();
                _shipComponent.isHorizontal = SetRandomBoolState();
                if (!_shipComponent.isHorizontal)
                {
                    ship.transform.Rotate(new Vector3(0, 90, 0));
                }

                int _choosen = GetRandomIntNumber(_cells.Length - 1);

                while (!_isPlaceable)
                {
                    if (_shipComponent.isHorizontal)
                    {
                        if (_choosen % 10 < 1)
                        {
                            _choosen = _choosen / 10 * 10 + 1;
                        }

                        if (_cells[_choosen].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen - 1].GetComponent<FieldProperty>().isBusy ||
                            ((_choosen % 10) < 1 && _cells[_choosen / 10 + 1].GetComponent<FieldProperty>().isBusy)
                            )
                        {
                            _choosen = GetRandomIntNumber(_cells.Length - 1);
                            continue;
                        }
                        else
                        {
                            for (int i = 1; i >= 0; i--)
                            {
                                ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen - i].gameObject;
                                _cells[_choosen - i].GetComponent<FieldProperty>().isBusy = true;
                                foreach (Transform neighbors in _cells[_choosen - i].GetComponent<FieldProperty>()._adjacentCells)
                                {
                                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (_choosen / 10 > 8)
                        {
                            _choosen = 80 + _choosen % 10;
                        }

                        if (_cells[_choosen].GetComponent<FieldProperty>().isBusy ||
                            _cells[_choosen + 10].GetComponent<FieldProperty>().isBusy ||
                            ((_choosen / 10) > 8 && _cells[_choosen / 10 * 10 + 1].GetComponent<FieldProperty>().isBusy)
                            )
                        {
                            _choosen = GetRandomIntNumber(_cells.Length - 1);
                            continue;
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                ship.GetComponentsInChildren<Deck>()[i].stayAt = _cells[_choosen + i * 10].gameObject;
                                _cells[_choosen + i * 10].GetComponent<FieldProperty>().isBusy = true;
                                foreach (Transform neighbors in _cells[_choosen + i * 10].GetComponent<FieldProperty>()._adjacentCells)
                                {
                                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                                }
                            }
                            break;
                        }
                    }
                }

                ship.transform.position = new Vector3(_cells[_choosen].position.x, _cells[_choosen].position.y + 3, _cells[_choosen].position.z);

                GetComponent<AvailiableShips>().ships.Remove(ship);
                break;
            }
        }
    }

    private void SpawnSingleDeckShip()
    {
        foreach (Transform ship in _availableShips)
        {
            RemoveBusyCells();
            if (ship.GetComponent<Ship>()?.shipLevel == ShipTier.singleDeck)
            {

                int _choosen = GetRandomIntNumber(_freeCells.Count - 1);           

                ship.transform.position = new Vector3(_freeCells[_choosen].position.x, _freeCells[_choosen].position.y + 3, _freeCells[_choosen].position.z);

                _freeCells[_choosen].GetComponent<FieldProperty>().isBusy = true;

                ship.GetComponentsInChildren<Deck>()[0].stayAt = _freeCells[_choosen].gameObject;

                foreach (Transform neighbors in _freeCells[_choosen].GetComponent<FieldProperty>()._adjacentCells)
                {
                    neighbors.GetComponent<FieldProperty>().isBusy = true;
                }
            }
        }
    }
}
