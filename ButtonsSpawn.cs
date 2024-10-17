using UnityEngine;
using UnityEngine.UI;

public class ButtonsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject item;
    public int levelsNumber;
    private int counter = 1;

    [SerializeField] private GameObject[] LocationsArray;
    [SerializeField] private Sprite[] sprites;

    private Button[] buttonsAll;

    private void Start()
    {
        buttonsAll = new Button[levelsNumber];
        for (int i = 0; i < levelsNumber; i++)
        {
            GameObject cur = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
            cur.transform.SetParent(this.gameObject.transform);
            cur.GetComponentInChildren<Text>().text = counter.ToString();

            ButtonsBehaviour button = cur.GetComponent<ButtonsBehaviour>();
            button.curLocation = LocationsArray[i];
            switch (i)
            {
                case 2: button.startsToLoad = 1; button.GetComponent<Button>().image.sprite = sprites[1]; break;
                case 3: button.startsToLoad = 2; button.GetComponent<Button>().image.sprite = sprites[1]; break;
                case 4: button.startsToLoad = 4; button.GetComponent<Button>().image.sprite = sprites[1];
                    /*button.GetComponent<Image>().color = new Color(55, 255, 0, 255);*/ break;
                case 5: button.startsToLoad = 6/*i+2*/; button.GetComponent<Button>().image.sprite = sprites[2];
                    /*button.GetComponent<Image>().color = new Color(0, 255, 255, 255);*/ break;
                case 6: button.startsToLoad = 7/*i+2*/; button.GetComponent<Button>().image.sprite = sprites[2];
                    /*button.GetComponent<Image>().color = new Color(0, 255, 255, 255);*/ break;
                case 7: button.startsToLoad = 8/*i+2*/;  button.GetComponent<Button>().image.sprite = sprites[3];
                    /*button.GetComponent<Image>().color = new Color(255, 0, 0, 255);*/ break;
                case 8: button.startsToLoad = 10/*i+2*/; button.GetComponent<Button>().image.sprite = sprites[3]; break;
                case 9: button.startsToLoad = 12/*i+2*/; button.GetComponent<Button>().image.sprite = sprites[3];
                    /*button.GetComponent<Image>().color = new Color(255, 0, 0, 255);*/break;
                case 10: button.startsToLoad = 15; button.GetComponent<Button>().image.sprite = sprites[4]; break;
                case 11: button.startsToLoad = 100; button.GetComponent<Button>().image.sprite = sprites[4]; break;
                case 12: button.startsToLoad = 100; button.GetComponent<Button>().image.sprite = sprites[4]; break;
                default: button.startsToLoad = 0/*i*/; button.GetComponent<Button>().image.sprite = sprites[0];
                    /*button.GetComponent<Image>().color = new Color(255, 255, 0, 255);*/ break;
            }
            button.transform.GetChild(3).GetComponentInChildren<Text>().text = button.startsToLoad.ToString();
            counter++;
            buttonsAll[i] = cur.GetComponent<Button>();
        }
        Camera.main.GetComponent<ButtonsDelay>().buttons = buttonsAll;
    }
}
