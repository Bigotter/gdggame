using System.Collections;
using System.Collections.Generic;
using System.Net;
using core;
using UnityEngine;

public class ApplyHappiness : MonoBehaviour
{
    ApplyIconValue _applyIconValue = new ApplyIconValue();

    void Start()
    {
        _applyIconValue.Init(ProcessCard.InitHappiness);
    }

    void Update()
    {
        var processCard = ProcessCard.Instance();
        var happiness = processCard.Happiness;

        _applyIconValue.Apply(gameObject, processCard.Happiness);
    }
}