using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuEntryButton : MonoBehaviour
{
    public GameObject board;
    public Button button;
    public GameObject menu;

    private GameController _board;

    void Start()
    {
        _board = board.GetComponent<GameController>();
        button.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        _board.PauseGame();
        menu.SetActive(true);
    }
}
