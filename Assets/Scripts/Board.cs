using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int numOfRows = 20;
    public int numOfColumns = 10;
    public GameObject brickPrefab;
    public GameObject dominoPrefab;

    private Brick[,] _fixedBricks;
    private Brick _activeBrick;
    private Domino _domino;
    private delegate void SetShape(Domino domino);
    private SetShape[] DominoShapes;

    public int level = 5;
    public float baseSecondsPerMove = 1.0f;
    private float _secondsPerMove;
    private float _timer;

    private void Start()
    {
        InitBoard();
        InitShapes();
        InitDomino();
        SetLevel(level);
        _timer = _secondsPerMove;
	}

    private void Update()
    {
        UpdateInput();
        UpdateMoveDown();
        CheckFull();
    }

    private void UpdateInput()
    {
        var d = _domino;
        if (Input.GetKeyDown("right") && CanMoveRight(_domino))
        {
            d.MoveRight();
        }
        else if (Input.GetKeyDown("left") && CanMoveLeft(_domino))
        {
            d.MoveLeft();
        }

        if (Input.GetKey("down"))
        {
            _timer -= 3 * Time.deltaTime;
        }

        if (Input.GetKeyDown("up") && CanRotate(_domino))
        {
            d.Rotate();
        }
    }

    private void UpdateMoveDown()
    {
        _timer -= Time.deltaTime;
        var d = _domino;
        if (_timer <= 0)
        {
            _timer = _secondsPerMove;
            if (CanMoveDown(d))
            {
                d.MoveDown();
            }
            else
            {
                SetDominoFixed(d);
                InitDomino();
            }
        }
    }

    private void SetDominoFixed(Domino domino)
    {
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            _fixedBricks[b.x, b.y] = b;
        }
        Destroy(domino.gameObject);
    }

    private void CheckFull()
    {
        for (int row = 0; row < numOfRows; row++)
        {
            bool full = RowFull(row);
            if (full)
            {
                RemoveRow(row);
                MoveDownBoardByRow(row);
                CheckFull();
                return;
            }
        }
    }

    private bool RowFull(int row)
    {
        for (int c = 0; c < numOfColumns; c++)
        {
            if (_fixedBricks[c, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveRow(int row)
    {
        for (int c = 0; c < numOfColumns; c++)
        {
            var b = _fixedBricks[c, row];
            if (b != null)
            {
                var o = b.gameObject;
                Destroy(o);
            }
        }
    }

    private void MoveDownBoardByRow(int row)
    {
        for (int r = row + 1; r < numOfRows; r++)
        {
            for (int c = 0; c < numOfColumns; c++)
            {
                var b = _fixedBricks[c, r];
                if (b != null)
                {
                    log("move down borad by row");
                    b.MoveDown();
                }
                // 移动本行到数组的下一行
                _fixedBricks[c, r - 1] = _fixedBricks[c, r];
            }
        }

        // 清除最上一行
        for (int c = 0; c < numOfColumns; c++)
        {
            _fixedBricks[c, numOfColumns - 1] = null;
        }
    }

    private void InitBrick()
    {
        int x = 9;
        int y = 9;
        Vector3 p = new Vector3(x, y, 0);
        Quaternion r = Quaternion.identity;
        GameObject brick = Instantiate(brickPrefab, p, r);
        _activeBrick = brick.GetComponent<Brick>();
        _activeBrick.x = x;
        _activeBrick.y = y;
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
        log("y " + _domino.y);
        int i = Random.Range(0, DominoShapes.Length);
        DominoShapes[i](_domino);
    }

    private void InitBoard()
    {
        var rows = numOfRows;
        var columns = numOfColumns;
        _fixedBricks = new Brick[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                _fixedBricks[x, y] = null;
            }
        }
    }

    #region CanMove
    private bool _PositionValid(int x, int y)
    {
        bool result;
        if (x < 0 || x >= numOfColumns)
        {
            result = false;
        }
        else if (y < 0 || y >= numOfRows)
        {
            result = false;
        }
        else
        {
            var slot = _fixedBricks[x, y];
            result = slot == null;
        }
        return result;
    }

    private bool _CanMove(Brick brick, int dx, int dy)
    {
        int x = brick.x;
        int y = brick.y;
        return _PositionValid(x + dx, y + dy);
    }

    private bool CanMoveRight(Brick brick)
    {
        return _CanMove(brick, 1, 0);
    }

    private bool CanMoveRight(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && _CanMove(b, 1, 0);
        }
        return result;
    }

    private bool CanMoveLeft(Brick brick)
    {
        return _CanMove(brick, -1, 0);
    }

    private bool CanMoveLeft(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && _CanMove(b, -1, 0);
        }
        return result;
    }

    private bool CanMoveDown(Brick brick)
    {
        return _CanMove(brick, 0, -1);
    }

    private bool CanMoveDown(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && _CanMove(b, 0, -1);
        }
        return result;
    }
    #endregion

    private bool CanRotate(Domino domino)
    {
        bool result = true;
        var positions = domino.RotatedPositions();
        for (int i = 0; i < 4; i++)
        {
            var p = positions[i];
            result = result && _PositionValid((int)p.x, (int)p.y);
        }
        return result;
    }

    private void SetLevel(int level)
    {
        this.level = level <= 9 ? level : 9;
        this.level = level >= 0 ? level : 0;
        _secondsPerMove = baseSecondsPerMove * ((1.0f - this.level / 10.0f));
    }

    private void log(object message)
    {
        Debug.Log(message);
    }
}
