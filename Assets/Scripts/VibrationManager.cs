using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Vibration(HapticTypes type)
    {
        //if (PlayerPrefs.GetInt(Settings.Vibration.ToString()) != 1) return;
        if (type == HapticTypes.Warning)
        {
            Taptic.Warning();
        }
        else if (type == HapticTypes.Failure)
        {
            Taptic.Failure();
        }
        else if (type == HapticTypes.Success)
        {
            Taptic.Success();
        }
        else if (type == HapticTypes.LightImpact)
        {
            Taptic.Light();
        }
        else if (type == HapticTypes.MediumImpact)
        {
            Taptic.Medium();
        }
        else if (type == HapticTypes.HeavyImpact)
        {
            Taptic.Heavy();
        }
        else if (type == HapticTypes.Default)
        {
            Taptic.Default();
        }
        else if (type == HapticTypes.VibrationImpact)
        {
            Taptic.Vibrate();
        }
        else if (type == HapticTypes.Selection)
        {
            Taptic.Selection();
        }
    }
}