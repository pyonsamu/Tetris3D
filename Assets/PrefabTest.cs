using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    static float BlockSize = 1.0f;
    static float XMIN = BlockSize;
    static float XMAX = BlockSize * 11;
    static float YMIN = BlockSize;
    static float YMAX = BlockSize * 22;
    static float SideTime = 0.05f;
    static float SideSmoothTime = 0.5f;
    static float LockTime = 1.0f;


    private GameObject TMino;
    private GameObject TMinoBlock;
    public GameObject ZMino;
    private GameObject TMinos;
    private GameObject TMinoBlocks;
    private GameObject minoD;
    private GameObject minoR;
    private GameObject minoL;

    private float DownTime = 1.0f;
    private Trigger trigger_D;
    private Trigger trigger_L;
    private Trigger trigger_R;
    private float lockcount = LockTime;

    private Dictionary<Direction, float> KeyCount = new Dictionary<Direction, float>()
    {
        {Direction.Up, 0.0f},
        {Direction.Right, SideSmoothTime},
        {Direction.Down, 0.0f},
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

    // Start is called before the first frame update
    void Start()
    {
        TMino = (GameObject)Resources.Load("Tetris\\TMino1");
        TMinoBlock = (GameObject)Resources.Load("Tetris\\MinoBlock1");
        TMinos = Instantiate(TMino,new Vector3(XMIN+BlockSize*5,YMAX-BlockSize*2,0.0f), Quaternion.identity) as GameObject;
        minoD = Instantiate(TMino, new Vector3(XMIN + BlockSize * 5, YMAX - BlockSize * 2, 0.0f), Quaternion.identity) as GameObject;
        minoR = Instantiate(TMino, new Vector3(XMIN + BlockSize * 5, YMAX - BlockSize * 2, 0.0f), Quaternion.identity) as GameObject;
        minoL = Instantiate(TMino, new Vector3(XMIN + BlockSize * 5, YMAX - BlockSize * 2, 0.0f), Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (TMinos==null)
        {
            return;
        }
        DownTime -= Time.deltaTime;

        if (DownTime <= 0)
        {
            MoveDown(TMinos);
            if (trigger_D)
            {
                lockcount -= 0.1f;
            }
            else
            {
                lockcount = LockTime;
            }
            if (lockcount < 0)
            {
                TMinoBlocks = Instantiate(TMinoBlock, new Vector3(TMinos.transform.position.x, TMinos.transform.position.y, TMinos.transform.position.z), Quaternion.identity) as GameObject;
                TMinoBlocks = Instantiate(TMinoBlock, new Vector3(TMinos.transform.position.x-BlockSize, TMinos.transform.position.y, TMinos.transform.position.z), Quaternion.identity) as GameObject;
                TMinoBlocks = Instantiate(TMinoBlock, new Vector3(TMinos.transform.position.x+BlockSize, TMinos.transform.position.y, TMinos.transform.position.z), Quaternion.identity) as GameObject;
                TMinoBlocks = Instantiate(TMinoBlock, new Vector3(TMinos.transform.position.x, TMinos.transform.position.y+BlockSize, TMinos.transform.position.z), Quaternion.identity) as GameObject;
                Destroy(TMinos);
            }

            CheckDown(TMinos);

            DownTime = 0.1f;
        }

        Action(TMinos);

        CheckAround(TMinos);

       

        KeyCheck();
    }

    public void MoveDown(GameObject obj)
    {
        if (trigger_D == null)
        {
            return;
        }
        if (!trigger_D.overlap)
        {
            obj.transform.Translate(0.0f, -BlockSize, 0.0f);
            lockcount = LockTime;
        }
        Destroy(trigger_D.gameObject);
    }

    public void MoveLeft(GameObject obj)
    {
        if (KeyState[Direction.Left] == Direction.Hold)
        {
            if (KeyCount[Direction.Left] < 0)
            {
                if (trigger_L == null)
                {
                    return;
                }
                if (!trigger_L.overlap)
                {
                    obj.transform.Translate(-BlockSize, 0.0f, 0.0f);
                }
                KeyCount[Direction.Left] = SideTime;
            }
            KeyCount[Direction.Left] -= Time.deltaTime;
        }
        if (trigger_L != null)
        {
            Destroy(trigger_L.gameObject);
        }
    }

    public void MoveRight(GameObject obj)
    {
        if (KeyState[Direction.Right] == Direction.Hold)
        {
            if (KeyCount[Direction.Right] < 0)
            {
                if (trigger_R == null)
                {
                    return;
                }
                if (!trigger_R.overlap)
                {
                    obj.transform.Translate(BlockSize, 0.0f, 0.0f);
                }
                KeyCount[Direction.Right] = SideTime;
            }
            KeyCount[Direction.Right] -= Time.deltaTime;
        }
        if (trigger_R != null)
        {
            Destroy(trigger_R.gameObject);
        }
    }

    public void RotateRight(GameObject obj)
    {
        if (KeyState[Direction.RotateR] == Direction.Down)
        {
            Rotation = Direction.Up;
            obj.transform.localRotation = new Quaternion(0.0f,0.0f,90.0f,90.0f);
        }
            
    }

    public void RotateLeft(GameObject obj)
    {
        if (KeyState[Direction.RotateL] == Direction.Down)
        {
            Rotation = Direction.Up;
            obj.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        }
            
    }

    public void CheckAround(GameObject obj)
    {
        CheckLeft(obj);
        CheckRight(obj);
    }

    public void CheckDown(GameObject obj)
    {
        GameObject mino = Instantiate(obj, obj.transform.position, Quaternion.identity) as GameObject;
        trigger_D = mino.AddComponent<Trigger>();
        mino.GetComponent<Renderer>().enabled = false;
        mino.transform.Translate(0.0f, -BlockSize, 0.0f);
    }

    public void CheckLeft(GameObject obj)
    {
        GameObject mino = Instantiate(obj, obj.transform.position, Quaternion.identity) as GameObject;
        trigger_L = mino.AddComponent<Trigger>();
        mino.GetComponent<Renderer>().enabled = false;
        mino.transform.Translate(-BlockSize, 0.0f, 0.0f);
    }

    public void CheckRight(GameObject obj)
    {
        GameObject mino = Instantiate(obj, obj.transform.position, Quaternion.identity) as GameObject;
        trigger_R = mino.AddComponent<Trigger>();
        mino.GetComponent<Renderer>().enabled = false;
        mino.transform.Translate(BlockSize, 0.0f, 0.0f);
    }

    public void Action(GameObject obj)
    {
        MoveLeft(obj);
        MoveRight(obj);
        RotateRight(obj);
        RotateLeft(obj);
    }

    public void KeyCheck()
    {
        KeyUpCheck();
        KeyHoldCheck();
        KeyDownCheck();
    }

    public void KeyUpCheck()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            KeyState[Direction.Up] = Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            KeyState[Direction.Right] = Direction.Up;
            KeyCount[Direction.Right] = SideSmoothTime;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            KeyState[Direction.Down] = Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            KeyState[Direction.Left] = Direction.Up;
            KeyCount[Direction.Left] = SideSmoothTime;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            KeyState[Direction.RotateR] = Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            KeyState[Direction.RotateL] = Direction.Up;
        }
    }

    public void KeyHoldCheck()
    {
        if (Input.GetKey(KeyCode.W) && KeyState[Direction.Up] == Direction.Down)
        {
            KeyState[Direction.Up] = Direction.Hold;
        }
        if (Input.GetKey(KeyCode.D) && KeyState[Direction.Right] == Direction.Down)
        {
            KeyState[Direction.Right] = Direction.Hold;
        }
        if (Input.GetKey(KeyCode.S) && KeyState[Direction.Down] == Direction.Down)
        {
            KeyState[Direction.Down] = Direction.Hold;
        }
        if (Input.GetKey(KeyCode.A) && KeyState[Direction.Left] == Direction.Down)
        {
            KeyState[Direction.Left] = Direction.Hold;
        }
        if (Input.GetKey(KeyCode.R) && KeyState[Direction.RotateR] == Direction.Down)
        {
            KeyState[Direction.RotateR] = Direction.Hold;
        }
        if (Input.GetKey(KeyCode.L) && KeyState[Direction.RotateL] == Direction.Down)
        {
            KeyState[Direction.RotateL] = Direction.Hold;
        }
    }

    public void KeyDownCheck()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            KeyState[Direction.Up] = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            KeyState[Direction.Right] = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyState[Direction.Down] = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyState[Direction.Left] = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            KeyState[Direction.RotateR] = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.L))
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
}
