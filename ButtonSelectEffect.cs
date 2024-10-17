using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool play = true;
    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("play", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("play", false);
    }
}
