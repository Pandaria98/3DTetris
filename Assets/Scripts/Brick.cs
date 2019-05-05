using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {
    public int x;
    public int y;

    private void Update()
    {
        // 用 x y 值设置 Object 的实际位置
        transform.position = new Vector3(x, y, 0);
    }

    public void MoveDown()
    {
        y -= 1;
    }

    public void MoveLeft()
    {
        x -= 1;
    }

    public void MoveRight()
    {
        x += 1;
    }
}
