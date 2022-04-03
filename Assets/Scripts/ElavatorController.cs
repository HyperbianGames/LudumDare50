using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElavatorController : MonoBehaviour
{
    public GameObject Elavator;
    public GameObject PlayerCenterLoc;
    public Collider CSollider;
    public GameObject ElevatorTarget;

    bool reset = false;
    bool raise = false;
    float raiseStartTime = 0;
    float raiseEndTime = 0;
    Vector3 initialPos;
    bool hasBeenRaised = false;

    PlayerController cont2 = null;

    public float ElevatorRiseLength = 10;

    public static ElavatorController Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (raise && Time.time < raiseStartTime + ElevatorRiseLength)
        {
            float durationTime = Time.time - raiseStartTime;
            Elavator.transform.position = Vector3.Lerp(initialPos, ElevatorTarget.transform.position, durationTime / ElevatorRiseLength);
            PlayerController.Instance.gameObject.transform.position = PlayerCenterLoc.transform.position;
        }
        else
        {
            if (raise)
            {
                raise = false;
                PlayerController.Instance.ReleasePlayer();
                //PlayerController.Instance.gameObject.transform.SetParent(null);
            }
        }            
    }

    public void StartRaise()
    {
        hasBeenRaised = true;
        raiseStartTime = Time.time;
        raiseEndTime = Time.time + ElevatorRiseLength;
        initialPos = Elavator.transform.position;
        //PlayerController.Instance.gameObject.transform.SetParent(Elavator.transform);
        raise = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenRaised && !reset)
        {
            PlayerController cont = other.GetComponent<PlayerController>();
            if (cont != null && cont2 == null)
            {
                //reset = true;
                cont2 = cont;
                cont.ReadyPlayerForElavator();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!reset)
        {
            PlayerController cont = other.GetComponent<PlayerController>();
            if (cont == cont2)
            {
                cont2 = null;
            }
        }
    }
}
