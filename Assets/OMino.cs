using UnityEngine;

internal class OMino : TetriMino
{


    public OMino() : base()
    {
    }

    public OMino(Position2D pos) : base(pos)
    {
    }

    public OMino(Position2D pos, Direction rotation) : base(pos, rotation)
    {
    }

    public override void setObject()
    {
        GameObject obj = (GameObject)Resources.Load("Tetris\\OMino1");
        MinoObject = Instantiate(obj, ToVector3(MainPos), Quaternion.identity) as GameObject;

    }

    public override void setType()
    {
        type = Type.O;
    }

    public override void setStart()
    {
        MainPos = new Position2D(4, 19);
        Renew();
    }

    public override void Renew()
    {
        APos = new Position2D(MainPos.getX(), MainPos.getY() + 1);
        BPos = new Position2D(MainPos.getX() + 1, MainPos.getY() + 1);
        CPos = new Position2D(MainPos.getX() + 1, MainPos.getY());
        setBottom();
    }

    public override TetriMino clone()
    {
        return new OMino(MainPos.clone(), Rotation);
    }
}