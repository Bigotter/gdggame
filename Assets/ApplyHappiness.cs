using System.Collections;
using System.Collections.Generic;
using System.Net;
using core;
using UnityEngine;

public class ApplyHappiness : MonoBehaviour
{
    readonly ApplyIconValue _applyIconValue = new ApplyIconValue();

    void Start()
    {
        _applyIconValue.Init(ProcessCard.InitHappiness, ProcessCard.MaxHappiness, gameObject);
    }

    void Update()
    {
        var processCard = ProcessCard.Instance();
        _applyIconValue.Apply(gameObject, processCard.Happiness);
    }
}