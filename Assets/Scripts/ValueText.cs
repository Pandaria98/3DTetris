using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ValueText : MonoBehaviour
{
    public Text text;
    public string format;

    private int _value;

    public void Update()
    {
        text.text = string.Format(format, _value);
        // text.text = "Score\n" + _value;
    }

    public void SetValue(int value)
    {
        _value = value;
    }
}
