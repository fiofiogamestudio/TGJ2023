using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public Color ContinuesMissionColor = Color.green;
    public Color NormalMissionColor = Color.blue;
    public Color TimeMissonColor = Color.yellow;
    public Color EmergencyMissionColor = Color.red;

    public static ColorHolder instance;
    void Awake() { if (instance == null) instance = this; }
}
