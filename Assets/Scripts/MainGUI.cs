using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGUI : MonoBehaviour
{
    public GameObject scoreText;
    private ValueText _scoreText;

    public GameObject levelText;
    private ValueText _levelText;

    public GameObject deadGUI;

    private void Start()
    {
        _scoreText = scoreText.GetComponent<ValueText>();
        _scoreText.format = "Score\n{0:D}";
        _levelText = levelText.GetComponent<ValueText>();
        _levelText.format = "Level\n{0:D}";
    }

    public void SetLevel(int level)
    {
        _levelText.SetValue(level);
    }

    public void SetScore(int score)
    {
        _scoreText.SetValue(score);
    }

    public void ShowDeadGUI()
    {
        deadGUI.SetActive(true);
    }

    public void HideDeadGUI()
    {
        deadGUI.SetActive(false);
    }

    public void GetScript<T>(GameObject o, ref T script)
    {
        script = o.GetComponent<T>();
    }
}
