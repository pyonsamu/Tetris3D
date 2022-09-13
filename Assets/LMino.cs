﻿using UnityEngine;

internal class LMino : TetriMino
{


    public LMino() : base()
    {
    }

    public LMino(Position2D pos) : base(pos)
    {
    }

    public LMino(Position2D pos, Direction rotation) : base(pos, rotation)
    {
    }

    public override void setObject()
    {
        GameObject obj = (GameObject)Resources.Load("Tetris\\LMino1");
        MinoObject = Instantiate(obj, ToVector3(MainPos), Quaternion.identity) as GameObject;

    }

    public override void setType()
    {
        type = Type.L;
    }

    public override void setStart()
    {
        MainPos = new Position2D(4, 19);
        Renew();
    }

    public override void Renew()
    {
        switch (Rotation)
        {
            case Direction.Up:
                APos = new Position2D(MainPos.getX() + 1, MainPos.getY() + 1);
                BPos = new Position2D(MainPos.getX() - 1, MainPos.getY());
                CPos = new Position2D(MainPos.getX() + 1, MainPos.getY());
                setBottom();
                break;
            case Direction.Right:
                APos = new Position2D(MainPos.getX() + 1, MainPos.getY() - 1);
                BPos = new Position2D(MainPos.getX(), MainPos.getY() + 1);
                CPos = new Position2D(MainPos.getX(), MainPos.getY() - 1);
                setBottom();
                break;
            case Direction.Down:
                APos = new Position2D(MainPos.getX() - 1, MainPos.getY() - 1);
                BPos = new Position2D(MainPos.getX() + 1, MainPos.getY());
                CPos = new Position2D(MainPos.getX() - 1, MainPos.getY());
                setBottom();
                break;
            case Direction.Left:
                APos = new Position2D(MainPos.getX() - 1, MainPos.getY() + 1);
                BPos = new Position2D(MainPos.getX(), MainPos.getY() - 1);
                CPos = new Position2D(MainPos.getX(), MainPos.getY() + 1);
                setBottom();
                break;
        }
    }

    public override TetriMino clone()
    {
        return new LMino(MainPos.clone(), Rotation);
    }
}