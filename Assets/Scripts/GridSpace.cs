using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button _button;
    public Text _buttonText;
    private GameController _gameController;

    public void SetGameControllerReference(GameController gameController)
    {
        _gameController = gameController;
    }

    public void SetSpace()
    {
        _buttonText.text = _gameController.GetPlayerSide();
        _button.interactable = false;
        _gameController.EndTurn();
    }
}
