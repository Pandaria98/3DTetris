using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    private int _numOfRows = 20;
    private int _numOfColumns = 10;
    private Brick[,] _fixedBricks;

    public Board(int numOfRows, int numOfColumns)
    {
        this._numOfRows = numOfRows;
        this._numOfColumns = numOfColumns;
        InitBoard();
    }

    private void InitBoard()
    {
        var rows = _numOfRows;
        var columns = _numOfColumns;
        _fixedBricks = new Brick[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                _fixedBricks[x, y] = null;
            }
        }
    }

    private bool _PositionValid(int x, int y)
    {
        bool result;
        if (x < 0 || x >= _numOfColumns)
        {
            result = false;
        }
        else if (y < 0 || y >= _numOfRows)
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

    public bool CanMoveRight(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && CanMoveRight(b);
        }
        return result;
    }

    private bool CanMoveLeft(Brick brick)
    {
        return _CanMove(brick, -1, 0);
    }

    public bool CanMoveLeft(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && CanMoveLeft(b);
        }
        return result;
    }

    private bool CanMoveDown(Brick brick)
    {
        return _CanMove(brick, 0, -1);
    }

    public bool CanMoveDown(Domino domino)
    {
        bool result = true;
        var bricks = domino._bricks;
        for (int i = 0; i < bricks.Length; i++)
        {
            var b = bricks[i];
            result = result && CanMoveDown(b);
        }
        return result;
    }

    public bool CanRotate(Domino domino)
    {
        bool result = true;
        var positions = domino.RotatedPositions();
        foreach (var p in positions)
        {
            result = result && _PositionValid((int)p.x, (int)p.y);
        }
        return result;
    }

    public void SetDominoFixed(Domino domino)
    {
        var bricks = domino._bricks;
        foreach (var b in bricks)
        {
            _fixedBricks[b.x, b.y] = b;
        }
        Destroy(domino.gameObject);
    }

    public void Clear()
    {
        foreach (var b in _fixedBricks)
        {
            if (b != null)
            {
                Destroy(b.gameObject);
            }
        }
    }

    public bool RowFull(int row)
    {
        for (int c = 0; c < _numOfColumns; c++)
        {
            if (_fixedBricks[c, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveRow(int row)
    {
        for (int c = 0; c < _numOfColumns; c++)
        {
            var b = _fixedBricks[c, row];
            if (b != null)
            {
                var o = b.gameObject;
                Destroy(o);
            }
        }
    }

    public void MoveDownBoardByRow(int row)
    {
        for (int r = row + 1; r < _numOfRows; r++)
        {
            for (int c = 0; c < _numOfColumns; c++)
            {
                var b = _fixedBricks[c, r];
                if (b != null)
                {
                    b.MoveDown();
                }
                // 移动本行到数组的下一行
                _fixedBricks[c, r - 1] = _fixedBricks[c, r];
            }
        }

        // 清除最上一行
        for (int c = 0; c < _numOfColumns; c++)
        {
            _fixedBricks[c, _numOfColumns - 1] = null;
        }
    }
}
