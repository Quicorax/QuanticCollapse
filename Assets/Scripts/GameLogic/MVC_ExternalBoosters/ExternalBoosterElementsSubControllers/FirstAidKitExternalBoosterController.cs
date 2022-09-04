﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstAidKit", menuName = "ScriptableObjects/ExternalBoosters/FirstAidKit")]
public class FirstAidKitExternalBoosterController : ExternalBoosterSourceController
{
    public int lifeRegenAmount = 5;
    public FirstAidKitExternalBoosterController()
    {
        boosterName = "FistAidKit";
    }

    public override void Execute(GridController Controller, Action<string, bool> ConfirmExecution)
    {
        if (Controller.Model.PlayerLife < Controller.Model.playerMaxHealth)
        {
            Controller.ModifyPlayerLife(lifeRegenAmount);
            ConfirmExecution?.Invoke(boosterName, true);
        }
    }
}