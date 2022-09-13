using UnityEngine;

internal class IMino : TetriMino
{


    public IMino() : base()
    {
    }

    public IMino(Position2D pos) : base(pos)
    {
    }

    public IMino(Position2D pos, Direction rotation) : base(pos, rotation)
    {
    }

    public override void setObject()
    {
        GameObject obj = (GameObject)Resources.Load("Tetris\\IMino1");
        MinoObject = Instantiate(obj, ToVector3(MainPos), Quaternion.identity) as GameObject;

    }

    public override void setType()
    {
        type = Type.I;
    }

    public override void setStart()
    {
        MainPos = new Position2D(4, 20);
        Renew();
    }

    public override void Renew()
    {
        switch (Rotation)
        {
            case Direction.Up:
                APos = new Position2D(MainPos.getX() - 1, MainPos.getY());
                BPos = new Position2D(MainPos.getX() + 1, MainPos.getY());
                CPos = new Position2D(MainPos.getX() + 2, MainPos.getY());
                setBottom();
                break;
            case Direction.Right:
                APos = new Position2D(MainPos.getX(), MainPos.getY() + 1);
                BPos = new Position2D(MainPos.getX(), MainPos.getY() - 1);
                CPos = new Position2D(MainPos.getX(), MainPos.getY() - 2);
                setBottom();
                break;
            case Direction.Down:
                APos = new Position2D(MainPos.getX() + 1, MainPos.getY());
                BPos = new Position2D(MainPos.getX() - 1, MainPos.getY());
                CPos = new Position2D(MainPos.getX() - 2, MainPos.getY());
                setBottom();
                break;
            case Direction.Left:
                APos = new Position2D(MainPos.getX(), MainPos.getY() - 1);
                BPos = new Position2D(MainPos.getX(), MainPos.getY() + 1);
                CPos = new Position2D(MainPos.getX(), MainPos.getY() + 2);
                setBottom();
                break;
        }
    }

    public override TetriMino clone()
    {
        return new IMino(MainPos.clone(), Rotation);
    }
}