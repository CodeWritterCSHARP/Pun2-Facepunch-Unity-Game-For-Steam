using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectLanguage : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] private List<string> languages = new List<string>();
    [SerializeField] private GetSelectedLanguage[] gets;

    private void Start()
    {
        int currentIndex = 0;
        dropdown.ClearOptions();
        dropdown.AddOptions(languages);

        if (currentIndex != 0) { dropdown.value = currentIndex; dropdown.RefreshShownValue(); }
        else dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "En";
    }

    public void OnChange()
    {
        Languages.instance.type = dropdown.value;
        foreach (var item in gets) item.ChangeText(Languages.instance.type);
    }
}
