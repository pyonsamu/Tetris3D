using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrisMain : MonoBehaviour
{
    public const float BlockSize = 1.0f;
    public const int STAGE_WIDTH = 10;
    public const int STAGE_HEIGHT = 21;
    public const float XMIN = BlockSize;
    public const float XMAX = BlockSize * 11;
    public const float YMIN = BlockSize;
    public const float YMAX = BlockSize * 22;
    public const float DownTime = 0.5f;
    public const float SideTime = 0.01f;
    public const float SideSmoothTime = 0.1f;
    public const float LockTime = 1.0f;
    public const float DeleteTime = 1.0f;
    public const int MARGIN = 10;

    public const KeyCode KEY_UP = KeyCode.UpArrow;
    public const KeyCode KEY_RIGHT = KeyCode.RightArrow;
    public const KeyCode KEY_DOWN = KeyCode.DownArrow;
    public const KeyCode KEY_LEFT = KeyCode.LeftArrow;
    public const KeyCode KEY_ROTATE_R = KeyCode.X;
    public const KeyCode KEY_ROTATE_L = KeyCode.Z;


    //private GameObject TMino;
    private GameObject TMinoBlock;
    public GameObject ZMino;
    private GameObject TMinos;
    private GameObject TMinoBlocks;
    private bool[,] Blocks = new bool[35,25];
    private GameObject[,] DefaultBlocks = new GameObject[21,10];
    private GameObject[,] ObjectBlocks = new GameObject[21, 10];
    private Position2D MainPos = new Position2D(5,10);
    private TetriMino ActiveMino;
    private Queue<int> Minos = new Queue<int>();
    private Stack<int> LineNums = new Stack<int>();
    private Scene scene = Scene.Title;


    private float downcount = DownTime;
    private float lockcount = LockTime;
    private float deletecount = DeleteTime;
    private int bottompast = 0;

    private Dictionary<Direction, float> KeyCount = new Dictionary<Direction, float>()
    {
        {Direction.Up, 0.0f},
        {Direction.Right, SideSmoothTime},
        {Direction.Down, DownTime/2},
        {Direction.Left, SideSmoothTime}
    };

    private Direction Rotation = Direction.Up;
    private Dictionary<Direction, Direction> KeyState = new Dictionary<Direction, Direction>()
    {
        {Direction.Up, Direction.Default},
        {Direction.Right, Direction.Default},
        {Direction.Down, Direction.Default},
        {Direction.Left, Direction.Default},
        {Direction.RotateR, Direction.Default},
        {Direction.RotateL, Direction.Default}
    };

    enum Direction
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

    enum Scene
    {
        Title,
        Active,
        Deleting,
        GameOver
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<Blocks.GetLength(0);i++)
        {
            for(int j=0;j<Blocks.GetLength(1);j++)
            {
                Blocks[i, j] = true;
            }
        }
        for (int i = 10; i < Blocks.GetLength(0); i++)
        {
            for (int j = 10; j < 20; j++)
            {
                Blocks[i, j] = false;
            }
        }

        GameObject obj = (GameObject)Resources.Load("Tetris\\DefaultBlock");
        for (int i = 0; i < DefaultBlocks.GetLength(0); i++)
        {
            for (int j = 0; j < DefaultBlocks.GetLength(1); j++)
            {
                DefaultBlocks[i, j] = Instantiate(obj, ToVector3(new Position2D(j, i)), Quaternion.identity) as GameObject;
            }
        }

        //ActiveMino = new IMino(new Position2D(5,19), TetriMino.Direction.Up);
        setMinos();
        //ActiveMino.setObject();
        scene = Scene.Active;
    }

    // Update is called once per frame
    void Update()
    {
        if (scene == Scene.Active)
        {
            downcount -= Time.deltaTime;
            lockcount -= Time.deltaTime;

            if (ActiveMino.getObject() == null)
            {
                setMinos();
            }

            if (downcount <= 0)
            {
                MoveDown();

                downcount = DownTime;
            }

            ScanStage();

            Action();

            KeyCheck();

            if (bottompast != ActiveMino.getBottom() || CheckDown())
            {
                lockcount = LockTime;
            }
            bottompast = ActiveMino.getBottom();

            if (lockcount <= 0)
            {
                LockMino();
            }

            CheckLine();
        }

        if (scene == Scene.Deleting)
        {
            deletecount -= Time.deltaTime;
            if (deletecount < 0)
            {
                DeleteLine();
                deletecount = DeleteTime;
            }
        }
    }

    public void MoveDown()
    {
        if (CheckDown())
        {
            ActiveMino.MoveDown();
            ActiveMino.Draw();
        }
    }

    public void MoveLeft()
    {
        if (CheckLeft())
        {
            ActiveMino.MoveLeft();
            ActiveMino.Draw();
        }
    }

    public void MoveRight()
    {
        if (CheckRight())
        {
            ActiveMino.MoveRight();
            ActiveMino.Draw();
        }
    }

    public void HardDrop()
    {
        while (CheckDown())
        {
            ActiveMino.MoveDown();
            ActiveMino.Draw();
        }
        LockMino();
    }

    public void RotateRight()
    {
        if (CheckRotateR(0))
        {
            ActiveMino.SuperRotateR(0);
        }
        else if (CheckRotateR(1))
        {
            ActiveMino.SuperRotateR(1);
        }
        else if (CheckRotateR(2))
        {
            ActiveMino.SuperRotateR(2);
        }
        else if (CheckRotateR(3))
        {
            ActiveMino.SuperRotateR(3);
        }
        else if (CheckRotateR(4))
        {
            ActiveMino.SuperRotateR(4);
        }
        ActiveMino.Draw();
    }

    public void RotateLeft()
    {
        if (CheckRotateL(0))
        {
            ActiveMino.SuperRotateL(0);
        }
        else if (CheckRotateL(1))
        {
            ActiveMino.SuperRotateL(1);
        }
        else if (CheckRotateL(2))
        {
            ActiveMino.SuperRotateL(2);
        }
        else if (CheckRotateL(3))
        {
            ActiveMino.SuperRotateL(3);
        }
        else if (CheckRotateL(4))
        {
            ActiveMino.SuperRotateL(4);
        }
        ActiveMino.Draw();
    }

    public bool CheckDown()
    {
        TetriMino dummymino = ActiveMino.clone();
        dummymino.MoveDown();
        return CheckBlocks(dummymino);
    }

    public bool CheckLeft()
    {
        TetriMino dummymino = ActiveMino.clone();
        dummymino.MoveLeft();
        return CheckBlocks(dummymino);
    }

    public bool CheckRight()
    {
        TetriMino dummymino = ActiveMino.clone();
        dummymino.MoveRight();
        return CheckBlocks(dummymino);
    }

    public bool CheckRotateR(int num)
    {
        TetriMino dummymino = ActiveMino.clone();
        dummymino.SuperRotateR(num);
        return CheckBlocks(dummymino);
    }

    public bool CheckRotateL(int num)
    {
        TetriMino dummymino = ActiveMino.clone();
        dummymino.SuperRotateL(num);
        return CheckBlocks(dummymino);
    }

    public void Action()
    {
        if(KeyState[Direction.Left] == Direction.Down)
        {
            MoveLeft();
        }

        if (KeyState[Direction.Left] == Direction.Hold)
        {
            if (KeyCount[Direction.Left] < 0)
            {
                MoveLeft();
                KeyCount[Direction.Left] = SideTime;
            }
            KeyCount[Direction.Left] -= Time.deltaTime;
        }

        if (KeyState[Direction.Right] == Direction.Down)
        {
            MoveRight();
        }

        if (KeyState[Direction.Right] == Direction.Hold)
        {
            if (KeyCount[Direction.Right] < 0)
            {
                MoveRight();
                KeyCount[Direction.Right] = SideTime;
            }
            KeyCount[Direction.Right] -= Time.deltaTime;
        }

        if(KeyState[Direction.Up] == Direction.Down)
        {
            HardDrop();
        }

        if (KeyState[Direction.Down] == Direction.Hold)
        {
            downcount -= Time.deltaTime*7;
        }

        if (KeyState[Direction.RotateR] == Direction.Down)
        {
            RotateRight();
        }

        if (KeyState[Direction.RotateL] == Direction.Down)
        {
            RotateLeft();
        }
    }

    private bool CheckBlocks(TetriMino mino)
    {
        bool Main = Blocks[mino.getMainPos().getY() + MARGIN, mino.getMainPos().getX() + MARGIN];
        bool A = Blocks[mino.getAPos().getY() + MARGIN, mino.getAPos().getX() + MARGIN];
        bool B = Blocks[mino.getBPos().getY() + MARGIN, mino.getBPos().getX() + MARGIN];
        bool C = Blocks[mino.getCPos().getY() + MARGIN, mino.getCPos().getX() + MARGIN];
        return !(Main || A || B || C);
    }

    public void CheckLine()
    {
        bool flag = true;
        for(int i = MARGIN; i < STAGE_HEIGHT + MARGIN; i++)
        {
            flag = true;
            for(int j = MARGIN; j < STAGE_WIDTH + MARGIN; j++)
            {
                flag = flag && Blocks[i, j];
            }
            if (flag)
            {
                LineNums.Push(i - MARGIN);
                scene = Scene.Deleting;
            }
            
        }
        
    }

    public void DeleteLine()
    {
        Debug.Log(LineNums.Count);
        while (LineNums.Count > 0)
        {
            int num = LineNums.Pop();
            Debug.Log(num);
            for (int j = 0; j < STAGE_WIDTH; j++) {
                GameObject obj = ObjectBlocks[num, j];
                Destroy(obj);
                DefaultBlocks[num, j].GetComponent<DefaultBlock>().OnTriggerExit(null);
            }
            for(int i = num + 1; i < STAGE_HEIGHT; i++)
            {
                for(int j = 0; j < STAGE_WIDTH; j++)
                {
                    GameObject obj = ObjectBlocks[i, j];
                    if (obj != null)
                    {
                        obj.transform.position = ToVector3(new Position2D(j, i - 1));
                        ObjectBlocks[i - 1, j] = obj;
                    }
                    
                }
            }
        }
        scene = Scene.Active;
    }

    public void ScanStage()
    {
        for (int i = 0; i < DefaultBlocks.GetLength(0); i++)
        {
            for (int j = 0; j < DefaultBlocks.GetLength(1); j++)
            {
                Blocks[i + MARGIN, j + MARGIN] = DefaultBlocks[i, j].GetComponent<DefaultBlock>().overlap;
            }
        }
    }

    public void LockMino()
    {
        ActiveMino.Lock();

        GameObject obj = (GameObject)Resources.Load("Tetris\\MinoBlock1");

        setObjectBlocks(ActiveMino.getMainPos(), obj);
        setObjectBlocks(ActiveMino.getAPos(), obj);
        setObjectBlocks(ActiveMino.getBPos(), obj);
        setObjectBlocks(ActiveMino.getCPos(), obj);
    }

    private void setObjectBlocks(Position2D pos, GameObject obj)
    {
        Debug.Log(pos.getX()+" : "+ pos.getY());
        ObjectBlocks[pos.getY(), pos.getX()] = Instantiate(obj, ToVector3(pos), Quaternion.identity) as GameObject;
    }

    public void setMinos()
    {
        if (Minos.Count() < 6)
        {
            List<int> list = GetUniqRandomNumbers(0, 6, 7).ToList();
            foreach(int i in list)
            {
                Minos.Enqueue(i);
            }
        }
        setActiveMino();
        setNextMino();
    }

    public void setActiveMino()
    {
        ActiveMino = ToTetriMino(Minos.Dequeue());
        ActiveMino.setStart();
        ActiveMino.setObject();
    }

    public void setNextMino()
    {

    }

    public void KeyCheck()
    {
        KeyUpCheck();
        KeyHoldCheck();
        KeyDownCheck();
    }

    public void KeyUpCheck()
    {
        if (Input.GetKeyUp(KEY_UP))
        {
            KeyState[Direction.Up] = Direction.Up;
        }
        if (Input.GetKeyUp(KEY_RIGHT))
        {
            KeyState[Direction.Right] = Direction.Up;
            KeyCount[Direction.Right] = SideSmoothTime;
        }
        if (Input.GetKeyUp(KEY_DOWN))
        {
            KeyState[Direction.Down] = Direction.Up;
        }
        if (Input.GetKeyUp(KEY_LEFT))
        {
            KeyState[Direction.Left] = Direction.Up;
            KeyCount[Direction.Left] = SideSmoothTime;
        }
        if (Input.GetKeyUp(KEY_ROTATE_R))
        {
            KeyState[Direction.RotateR] = Direction.Up;
        }
        if (Input.GetKeyUp(KEY_ROTATE_L))
        {
            KeyState[Direction.RotateL] = Direction.Up;
        }
    }

    public void KeyHoldCheck()
    {
        if (Input.GetKey(KEY_UP))
        {
            if (KeyState[Direction.Up] == Direction.Down)
            {
                KeyState[Direction.Up] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.Up] = Direction.Up;
        }
        if (Input.GetKey(KEY_RIGHT))
        {
            if (KeyState[Direction.Right] == Direction.Down)
            {
                KeyState[Direction.Right] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.Right] = Direction.Up;
        }
        if (Input.GetKey(KEY_DOWN))
        {
            if (KeyState[Direction.Down] == Direction.Down)
            {
                KeyState[Direction.Down] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.Down] = Direction.Up;
        }
        if (Input.GetKey(KEY_LEFT))
        {
            if (KeyState[Direction.Left] == Direction.Down)
            {
                KeyState[Direction.Left] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.Left] = Direction.Up;
        }
        if (Input.GetKey(KEY_ROTATE_R))
        {
            if (KeyState[Direction.RotateR] == Direction.Down)
            {
                KeyState[Direction.RotateR] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.RotateR] = Direction.Up;
        }
        if (Input.GetKey(KEY_ROTATE_L))
        {
            if (KeyState[Direction.RotateL] == Direction.Down)
            {
                KeyState[Direction.RotateL] = Direction.Hold;
            }
        }
        else
        {
            KeyState[Direction.RotateL] = Direction.Up;
        }
    }

    public void KeyDownCheck()
    {
        if (Input.GetKeyDown(KEY_UP))
        {
            KeyState[Direction.Up] = Direction.Down;
        }
        else
        {

        }
        if (Input.GetKeyDown(KEY_RIGHT))
        {
            KeyState[Direction.Right] = Direction.Down;
        }
        if (Input.GetKeyDown(KEY_DOWN))
        {
            KeyState[Direction.Down] = Direction.Down;
        }
        if (Input.GetKeyDown(KEY_LEFT))
        {
            KeyState[Direction.Left] = Direction.Down;
        }
        if (Input.GetKeyDown(KEY_ROTATE_R))
        {
            KeyState[Direction.RotateR] = Direction.Down;
        }
        if (Input.GetKeyDown(KEY_ROTATE_L))
        {
            KeyState[Direction.RotateL] = Direction.Down;
        }
    }

    private string ToString(Direction d)
    {
        string str;
        switch (d)
        {
            case Direction.Up:
                str = "Up";
                break;
            case Direction.Right:
                str = "Right";
                break;
            case Direction.Down:
                str = "Down";
                break;
            case Direction.Left:
                str = "Left";
                break;
            case Direction.Hold:
                str = "Hold";
                break;
            case Direction.Default:
                str = "Default";
                break;
            default:
                str = "null";
                break;
        }
        return str;
    }

    private Vector3 ToVector3(Position2D pos)
    {
        return new Vector3(XMIN + pos.getX()*BlockSize, YMIN + pos.getY()*BlockSize, 0.0f);
    }

    private TetriMino ToTetriMino(int num)
    {
        TetriMino mino;
        switch (1)
        {
            case 0:
                mino = new TMino();
                break;
            case 1:
                mino = new IMino();
                break;
            case 2:
                mino = new OMino();
                break;
            case 3:
                mino = new SMino();
                break;
            case 4:
                mino = new ZMino();
                break;
            case 5:
                mino = new JMino();
                break;
            case 6:
                mino = new LMino();
                break;
            default:
                mino = new TMino();
                break;
        }
        return mino;
    }


    static void Swap(ref int m, ref int n)
    {
        int work = m;
        m = n;
        n = work;
    }

    static IEnumerable<int> GetUniqRandomNumbers(int rangeBegin, int rangeEnd, int count)
    {
        // 指定された範囲の整数を格納できる配列を用意する
        int[] work = new int[rangeEnd - rangeBegin + 1];

        // 配列を初期化する
        for (int n = rangeBegin, i = 0; n <= rangeEnd; n++, i++)
            work[i] = n;

        // ランダムに取り出しては先頭から順に置いていく（count回繰り返す）
        var rnd = new System.Random();
        for (int resultPos = 0; resultPos < count; resultPos++)
        {
            // （resultPosを含めて）resultPosの後ろからランダムに1つ選ぶ
            int nextResultPos = rnd.Next(resultPos, work.Length);

            // nextResultPosの値をresultPosと入れ替える
            Swap(ref work[resultPos], ref work[nextResultPos]);
        }

        return work.Take(count); // workの先頭からcount個を返す
    }
}
