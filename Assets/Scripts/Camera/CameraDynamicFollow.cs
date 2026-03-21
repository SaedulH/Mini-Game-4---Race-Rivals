using Unity.Cinemachine;
using UnityEngine;
using Utilities;

public class CameraDynamicFollow : NonPersistentSingleton<CameraDynamicFollow>
{
    private CinemachineCamera cinemachineCamera;

    [field: SerializeField] public GameObject PlayerOne {  get; private set; }
    private Rigidbody2D _rb1;
    [field: SerializeField] public GameObject PlayerTwo {  get; private set; }
    private Rigidbody2D _rb2;

    private bool _enabled;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialisePlayers(bool enabled, GameObject playerOne, GameObject playerTwo)
    {

    }
}
