using UnityEngine;
using System.Collections;

public class Domino : MonoBehaviour
{
    public GameObject brickPrefab;
    public int x;
    public int y;
    public Brick[] _bricks = new Brick[4];
    public Material[] materials = new Material[7];

    private int _width = 3;
    private int _height = 3;
    private int _shapeIndex = 0;
    private int _numOfShapes = 2;
    private int[,,] _shapes;

    public static void SetS(Domino domino)
    {
        domino.InitBricksByColorIndex(0);
        domino._width = 3;
        domino._height = 3;
        domino._shapeIndex = 0;
        domino._numOfShapes = 2;
        domino._shapes = new int[,,] {
            {
                { 0, 1, 1, },
                { 1, 1, 0, },
                { 0, 0, 0, },
            },
            {
                { 0, 1, 0, },
                { 0, 1, 1, },
                { 0, 0, 1, },
            },
        };
    }

    public static void SetZ(Domino domino)
    {
        domino.InitBricksByColorIndex(1);
        domino._width = 3;
        domino._height = 3;
        domino._shapeIndex = 0;
        domino._numOfShapes = 2;
        domino._shapes = new int[,,] {
            {
                { 1, 1, 0, },
                { 0, 1, 1, },
                { 0, 0, 0, },
            },
            {
                { 0, 1, 0, },
                { 1, 1, 0, },
                { 1, 0, 0, },
            },
        };
    }

    public static void SetO(Domino domino)
    {
        domino.InitBricksByColorIndex(2);
        domino._width = 2;
        domino._height = 2;
        domino._shapeIndex = 0;
        domino._numOfShapes = 1;
        domino._shapes = new int[,,] {
            {
                { 1, 1, },
                { 1, 1, },
            },
        };
    }

    public static void SetT(Domino domino)
    {
        domino.InitBricksByColorIndex(3);
        domino._width = 3;
        domino._height = 3;
        domino._shapeIndex = 0;
        domino._numOfShapes = 4;
        domino._shapes = new int[,,] {
            {
                { 0, 1, 0, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            },
            {
                { 0, 1, 0, },
                { 0, 1, 1, },
                { 0, 1, 0, },
            },
            {
                { 0, 0, 0, },
                { 1, 1, 1, },
                { 0, 1, 0, },
            },
            {
                { 0, 1, 0, },
                { 1, 1, 0, },
                { 0, 1, 0, },
            },
        };
    }

    public static void SetL(Domino domino)
    {
        domino.InitBricksByColorIndex(4);
        domino._width = 3;
        domino._height = 3;
        domino._shapeIndex = 0;
        domino._numOfShapes = 4;
        domino._shapes = new int[,,] {
            {
                { 0, 1, 0, },
                { 0, 1, 0, },
                { 0, 1, 1, },
            },
            {
                { 0, 0, 0, },
                { 1, 1, 1, },
                { 1, 0, 0, },
            },
            {
                { 1, 1, 0, },
                { 0, 1, 0, },
                { 0, 1, 0, },
            },
            {
                { 0, 0, 1, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            },
        };
    }

    public static void SetJ(Domino domino)
    {
        domino.InitBricksByColorIndex(5);
        domino._width = 3;
        domino._height = 3;
        domino._shapeIndex = 0;
        domino._numOfShapes = 4;
        domino._shapes = new int[,,] {
            {
                { 0, 1, 0, },
                { 0, 1, 0, },
                { 1, 1, 0, },
            },
            {
                { 1, 0, 0, },
                { 1, 1, 1, },
                { 0, 0, 0, },
            },
            {
                { 0, 1, 1, },
                { 0, 1, 0, },
                { 0, 1, 0, },
            },
            {
                { 0, 0, 0, },
                { 1, 1, 1, },
                { 0, 0, 1, },
            },
        };
    }

    public static void SetI(Domino domino)
    {
        domino.InitBricksByColorIndex(6);
        domino._width = 4;
        domino._height = 4;
        domino._shapeIndex = 0;
        domino._numOfShapes = 2;
        domino._shapes = new int[,,] {
            {
                { 0, 0, 0, 0, },
                { 1, 1, 1, 1, },
                { 0, 0, 0, 0, },
                { 0, 0, 0, 0, },
            },
            {
                { 0, 1, 0, 0, },
                { 0, 1, 0, 0, },
                { 0, 1, 0, 0, },
                { 0, 1, 0, 0, },
            },
        };
    }

    private void Update()
    {
        // 用 x y 值设置 Object 的实际位置
        transform.position = new Vector3(x, y, 0);
        UpdateBricksPosition();
    }

    private void InitBricks()
    {
        Vector3 p = new Vector3(100, 100, 0);
        Quaternion r = Quaternion.identity;
        for (int i = 0; i < _bricks.Length; i++)
        {
            GameObject o = Instantiate(brickPrefab, p, r);
            Brick b = o.GetComponent<Brick>();
            _bricks[i] = b;
            var renderer = b.GetComponentInChildren<Renderer>();
            renderer.material = materials[0];
        }
    }

    private void InitBricksByColorIndex(int colorIndex)
    {
        Vector3 p = new Vector3(100, 100, 0);
        Quaternion r = Quaternion.identity;
        for (int i = 0; i < _bricks.Length; i++)
        {
            GameObject o = Instantiate(brickPrefab, p, r);
            Brick b = o.GetComponent<Brick>();
            _bricks[i] = b;
            var renderer = b.GetComponentInChildren<Renderer>();
            renderer.material = materials[colorIndex];
        }
    }

    private Vector2[] _PositionsByShapeIndex(int shapeIndex)
    {
        Vector2[] positions = new Vector2[4];
        int index = 0;
        for (int row = 0; row < _height; row++)
        {
            for (int column = 0; column < _width; column++)
            {
                int slot = _shapes[shapeIndex, row, column];
                if (slot == 1)
                {
                    Vector2 v = new Vector2(x + column, y - row);
                    positions[index] = v;
                    index++;
                }
            }
        }
        return positions;
    }

    public Vector2[] CurrentPositoins()
    {
        int index = _shapeIndex;
        return _PositionsByShapeIndex(index);
    }

    public Vector2[] RotatedPositions()
    {
        int index = _NextShapeIndex();
        return _PositionsByShapeIndex(index);
    }

    private void UpdateBricksPosition()
    {
        var positions = _PositionsByShapeIndex(_shapeIndex);
        for (int i = 0; i < 4; i++)
        {
            var b = _bricks[i];
            var p = positions[i];
            b.x = (int)p.x;
            b.y = (int)p.y;
        }
    }

    public void MoveDown()
    {
        y -= 1;
        Update();
    }

    public void MoveLeft()
    {
        x -= 1;
    }

    public void MoveRight()
    {
        x += 1;
    }

    private int _NextShapeIndex()
    {
        int result = _shapeIndex + 1;
        if (result >= _numOfShapes)
        {
            result = 0;
        }
        return result;
    }

    public void Rotate()
    {
        _shapeIndex = _NextShapeIndex();
    }

    private void log(object message)
    {
        Debug.Log(message);
    }

    public void Clear()
    {
        foreach (var b in _bricks)
        {
            Destroy(b.gameObject);
        }
        Destroy(gameObject);
    }
}
