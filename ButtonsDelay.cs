using UnityEngine;
using UnityEngine.UI;

public class ButtonsDelay : MonoBehaviour
{
    public Button[] buttons;
    public bool waiting = false;
    [SerializeField] private float delay;
    private float startdelay;

    void Start() => startdelay = delay;

    private void FixedUpdate()
    {
        if (waiting == true) delay -= Time.fixedDeltaTime;
        if (delay <= 0)
        {
            ChangeStatus(true);
            waiting = false; delay = startdelay;
        }
    }
    private void ChangeStatus(bool status)
    {
        for (int i = 0; i < buttons.Length; i++) buttons[i].enabled = status;
    }
}
