using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public double Money { get; set; } = 0;
    public int ClickPower { get; set; } = 1;
    public int Clients { get; set; } = 0;
    public long ClicksDone { get; set; } = 0;
    // Level 1
    public int Level2G { get; set; } = 0;
    public int Cost2G { get; set; } = 10;
    // Level 2
    public int LevelWifi { get; set; } = 0;
    public int CostWifi { get; set; } = 3000;
    // Level 3
    public int Level3G { get; set;} = 0;
    public int Cost3G { get; set;} = 100000;
    // Level 4
    public int Level4G { get; set; } = 0;
    public int Cost4G { get; set; } = 3000000;
    // Level 5
    public int Level5G { get; set;} = 0;
    public int Cost5G { get; set;} = 10000000;
    // Level 6
    public int Level6G { get; set;} = 0;
    public int Cost6G { get; set;} = 300000000;

    public void ResetData()
    {
        Money = 0;
        ClickPower = 1;
        Clients = 0;
        ClicksDone = 0;
        Level2G = 0;
        Cost2G = 10;
        LevelWifi = 0;
        CostWifi = 3000;
        Level3G = 0;
        Cost3G = 100000;
        Level4G = 0;
        Cost4G = 3000000;
        Level5G = 0;
        Cost5G = 10000000;
        Level6G = 0;
        Cost6G = 300000000;
    }
}
