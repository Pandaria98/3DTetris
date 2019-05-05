using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuCloseButton : MonoBehaviour
{
    public GameObject board;
    public Button button;
    public GameObject menu;

    private GameController _board;

    void Start()
    {
        _board = board.GetComponent<GameController>();
        button.onClick.AddListener(CloseMenu);
    }

    private void CloseMenu()
    {
        _board.ResumeGame();
        menu.SetActive(false);
    }
}
