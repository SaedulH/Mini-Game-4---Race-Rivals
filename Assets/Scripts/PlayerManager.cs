using Unity.Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private CinemachineTargetGroup targetGroup;


    void Start()
    {
        Debug.Log("working");
        Instance = this;
        targetGroup = GameObject.FindGameObjectWithTag("Target").GetComponent<CinemachineTargetGroup>();
    }

    public void AddTargets(int index, GameObject player)
    {
        //CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        //target.target = player.transform; // You can attach an existing GameObject's transform here
        //target.radius = 30f;
        //target.weight = 1f;
        //Debug.Log("target found");
        //targetGroup.m_Targets[index] = target;
    }
}
