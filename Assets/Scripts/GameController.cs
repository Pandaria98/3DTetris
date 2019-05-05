using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int numOfRows = 20;
    public int numOfColumns = 10;
    public int scorePerRow = 10;

    public int level = 0;
    public float baseSecondsPerMove = 0.5f;
    public float downSecondsPerMove = 0.07f;
    private float _secondsPerMove;
    private float _timer;

    public float blinkDuration = 0.3f;
    public float blinkInterval = 0.08f;
    private bool _blinking = false;
    private float _blinkTimer;
    private float _blinkIntervalTimer;

    private Brick[,] _fixedBricks;
    private Board _board;

    public GameObject dominoPrefab;
    private Domino _domino;

    public GameObject mainGUI;
    private MainGUI _mainGUI;

    private int _score = 0;

    private bool _paused = false;

    private void Start()
    {
        _mainGUI = mainGUI.GetComponent<MainGUI>();
        _board = new Board(numOfRows, numOfColumns);
        _timer = _secondsPerMove;
        InitDomino();
	}

    private void Update()
    {
        if (_paused)
        {
            return;
        }
        if (_blinking)
        {
            UpdateBlink();
        }
        else
        {
            UpdateInput();
            UpdateMoveDown();
            CheckFull();
        }
    }

    private void StartBlink()
    {
        _blinking = true;
        _blinkTimer = blinkDuration;
        _blinkIntervalTimer = blinkInterval;
    }

    private void UpdateBlink()
    {
        _blinkTimer -= Time.deltaTime;
        _blinkIntervalTimer -= Time.deltaTime;
        if (_blinkTimer <= 0)
        {
            StopBlink();
        }
        if (_blinkIntervalTimer <= 0)
        {
            _blinkIntervalTimer = blinkInterval;
            _board.BlinkFullRows();
        }
    }

    private void StopBlink()
    {
        _blinking = false;
        int n = _board.NumOfFullRows();
        Score(n);
        _board.EliminateFullRows();
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

        if (Input.GetKeyDown("down"))
        {
            _timer = 0;
            _secondsPerMove = downSecondsPerMove;
        } else if (Input.GetKeyUp("down"))
        {
            SetLevel(level);
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

    private bool Tick()
    {
        // 计时完成时，返回真
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _secondsPerMove;
            return true;
        }
        return false;
    }

    private void UpdateMoveDown()
    {
        var d = _domino;
        bool shouldMoveDown = Tick();
        if (shouldMoveDown)
        {
            if (_board.CanMoveDown(d))
            {
                d.MoveDown();
            } else
            {
                // Game Over 只会发生在 Domino 想要被固定的时候
                if (IsDead())
                {
                    GameOver();
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
        _board.UpdateFullRows();
        int numOfFullRows = _board.NumOfFullRows();
        if (numOfFullRows > 0)
        {
            StartBlink();
        }
        

    }

    private void Score(int rows = 1)
    {
        _score += scorePerRow * rows;
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

    private void GameOver()
    {
        PauseGame();
        _mainGUI.ShowDeadGUI();
    }

    private void InitDomino()
    {
        int x = Mathf.FloorToInt(numOfColumns / 2);
        int y = numOfRows;
        _domino = Domino.GetRandomDomino(x, y);
    }

    private void log(object message)
    {
        Debug.Log(message);
    }
}
