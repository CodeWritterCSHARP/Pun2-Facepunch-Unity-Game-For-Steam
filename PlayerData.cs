using UnityEngine;
using Steamworks;
using System;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public string[] star = new string[12];
    public float[] times = new float[12];
    public int allStars;
    public GetAchivement achivement;

    private void Awake()
    {
        star = new string[12];
        times = new float[12];
        for (int i = 0; i < 12; i++) { star[i] = "000"; times[i] = 0; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) Recount();
    }

    public void Recount()
    {
        SteamUserStats.GetStat("STAR", out allStars);
        allStars = 0;
        for (int i = 0; i < star.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (star[i][j] == '1') allStars+=1;
            }
        }
        for (int i = 0; i < 12; i++)
        {
            print(star[i] + " " + times[i] + Environment.NewLine);
        }
        SteamUserStats.SetStat("STAR", allStars);
        SteamUserStats.StoreStats();
        if (allStars >= 15) achivement.enabled = true;
    }
}
