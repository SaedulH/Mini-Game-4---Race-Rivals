using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance;

    public List<GameObject> players;
    [SerializeReference] private GameObject playersParent;
    [SerializeReference] private List<GameObject> checkpoints;

    [SerializeField] TMP_Text lapNumber;
    public int maxLaps {  get; set; }
    private ArrayList[] raceProgress {  get; set; }
    private int lastCheckpoint;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        AssignCheckpoints();
        StartCoroutine(AssignPlayers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AssignPlayers()
    {

        yield return new WaitForSeconds(3);
        playersParent = GameObject.FindGameObjectWithTag("PlayerParent");
        foreach (Transform playerChild in playersParent.transform)
        {
            players.Add(playerChild.gameObject);
        }
        raceProgress = new ArrayList[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            raceProgress[i] = new ArrayList()
            {
                i + 1, // Player number
                1, // lap number
                0 // checkpointNum
            };
        }
    }

    void AssignCheckpoints()
    {
        int trackno = PlayerPrefs.GetInt("TrackNum");
        if (trackno == 1) 
        {
            GameObject nonActiveCheckpoints = gameObject.transform.GetChild(1).gameObject;
            nonActiveCheckpoints.SetActive(false);
        }
        else
        {
            GameObject nonActiveCheckpoints = gameObject.transform.GetChild(0).gameObject;
            nonActiveCheckpoints.SetActive(false);
        }
        GameObject trackcheckpoints = gameObject.transform.GetChild(trackno-1).gameObject;
        foreach(Transform pointChild in trackcheckpoints.transform)
        {
            checkpoints.Add(pointChild.gameObject);
        }
        lastCheckpoint = checkpoints.Count;
        maxLaps = 3;
    }

    public void checkpointReached(int pointNum, int playerNum)
    {
        ArrayList playerStat = raceProgress[playerNum-1];
        int playerNo = (int)playerStat[0];
        int lapNo = (int)playerStat[1];
        int checkpointNo = (int)playerStat[2];

        if(checkpointNo == (pointNum - 1))
        {
            raceProgress[playerNum-1][2] = pointNum;
        }else if(checkpointNo < (pointNum - 1))
        {
            Debug.Log("Checkpoint missed!");
        }else if(checkpointNo == lastCheckpoint && pointNum == 1)
        {
            if(lapNo != maxLaps)
            {
                raceProgress[playerNum - 1][1] = (lapNo + 1);
                lapNumber.text = (lapNo + 1) + "/" + maxLaps; 
            }
            else
            {
                Debug.Log("Player " + playerNo + "Wins!!");
                GameManager.Instance.isRaceComplete = true;
                //StartCoroutine(RaceOver());
            }
            
        }


    }
}
