using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class GameManager : NonPersistentSingleton<GameManager>
{
    [SerializeField] string raceType;
    private string TIME_ATTACK = "timer";
    [SerializeField] float timeToBeat;
    private string VERSUS = "versus";
    [SerializeField] GameObject playerParent;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject playerAI;

    [SerializeReference] private PlayerManager playerManager;
    [SerializeField] TMP_Text timer;
    public bool isRaceComplete { get; set; }
    public bool isRaceStart { get; set; }
    private int playerCount {  get; set; }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        //StartCoroutine(GetRaceType());
        //timer.text = timeToBeat.ToString();
        //playerManager = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<PlayerManager>();
    }

    private void Start()
    {
       //GetPlayers(); 
       // if(PlayerPrefs.GetInt("TrackNum") == 1 && PlayerPrefs.GetString("GameMode") == "timed")
       // {
       //     timeToBeat = 50;
       // }
       // else
       // {
       //     timeToBeat = 100;
       // }
       // timer.text = timeToBeat.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isRaceComplete) {
        //    if (raceType == TIME_ATTACK)
        //    {
        //        TimeAttack();
        //    }
        //}
    }
    void GetPlayers()
    {
        playerCount = PlayerPrefs.GetInt("PlayerCount");
        for (int i = 0; i < playerCount; i++)
        {
            if (i == 0)
            {
                GameObject playerOne = Instantiate(player1, new Vector3(-2, -35, 0), Quaternion.identity, playerParent.transform);
                HandleInput input = playerOne.GetComponent<HandleInput>();
                playerOne.name = "player1";
                input.AssignInput("Vertical_P1", "Horizontal_P1");
                playerManager.addTargets(i, playerOne);

                if(PlayerPrefs.GetString("GameMode") == "race")
                {
                    GameObject enemyAI = Instantiate(playerAI, new Vector3(10, -35, 0), Quaternion.identity, playerParent.transform);
                    enemyAI.name = "playerAI";
                    input.AssignInput("Vertical_P1", "Horizontal_P1");
                }
            }
            else if (i == 1)
            {
                GameObject playerTwo = Instantiate(player2, new Vector3(10, -35, 0), Quaternion.identity, playerParent.transform);
                HandleInput input = playerTwo.GetComponent<HandleInput>();
                playerTwo.name = "player2";
                input.AssignInput("Vertical", "Horizontal");
                playerManager.addTargets(i, playerTwo);
            }
            
        }
    }
    IEnumerator GetRaceType()
    {
        isRaceStart = false;
        yield return new WaitForSeconds(3);
        isRaceStart = true;
        if (playerCount == 2)
        {  
            raceType = VERSUS;
        }
        else
        {
            raceType = TIME_ATTACK;
        }   
    }

    public void GetRaceResults()
    {

    }

    public void TimeAttack()
    {
        timeToBeat -= Time.deltaTime;
        timer.text = timeToBeat.ToString("F2");

        if(timeToBeat <= 0)
        {
            Debug.Log("YOU LOSE!");
            isRaceComplete = true;
            
        }
    }

}
