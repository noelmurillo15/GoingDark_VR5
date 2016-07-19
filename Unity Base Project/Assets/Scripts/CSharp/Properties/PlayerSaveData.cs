using System;

[Serializable]
public class PlayerSaveData
{
    public int Credits;
    public string SectorName;

    public PlayerSaveData()
    {
        Credits = 0;
        SectorName = "Unknown";
    }
}