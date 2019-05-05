using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public GameObject board;
    public Button button;
    public GameObject deadGUI;

    private GameController _board;

    void Start()
    {
        _board = board.GetComponent<GameController>();
        button.onClick.AddListener(CloseDeadGUI);
    }

    private void CloseDeadGUI()
    {
        _board.RestartGame();
        deadGUI.SetActive(false);
    }
}
