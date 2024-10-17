using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSelectedLanguage : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string[] texts;
    [SerializeField] private int textType;

    void Start()
    {
        // 0 - en, 1 - italy, 2 - spain, 3 - fr, 4- ger, 5 - arab, 6 - portugal, 7 - rus, 8 - ukr, 9 - fi, 10 - ch(1), 11 - ch(2), 12 - japan
        switch (Languages.instance?.type)
        {
            case 0: ChangeText(0); break;
            case 1: ChangeText(1); break;
            case 2: ChangeText(2); break;
            case 3: ChangeText(3); break;
            case 4: ChangeText(4); break;
            case 5: ChangeText(5); break;
            case 6: ChangeText(6); break;
            case 7: ChangeText(7); break;
            case 8: ChangeText(8); break;
            case 9: ChangeText(9); break;
            case 10: ChangeText(10); break;
            case 11: ChangeText(11); break;
            case 12: ChangeText(12); break;
            default: break;
        }
    }

    public void ChangeText(int index)
    {
        if (textType == 0) GetComponent<Text>().text = texts[index];
        else if (textType == 1) GetComponent<TMP_Text>().text = texts[index];
    }
}
