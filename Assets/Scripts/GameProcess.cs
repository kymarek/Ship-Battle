using Assets.Enums;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public GameState _gameState;
    public GameState gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            ChangeGameState();
        }
    }
    [SerializeField] private PlaceShip _placeShip;
    [SerializeField] private SpawnEnemyShips _spawnEnemyShips;
    [SerializeField] private PlayerTurn _playerTurn;
    [SerializeField] private EnemyTurn _enemyTurn;
    // Start is called before the first frame update
    void Start()
    {
        ChangeGameState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeGameState() 
    {
        switch (_gameState)
        {
            case GameState.Placing:
                _placeShip.enabled = true;
                _spawnEnemyShips.enabled = true;
                _playerTurn.enabled = false;
                _enemyTurn.enabled = false;
                break;
            case GameState.PlayerTurn:
                _placeShip.enabled = false;
                _spawnEnemyShips.enabled = false;
                _playerTurn.enabled= true;
                _enemyTurn.enabled = false;
                break;
            case GameState.EnemyTurn:
                _placeShip.enabled = false;
                _spawnEnemyShips.enabled = false;
                _playerTurn.enabled = false;
                _enemyTurn.enabled = true;
                break;
            case GameState.WaitingState:
                _placeShip.enabled = false;
                _spawnEnemyShips.enabled = false;
                _playerTurn.enabled = false;
                _enemyTurn.enabled = false;
                break;
        }
    }
}
