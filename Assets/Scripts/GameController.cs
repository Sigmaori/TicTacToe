using JetBrains.Annotations;
using System;
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

    public void EndTurn()
    {
        _moveCount++;

        if (_buttonList[0].text == _playerSide && _buttonList[1].text == _playerSide && _buttonList[2].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[3].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[5].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[6].text == _playerSide && _buttonList[7].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[0].text == _playerSide && _buttonList[3].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[1].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[7].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[5].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[0].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_moveCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
            ChangeSides();
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

        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].text = string.Empty;
        }
    }
}
