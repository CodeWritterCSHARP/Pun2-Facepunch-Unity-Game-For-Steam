using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloBtn : MonoBehaviour
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
            LVLLoader button = cur.GetComponent<LVLLoader>();
            button.curLocation = LocationsArray[i];
            switch (i)
            {
                case 2: button.GetComponent<Button>().image.sprite = sprites[1]; /*button.gameObject.SetActive(false);*/ break;
                case 3: button.GetComponent<Button>().image.sprite = sprites[1]; break;
                case 4: button.GetComponent<Button>().image.sprite = sprites[1]; /*button.gameObject.SetActive(false);*/ break;
                case 5: button.GetComponent<Button>().image.sprite = sprites[2]; /*button.gameObject.SetActive(false);*/ break;
                case 6: button.GetComponent<Button>().image.sprite = sprites[2]; break;
                case 7: button.GetComponent<Button>().image.sprite = sprites[3]; /*button.gameObject.SetActive(false);*/ break;
                case 8: button.GetComponent<Button>().image.sprite = sprites[3]; /*button.gameObject.SetActive(false);*/ break;
                case 9: button.GetComponent<Button>().image.sprite = sprites[3]; /*button.gameObject.SetActive(false);*/ break;
                case 10: button.GetComponent<Button>().image.sprite = sprites[4]; /*button.gameObject.SetActive(false);*/ break;
                case 11: button.GetComponent<Button>().image.sprite = sprites[4]; /*button.gameObject.SetActive(false);*/ break;
                case 12: button.GetComponent<Button>().image.sprite = sprites[4]; button.gameObject.SetActive(false); break;
                default: button.GetComponent<Button>().image.sprite = sprites[0]; break;
            }
            counter++;
            buttonsAll[i] = cur.GetComponent<Button>();
        }
        Camera.main.GetComponent<ButtonsDelay>().buttons = buttonsAll;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerSoloController>().Invoke("InvokeRestart", .5f);
    }
}
