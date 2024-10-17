using Steamworks;
using UnityEngine;

public class GetAchivement : MonoBehaviour
{
    [SerializeField] private string name;

    [SerializeField] private int type = 0;

    [SerializeField] private string Statname;
    [SerializeField] private int currentValue = 0;
    [SerializeField] private int needValue = 0;
    [SerializeField] private int needValue2 = 0;

    void Start()
    {
        if (type == 0)
        {
            if (!SteamManager.Initialized) { Debug.Log("NO"); return; }
            ChangeState(name);
        }
    }

    public void Count()
    {
        SteamUserStats.GetStat(Statname, out currentValue);
        currentValue++;
        SteamUserStats.SetStat(Statname, currentValue);
        SteamUserStats.StoreStats();

        if (currentValue >= needValue) ChangeState(name);
        if (currentValue >= needValue2 && needValue2 != 0) ChangeState("JUMP_TIMES_1000");
    }

    void ChangeState(string achName)
    {
        SteamUserStats.GetAchievement(achName, out bool state);
        if (state == false)
        {
            SteamUserStats.SetAchievement(achName);
            SteamUserStats.StoreStats();
        }
    }
}
