using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class GameManager : NonPersistentSingleton<GameManager>
{
    [field: SerializeField] public GameObject PlayerOne { get; set; }
    [field: SerializeField] public GameObject PlayerTwo { get; set; }
    [field: SerializeField] public InputActionAsset Input { get; set; }

    [field: Header("Prefabs")]
    [field: SerializeField] public GameObject PlayerOnePrefab { get; set; }
    [field: SerializeField] public GameObject PlayerTwoPrefab { get; set; }
    [field: SerializeField] public GameObject PlayerAIPrefab { get; set; }

    private List<float> _currentTrackMedalTimes = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public async Task ConfigurePlayer(int playerIndex, int vehicleIndex, bool isPlayer)
    {
        if (playerIndex == 1)
        {
            PlayerOne = Instantiate(PlayerOnePrefab, Vector3.zero, Quaternion.identity);
            PlayerOne.name = "PlayerOne";
            InputHandler input = PlayerOne.GetOrAdd<InputHandler>();
            input.AssignInput(Input.FindActionMap("PlayerOne"));
        }
        else
        {
            if (isPlayer)
            {
                PlayerTwo = Instantiate(PlayerTwoPrefab, Vector3.zero, Quaternion.identity);
                PlayerTwo.name = "PlayerTwo";
            }
            else
            {
                PlayerTwo = Instantiate(PlayerAIPrefab, Vector3.zero, Quaternion.identity);
                PlayerTwo.name = "PlayerAI";
            }
            InputHandler input = PlayerTwo.GetOrAdd<InputHandler>();
            input.AssignInput(Input.FindActionMap("PlayerTwo"));
        }
        //playerManager.addTargets(vehicleIndex, playerObject);
        await Task.CompletedTask;
    }

    public async Task InitialiseHUD(TrackContext context, List<float> medalTimes)
    {
        _currentTrackMedalTimes = medalTimes;
        await HUDManager.Instance.SetupHUD(context, medalTimes);
    }

    //IEnumerator GetRaceType()
    //{
    //    //isRaceStart = false;
    //    //yield return new WaitForSeconds(3);
    //    //isRaceStart = true;
    //    //if (playerCount == 2)
    //    //{  
    //    //    raceType = VERSUS;
    //    //}
    //    //else
    //    //{
    //    //    raceType = TIME_ATTACK;
    //    //}   
    //}

    public void GetRaceResults()
    {

    }

    public void TimeAttack()
    {
        //timeToBeat -= Time.deltaTime;
        //timer.text = timeToBeat.ToString("F2");

        //if(timeToBeat <= 0)
        //{
        //    Debug.Log("YOU LOSE!");
        //    isRaceComplete = true;

        //}
    }

}
