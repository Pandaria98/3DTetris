using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    public Slider slider;
    public GameObject board;
    public Text sliderText;

    private GameController _board;

    private void Start()
    {
        _board = board.GetComponent<GameController>();
    }

    void Update()
    {
        int level = Mathf.FloorToInt(slider.value);
        _board.SetLevel(level);
        sliderText.text = "Level: " + level;
    }
}
