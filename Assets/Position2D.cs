

internal class Position2D
{
    private int x;
    private int y;

    public Position2D()
    {
        this.x = 0;
        this.y = 0;
    }
    public Position2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int getX()
    {
        return this.x;
    }

    public int getY()
    {
        return this.y;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    public void MoveX(int x)
    {
        this.x += x;
    }

    public void MoveY(int y)
    {
        this.y += y;
    }

    public void MoveXY(int x, int y)
    {
        MoveX(x);
        MoveY(y);
    }
    
    public Position2D clone()
    {
        return new Position2D(x,y);
    }

}