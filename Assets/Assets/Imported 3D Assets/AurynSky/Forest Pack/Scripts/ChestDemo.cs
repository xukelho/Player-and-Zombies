﻿using System.Collections;
using UnityEngine;

public class ChestDemo : MonoBehaviour
{
    //This script goes on the ChestComplete prefab;

    public Animator chestAnim; //Animator for the chest;

    // Use this for initialization
    private void Awake()
    {
        //get the Animator component from the chest;
        chestAnim = GetComponent<Animator>();
        //start opening and closing the chest for demo purposes;
        StartCoroutine(OpenCloseChest());
    }

    private IEnumerator OpenCloseChest()
    {
        //play open animation;
        chestAnim.SetTrigger("open");
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //play close animation;
        chestAnim.SetTrigger("close");
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseChest());
    }
}