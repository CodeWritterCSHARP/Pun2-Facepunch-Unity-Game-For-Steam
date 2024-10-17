using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionWindow : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] private AudioClip sound;

    private Resolution[] resolutions;
    private List<Resolution> filteredRes = new List<Resolution>();

    private float currentRate;

    private void Start()
    {
        int currentIndex = 0;
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        currentRate = Screen.currentResolution.refreshRate;

        // Application.targetFrameRate = Screen.currentResolution.refreshRate; //lock fps

        for (int i = 0; i < resolutions.Length; i++)
            if (resolutions[i].refreshRate == currentRate) filteredRes.Add(resolutions[i]);

        List<string> options = new List<string>();
        for (int i = 0; i < filteredRes.Count; i++)
        {
            string resOption = filteredRes[i].width + "x" + filteredRes[i].height + " " + filteredRes[i].refreshRate + " Hz";
            options.Add(resOption);
            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height) currentIndex = i;
        }

        dropdown.AddOptions(options);
        if (currentIndex != 0) { dropdown.value = currentIndex; dropdown.RefreshShownValue(); }
        else dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Screen.width + "x" + Screen.height + " " + currentRate + " Hz";
    }

    private void FixedUpdate() => dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Screen.width + "x" + Screen.height + " " + currentRate + " Hz";

    public void ChangeResolution()
    {
        Resolution resolution = filteredRes[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
