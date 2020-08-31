using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

[Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

public class GameController : MonoBehaviour
{
    public Text[] _buttonList;
    public Text _gameOverText;
    public GameObject _gameOverPanel;
    public GameObject _restartButton;
    public GameObject _startInfo;
    public Player _playerX;
    public Player _playerO;
    public PlayerColor _activePlayerColor;
    public PlayerColor _inactivePlayerColor;

    private string _playerSide;
    private int _moveCount;
    private bool _isWinner = false;
    private bool _toggleOnAIOpponent = true;
    private bool _hasAIMoved = false;

    private void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    private void SetBoardInteractable(bool boardToggle)
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<Button>().interactable = boardToggle;
        }
    }

    private void SetPlayerButtons(bool togglePlayerButton)
    {
        _playerX.button.interactable = togglePlayerButton;
        _playerO.button.interactable = togglePlayerButton;
    }

    private void SetPlayerColorInactive()
    {
        _playerX.panel.color = _inactivePlayerColor.panelColor;
        _playerX.text.color = _inactivePlayerColor.textColor;

        _playerO.panel.color = _inactivePlayerColor.panelColor;
        _playerO.text.color = _inactivePlayerColor.textColor;
    }

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        _gameOverPanel.SetActive(false);
        _moveCount = 0;
        _restartButton.SetActive(false);
    } 

    private void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        _startInfo.SetActive(false);
    }

    private void GameOver(string winningPlayer)
    {
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a draw!");
            SetPlayerColorInactive();
        }
        else
            SetGameOverText(winningPlayer + " Wins!");

        SetBoardInteractable(false);

        _restartButton.SetActive(true);
    }

    private void ChangeSides()
    {
        _playerSide = _playerSide == "X" ? "O" : "X";

        if (_playerSide == "X")
            SetPlayerColors(_playerX, _playerO);
        else
            SetPlayerColors(_playerO, _playerX);
    }

    private void SetGameOverText(string gameOverText)
    {
        _gameOverPanel.SetActive(true);
        _gameOverText.text = gameOverText;
    }

    private void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = _activePlayerColor.panelColor;
        newPlayer.text.color = _activePlayerColor.textColor;

        oldPlayer.panel.color = _inactivePlayerColor.panelColor;
        oldPlayer.text.color = _inactivePlayerColor.textColor;
    }

    public void SetStartingSide(string startingSide)
    {
        //Set AI player here after player selects side?
        _playerSide = startingSide;

        if (_playerSide == "X")
            SetPlayerColors(_playerX, _playerO);
        else
            SetPlayerColors(_playerO, _playerX);

        StartGame();
    }

    public string GetPlayerSide()
    {
        return _playerSide;
    }

    private bool IsWinner()
    {
        var winConditions = new[]
        {
            //Vertical
            new [] { 0, 1, 2 },
            new [] { 3, 4, 5 },
            new [] { 6, 7, 8 },

            //Horizontal
            new [] { 0, 3, 6 },
            new [] { 1, 4, 7 },
            new [] { 2, 5, 8 },

            //Diagonal
            new [] { 0, 4, 8 },
            new [] { 2, 4, 6 }
        };

        foreach (var winCondition in winConditions)
        {
            _isWinner = winCondition.All(c => _buttonList[c].text == _playerSide);

            if (_isWinner)
                break;
        }

        return _isWinner;
    }

    private void AITurn()
    {
        var winConditions = new[]
        {
            //Vertical
            new [] { 0, 1, 2 },
            new [] { 3, 4, 5 },
            new [] { 6, 7, 8 },

            //Horizontal
            new [] { 0, 3, 6 },
            new [] { 1, 4, 7 },
            new [] { 2, 5, 8 },

            //Diagonal
            new [] { 0, 4, 8 },
            new [] { 2, 4, 6 }
        };

        var buttonToSet = 0;

        //Easy -- Pick any empty space --
        foreach (var winCondition in winConditions)
        {
            foreach (var win in winCondition)
            {
                if (string.IsNullOrEmpty(_buttonList[win].text))
                {
                    buttonToSet = win;
                    break;
                }
            }
        }

        //Hard -- Determine unused win condition available --

        //Very Hard -- Learn player movements and block player --

        _buttonList[buttonToSet].text = _playerSide;
        _buttonList[buttonToSet].GetComponentInParent<Button>().interactable = false;

        _hasAIMoved = true;
        EndTurn();
    }

    public void EndTurn()
    {
        _moveCount++;

        IsWinner();

        if (_isWinner)
            GameOver(_playerSide);
        else
        {
            if (_moveCount >= 9)
                GameOver("draw");
            else
            {
                ChangeSides();

                if (!_hasAIMoved && _toggleOnAIOpponent)
                    AITurn();
                else
                    _hasAIMoved = false;
            }
        }
    }

    public void RestartGame()
    {
        _moveCount = 0;
        _gameOverPanel.SetActive(false);
        _restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorInactive();
        _startInfo.SetActive(true);
        _hasAIMoved = false;

        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].text = string.Empty;
        }
    }
}
