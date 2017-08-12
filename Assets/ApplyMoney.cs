using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using core;
using UnityEngine;

public class ApplyMoney : MonoBehaviour
{
    readonly ApplyIconValue _applyIconValue = new ApplyIconValue();

    void Start()
    {
        _applyIconValue.Init(ProcessCard.InitMoney, ProcessCard.MaxMoney, gameObject);
    }

    void Update()
    {
        var processCard = ProcessCard.Instance();

        _applyIconValue.Apply(gameObject, processCard.Money);
    }
}