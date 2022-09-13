using UnityEngine;

internal abstract class TetriMino : MonoBehaviour
{
    protected GameObject MinoObject;
    static float BlockSize = 1.0f;
    static float XMIN = BlockSize;
    static float XMAX = BlockSize * 11;
    static float YMIN = BlockSize;
    static float YMAX = BlockSize * 22;

    protected Position2D MainPos;
    protected Position2D APos;
    protected Position2D BPos;
    protected Position2D CPos;
    protected int bottom = 0;

    protected Direction Rotation;
    protected Type type;

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Hold,
        Default,
        RotateR,
        RotateL
    }

    public enum Type
    {
        T,
        Z,
        S,
        L,
        J,
        O,
        I
    }

    public abstract void setObject();

    public abstract void setType();

    public abstract void setStart();

    public abstract void Renew();

    public abstract TetriMino clone();

    public TetriMino() : this(new Position2D(0, 0))
    {
    }

    public TetriMino(Position2D pos) : this(pos, Direction.Up)
    {
    }

    public TetriMino(Position2D pos, Direction rotation)
    {
        setType();
        MainPos = pos;
        Rotation = rotation;
        Renew();
    }

    public GameObject getObject()
    {
        return MinoObject;
    }

    public Position2D getMainPos()
    {
        return MainPos;
    }

    public Position2D getAPos()
    {
        return APos;
    }

    public Position2D getBPos()
    {
        return BPos;
    }

    public Position2D getCPos()
    {
        return CPos;
    }

    public int getBottom()
    {
        return bottom;
    }

    public void setBottom()
    {
        bottom = min(MainPos.getY(), APos.getY(), BPos.getY(), CPos.getY());
    }

    public void MoveDown()
    {
        MainPos.MoveY(-1);
        Renew();
    }

    public void MoveRight()
    {
        MainPos.MoveX(1);
        Renew();
    }

    public void MoveLeft()
    {
        MainPos.MoveX(-1);
        Renew();
    }

    public void MoveXY(int x, int y)
    {
        MainPos.MoveXY(x,y);
        Renew();
    }

    public void RotateRight()
    {
        if (type == Type.O)
        {
            return;
        }
        switch (Rotation)
        {
            case Direction.Up:
                Rotation = Direction.Right;
                if (type == Type.I)
                {
                    MainPos.MoveXY(1, 0);
                }
                break;
            case Direction.Right:
                Rotation = Direction.Down;
                if (type == Type.I)
                {
                    MainPos.MoveXY(0, -1);
                }
                break;
            case Direction.Down:
                Rotation = Direction.Left;
                if (type == Type.I)
                {
                    MainPos.MoveXY(-1, 0);
                }
                break;
            case Direction.Left:
                Rotation = Direction.Up;
                if (type == Type.I)
                {
                    MainPos.MoveXY(0, 1);
                }
                break;
        }
        Renew();
    }

    public void RotateLeft()
    {
        if(type == Type.O)
        {
            return;
        }
        switch (Rotation)
        {
            case Direction.Up:
                Rotation = Direction.Left;
                if (type == Type.I)
                {
                    MainPos.MoveXY(0, -1);
                }
                break;
            case Direction.Left:
                Rotation = Direction.Down;
                if (type == Type.I)
                {
                    MainPos.MoveXY(1, 0);
                }
                break;
            case Direction.Down:
                Rotation = Direction.Right;
                if (type == Type.I)
                {
                    MainPos.MoveXY(0, 1);
                }
                break;
            case Direction.Right:
                Rotation = Direction.Up;
                if (type == Type.I)
                {
                    MainPos.MoveXY(-1, 0);
                }
                break;
        }
        Renew();
    }

    public void SuperRotateR(int num)
    {
        if (type == Type.O)
        {
            return;
        }
        RotateRight();
        switch (Rotation)
        {
            case Direction.Up:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-2, 0);
                            break;
                        case 2:
                            MoveXY(1, 0);
                            break;
                        case 3:
                            MoveXY(1, -2);
                            break;
                        case 4:
                            MoveXY(-2, 1);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(-1, -1);
                            break;
                        case 3:
                            MoveXY(0, 2);
                            break;
                        case 4:
                            MoveXY(-1, 2);
                            break;
                    }
                }
                break;
            case Direction.Right:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-2, 0);
                            break;
                        case 2:
                            MoveXY(1, 0);
                            break;
                        case 3:
                            MoveXY(-2, -1);
                            break;
                        case 4:
                            MoveXY(1, 2);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(-1, 1);
                            break;
                        case 3:
                            MoveXY(0, -2);
                            break;
                        case 4:
                            MoveXY(-1, -2);
                            break;
                    }
                }
                break;
            case Direction.Down:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(2, 0);
                            break;
                        case 3:
                            MoveXY(-1, 2);
                            break;
                        case 4:
                            MoveXY(2, -1);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(1, -1);
                            break;
                        case 3:
                            MoveXY(0, 2);
                            break;
                        case 4:
                            MoveXY(1, 2);
                            break;
                    }
                }
                break;
            case Direction.Left:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(2, 0);
                            break;
                        case 2:
                            MoveXY(-1, 0);
                            break;
                        case 3:
                            MoveXY(2, 1);
                            break;
                        case 4:
                            MoveXY(-1, -2);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(1, 1);
                            break;
                        case 3:
                            MoveXY(0, -2);
                            break;
                        case 4:
                            MoveXY(1, -2);
                            break;
                    }
                }
                break;
        }
    }

    public void SuperRotateL(int num)
    {
        if (type == Type.O)
        {
            return;
        }
        RotateLeft();
        switch (Rotation)
        {
            case Direction.Up:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(2, 0);
                            break;
                        case 2:
                            MoveXY(-1, 0);
                            break;
                        case 3:
                            MoveXY(2, 1);
                            break;
                        case 4:
                            MoveXY(-1, -2);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(1, -1);
                            break;
                        case 3:
                            MoveXY(0, 2);
                            break;
                        case 4:
                            MoveXY(1, 2);
                            break;
                    }
                }
                break;
            case Direction.Right:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(-2, 0);
                            break;
                        case 3:
                            MoveXY(1, -2);
                            break;
                        case 4:
                            MoveXY(-2, 1);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(-1, 1);
                            break;
                        case 3:
                            MoveXY(0, -2);
                            break;
                        case 4:
                            MoveXY(-1, -2);
                            break;
                    }
                }
                break;
            case Direction.Down:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(-2, 0);
                            break;
                        case 3:
                            MoveXY(-2, -1);
                            break;
                        case 4:
                            MoveXY(1, 2);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(-1, -1);
                            break;
                        case 3:
                            MoveXY(0, 2);
                            break;
                        case 4:
                            MoveXY(-1, 2);
                            break;
                    }
                }
                break;
            case Direction.Left:
                if (type == Type.I)
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(-1, 0);
                            break;
                        case 2:
                            MoveXY(2, 0);
                            break;
                        case 3:
                            MoveXY(-1, 2);
                            break;
                        case 4:
                            MoveXY(2, -1);
                            break;
                    }
                }
                else
                {
                    switch (num)
                    {
                        case 0:
                            MoveXY(0, 0);
                            break;
                        case 1:
                            MoveXY(1, 0);
                            break;
                        case 2:
                            MoveXY(1, 1);
                            break;
                        case 3:
                            MoveXY(0, -2);
                            break;
                        case 4:
                            MoveXY(1, -2);
                            break;
                    }
                }
                break;
        }
    }

    public void Draw()
    {
        int gapx = 0;
        int gapy = 0;
        int rotation = 0;
        switch (Rotation)
        {
            case Direction.Up:
                rotation = 0;
                gapx = 0;
                gapy = 0;
                break;
            case Direction.Right:
                rotation = -90;
                gapx = 0;
                gapy = 1;
                break;
            case Direction.Down:
                rotation = 180;
                gapx = 1;
                gapy = 1;
                break;
            case Direction.Left:
                rotation = 90;
                gapx = 1;
                gapy = 0;
                break;
        }
        MinoObject.transform.position = ToVector3(new Position2D(MainPos.getX()+gapx, MainPos.getY() + gapy));
        MinoObject.transform.rotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));
        
    }

    public void Lock()
    {
        Destroy(MinoObject);
        MinoObject = null;
        GameObject obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
        switch(type)
        {
            case Type.T:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.I:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.O:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.Z:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.S:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.L:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
            case Type.J:
                obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");
                break;
        }
    }

    private int min(int m, int a, int b, int c)
    {
        int r = m;
        if (r > a)
        {
            r = a;
        }
        if (r > b)
        {
            r = b;
        }
        if (r > c)
        {
            r = c;
        }
        return r;
    }

    protected Vector3 ToVector3(Position2D pos)
    {
        return new Vector3(XMIN + pos.getX() * BlockSize, YMIN + pos.getY() * BlockSize, 0.0f);
    }
}