using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnirtAnimController : MonoBehaviour
{
    private static class SnirtAnimParams
    {
        // Store string names for animator params here.

        public const string FidgetPrefix = "HeadFidget";
        public const int fidgetAnimAmount = 2;

    }

    public float timeToFidgetAnimBase = 45.0f;
    public float timeToFidgetAnimRandom = 15.0f;

    [Header("DEBUG ONLY. DO NOT EDIT.")]
    public float countdownToFidgetAnim;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Setup timer
        countdownToFidgetAnim = timeToFidgetAnimBase;
    }

    void Update()
    {
        // Timer countdown...
        countdownToFidgetAnim -= Time.deltaTime;

        // If timer is 0 or less...
        if (countdownToFidgetAnim <= 0)
        {
            // Trigger Anim...
            anim.SetTrigger(SnirtAnimParams.FidgetPrefix + Random.Range(0, SnirtAnimParams.fidgetAnimAmount).ToString());

            // Reset timer.
            countdownToFidgetAnim = timeToFidgetAnimBase + Random.Range(0, timeToFidgetAnimRandom);
        }
    }
}
