using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSelector : MonoBehaviour
{
    private int trackNum { get; set; }
    [SerializeReference] public List<GameObject> tracks;
    void Start()
    {
        GetTrack();
    }

    void GetTrack()
    {
        foreach(Transform child in transform)
        {
            if (child != null && child.CompareTag("Tracks"))
            {
                tracks.Add(child.gameObject);
            }
            
        }


        trackNum = PlayerPrefs.GetInt("TrackNum");
        if(trackNum == 1)
        {
            tracks[1].SetActive(false);
            tracks[0].SetActive(true);
        }
        else
        {
            tracks[0].SetActive(false);
            tracks[1].SetActive(true);
        }
    }
}
