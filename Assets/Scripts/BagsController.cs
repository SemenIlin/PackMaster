using System;
using UnityEngine;

public class BagsController : MonoBehaviour
{
    private int counter = 0;
    public GameObject[] bags;
    private GameObject bagContent;
    public IBag ibag { get; private set; }

    public static bool isGo = false;
    public bool isFail = false;

    private void Start()
    {
        ButtonController.ButtonClickOk += SetActiveForBagRoom;
    }
        
    private void FixedUpdate()
    {
        if (isGo)
        {
            if (ibag.RotationY < 180)
            {
                ibag.Closing();
            }
            else
            {
                isGo = false;
                isFail = ibag.IsFail();
            }
        }
    }

    public void InstantiateBag()
    {
        if(counter == bags.Length)
        {
            counter = 0;
        }
        bagContent = Instantiate(bags[counter]);
        ibag = bagContent.GetComponent<IBag>();
        bagContent.SetActive(false);
        ++counter;
    }

    private void SetActiveForBagRoom()
    {
        bagContent.SetActive(true);
    }
}
