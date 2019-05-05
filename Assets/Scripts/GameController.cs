using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int numOfRows = 20;
    public int numOfColumns = 10;
    public int scorePerRow = 10;

    public int level = 5;
    public float baseSecondsPerMove = 1.0f;
    private float _secondsPerMove;
    private float _timer;

    private Brick[,] _fixedBricks;
    private Board _board;

    public GameObject dominoPrefab;
    private Domino _domino;
    private delegate void SetShape(Domino domino);
    private SetShape[] DominoShapes;

    public GameObject mainGUI;
    private MainGUI _mainGUI;

    private int _score = 0;

    private bool _paused = false;

    private void Start()
    {
        _mainGUI = mainGUI.GetComponent<MainGUI>();
        _board = new Board(numOfRows, numOfColumns);

        InitShapes();
        InitDomino();
        SetLevel(level);
        _timer = _secondsPerMove;
	}

    private void Update()
    {
        if (_paused)
        {
            return;
        }
        UpdateInput();
        UpdateMoveDown();
        CheckFull();
    }

    private void UpdateInput()
    {
        var d = _domino;
        if (Input.GetKeyDown("right") && _board.CanMoveRight(d))
        {
            d.MoveRight();
        }
        else if (Input.GetKeyDown("left") && _board.CanMoveLeft(d))
        {
            d.MoveLeft();
        }

        if (Input.GetKey("down"))
        {
            _timer -= 3 * Time.deltaTime;
        }

        if (Input.GetKeyDown("up") && _board.CanRotate(d))
        {
            d.Rotate();
        }

        if (Input.GetKeyDown("space"))
        {
            while (_board.CanMoveDown(_domino))
            {
                d.MoveDown();
            }
        }
    }

    public void PauseGame()
    {
        _paused = true;
    }

    public void ResumeGame()
    {
        _paused = false;
    }

    private void UpdateMoveDown()
    {
        _timer -= Time.deltaTime;
        var d = _domino;
        if (_timer <= 0)
        {
            _timer = _secondsPerMove;
            if (_board.CanMoveDown(d))
            {
                d.MoveDown();
            }
            else
            {
                // Game Over 只会发生在 Domino 想要被固定的时候
                if (IsDead())
                {
                    PauseGame();
                    _mainGUI.ShowDeadGUI();
                    return;
                }
                _board.SetDominoFixed(d);
                InitDomino();
            }
        }
    }

    public void SetLevel(int level)
    {
        this.level = level <= 9 ? level : 9;
        this.level = level >= 0 ? level : 0;
        _secondsPerMove = baseSecondsPerMove * ((1.0f - this.level / 10.0f));
        _mainGUI.SetLevel(level);
    }

    private void CheckFull()
    {
        for (int row = 0; row < numOfRows; row++)
        {
            bool full = _board.RowFull(row);
            if (full)
            {
                _board.RemoveRow(row);
                _board.MoveDownBoardByRow(row);
                Score();
                CheckFull();
                return;
            }
        }
    }

    private void Score()
    {
        _score += scorePerRow;
        _mainGUI.SetScore(_score);
    }

    private bool IsDead()
    {
        bool dead = false;
        int maxY = numOfRows - 1;
        var positions = _domino.CurrentPositoins();
        foreach (var p in positions)
        {
            if (p.y >= maxY)
            {
                dead = true;
            }
        }
        return dead;
    }

    public void RestartGame()
    {
        _board.Clear();
        _domino.Clear();
        _mainGUI.HideDeadGUI();

        InitDomino();
        ResumeGame();
    }

    private void InitShapes()
    {
        DominoShapes = new SetShape[7];
        DominoShapes[0] = Domino.SetS;
        DominoShapes[1] = Domino.SetZ;
        DominoShapes[2] = Domino.SetO;
        DominoShapes[3] = Domino.SetL;
        DominoShapes[4] = Domino.SetJ;
        DominoShapes[5] = Domino.SetT;
        DominoShapes[6] = Domino.SetI;
    }

    private void InitDomino()
    {
        int x = Mathf.FloorToInt(numOfColumns / 2);
        int y = numOfRows;

        Vector3 p = new Vector3(x, y, 0);
        Quaternion r = Quaternion.identity;
        GameObject d = Instantiate(dominoPrefab, p, r);

        _domino = d.GetComponent<Domino>();
        _domino.x = x;
        _domino.y = y;

        // TODO: 修改随机形状实现
        int i = Random.Range(0, DominoShapes.Length);
        DominoShapes[i](_domino);
    }

    private void log(object message)
    {
        Debug.Log(message);
    }
}
