using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;




public class LockPick : MonoBehaviour
{
    public GameObject Easy;
    public GameObject Medium;
    public GameObject Hard;

    [SerializeField] public GameObject lockPickPin;

    [SerializeField] public GameObject[] EasyTumblers = new GameObject[4];
    private float[] easyPos = new float[4];

    [SerializeField] public GameObject[] MediumTumblers = new GameObject[6];
    private float[] mediumPos = new float[6];

    [SerializeField] public GameObject[] HardTumblers = new GameObject[8];
    private float[] hardPos = new float[8];

    private float[] PinPos;

    private List<int> tumblersOrder = new List<int>();
    private GameObject[] Tumblers;

    public Difficulty gameDifficulty;

    public PlayerSkill playerSkill;


    public float timeRemaining;

    public TextMeshProUGUI minuteText;
    public TextMeshProUGUI secondText;

    private bool win;
    private bool gameOver;

    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject gameOverText;

    private const float RELATIVEPOS = 8.25f;


    // Start is called before the first frame update
    // Setting some important variables
    // Setting Scene depends on Difficulty and Skill
    void Start()
    {
        win = false;
        gameOver = false;

        gameDifficulty = InputValue.gameDifficulty;

        playerSkill = InputValue.playerSkill;

        SettingDifficulty();

        SettingPinPos();

        CheckingPlayerSkill();        
    }


    // Setting Difficulty up to Input value
    private void SettingDifficulty()
    {
        if (gameDifficulty == Difficulty.EASY)
        {
            timeRemaining = 10.0f;
            Tumblers = EasyTumblers;
            PinPos = easyPos;
            Easy.gameObject.SetActive(true);
        }
        else if (gameDifficulty == Difficulty.MEDIUM)
        {
            timeRemaining = 15.0f;
            Tumblers = MediumTumblers;
            PinPos = mediumPos;
            Medium.gameObject.SetActive(true);
        }
        else if (gameDifficulty == Difficulty.HARD)
        {
            timeRemaining = 20.0f;
            Tumblers = HardTumblers;
            PinPos = hardPos;
            Hard.gameObject.SetActive(true);
        }
    }


    // Checking LockPicking Skill to preset the simulation
    private void CheckingPlayerSkill()
    {
        int numOfPresetTumblers = (int)playerSkill - 1;

        for (int i = 0; i < numOfPresetTumblers; i++)
        {
            Tumblers[tumblersOrder[0]].transform.position = new Vector3(Tumblers[tumblersOrder[0]].transform.position.x,
                                                                                    Tumblers[tumblersOrder[0]].transform.position.y + 1.5f,
                                                                                    Tumblers[tumblersOrder[0]].transform.position.z);

            tumblersOrder.RemoveAt(0);
        }
    }


    // Randomize order of tumblers' set to use to open the lock system
    private void GenerateTumblersOrder(int numOfTumblers)
    {
        int pos = UnityEngine.Random.Range(0, numOfTumblers);

        if(tumblersOrder.Count > 0)
        {
            bool existed = false;

            for (int i = 0; i < tumblersOrder.Count; i++)
            {
                if(tumblersOrder[i] == pos)
                {
                    existed = true;
                    break;
                }
            }

            if(existed == false)
            {
                tumblersOrder.Add(pos);
            }
            else
            {
                GenerateTumblersOrder(numOfTumblers);
            }
        }
        else
        {
            tumblersOrder.Add(pos);
        }    
    }


    // Setting Positions of LockPickPin relative to Tumbers' positions
    private void SettingPinPos()
    {
        if (Tumblers.Length > 0)
        {
            for (int i = 0; i < Tumblers.Length; i++)
            {
                PinPos[i] = Tumblers[i].transform.position.x - RELATIVEPOS;

                GenerateTumblersOrder(Tumblers.Length);
            }
        }
    }


    // Update is called once per frame
    // Checking win or lose condition
    void Update()
    {    

        if(gameOver == false)
        {
            TimeCounter();

            PinClicking();

            PinMoving();

            TumblersChecking();
        }
        else
        {
            if(win)
            {
                winText.gameObject.SetActive(true);
                StartCoroutine(ShowMessage(2.0f));
            }
            else
            {
                gameOverText.gameObject.SetActive(true);
                StartCoroutine(ShowMessage(2.0f));
            }
        }
    }


    // Show Message after finishing Game and change to Start scene
    IEnumerator ShowMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Start");
    }


    // Count down the time to set the game over state
    private void TimeCounter()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minute = (int)(timeRemaining) / 60;
            int second = (int)timeRemaining - (60 * minute);

            minuteText.text = (minute > 9) ? minute.ToString() : "0" + minute.ToString();
            secondText.text = (second > 9) ? second.ToString() : "0" + second.ToString();
        }
        else
        {
            gameOver = true;
        }
    }


    // Checking all Tumblers' postions to determine Player win or not
    private void TumblersChecking()
    {
        if(tumblersOrder.Count <= 0)
        {
            win = true;
            gameOver = true;
            Debug.Log("WIN!");
        }
    }

    
    // Process LockPickPin clicking
    private void PinClicking()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            lockPickPin.transform.Rotate(new Vector3(10.0f, 0.0f, 0.0f));

            if(tumblersOrder.Count > 0)
            {
                if (lockPickPin.transform.position.x == (Tumblers[tumblersOrder[0]].transform.position.x - RELATIVEPOS))
                {
                    Tumblers[tumblersOrder[0]].transform.position = new Vector3(Tumblers[tumblersOrder[0]].transform.position.x,
                                                                                    Tumblers[tumblersOrder[0]].transform.position.y + 1.5f,
                                                                                    Tumblers[tumblersOrder[0]].transform.position.z);

                    tumblersOrder.RemoveAt(0);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            lockPickPin.transform.Rotate(new Vector3(-10.0f, 0.0f, 0.0f));
        }
    }


    // Process LockPickPin movement
    private void PinMoving()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Debug.Log(PinPos.Length);

            int pos = -1;

            for (int i = 0; i < PinPos.Length; i++)
            {
                if (lockPickPin.transform.position.x == PinPos[i])
                {
                    pos = i;
                    break;
                }
            }

            if (pos > 0)
            {
                lockPickPin.transform.position = new Vector3(PinPos[pos - 1], lockPickPin.transform.position.y, lockPickPin.transform.position.z);
            }
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int pos = -1;
            for (int i = 0; i < PinPos.Length; i++)
            {
                if (lockPickPin.transform.position.x == PinPos[i])
                {
                    pos = i;
                    break;
                }
            }

            if (pos > -1 && pos < PinPos.Length - 1)
            {
                lockPickPin.transform.position = new Vector3(PinPos[pos + 1], lockPickPin.transform.position.y, lockPickPin.transform.position.z);
            }
        }
    }
}
