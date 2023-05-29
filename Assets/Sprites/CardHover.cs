using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    public enum CardEvent
    {
        None,
        SubPanel
    }

    public bool EnableInteractable = true;


    public CardEvent cardEvent = CardEvent.None;

    public GameObject SubPanel;
    private Vector3 startScale;

    private Vector3 targetScale;

    private Quaternion targetRotation;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    const float lerpFactor = 0.4f;
    const float biggerFactor = 1.05f;
    const float rotationScale = 0.1f;


    private Shadow shadow;

    void Awake()
    {
        shadow = GetComponentInChildren<Shadow>();
    }

    void Start()
    {
        startScale = transform.localScale;
        targetScale = startScale;

        targetRotation = Quaternion.identity;

        startPosition = transform.localPosition;
        targetPosition = startPosition;

    }

    void FixedUpdate()
    {
        if (!EnableInteractable) return;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpFactor);

        // transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, lerpFactor);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpFactor);

        float lerpValue = Mathf.InverseLerp(startPosition.y, startPosition.y + 0.1f, transform.localPosition.y);

        shadow.effectDistance = new Vector2(0, -10.0f) * lerpValue;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EnableInteractable) return;
        // transform.localPosition += new Vector3(0f, 0.5f, 0f);
        targetScale = startScale * biggerFactor;

        targetPosition += new Vector3(0f, 0.1f, 0f);

        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!EnableInteractable) return;
        // transform.localPosition = startPosition;
        targetScale = startScale;
        targetRotation = Quaternion.identity;

        targetPosition = startPosition;

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!EnableInteractable) return;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float screenX = Camera.main.WorldToScreenPoint(transform.position).x;
        float screenY = Camera.main.WorldToScreenPoint(transform.position).y;
        // float angle = Mathf.Atan2(mouseY - screenY, mouseX - screenX) * Mathf.Rad2Deg;
        float dx = (mouseX - screenX) * 2 / GetComponent<RectTransform>().rect.width; ;
        float angle = Mathf.PI / 2 * Mathf.Rad2Deg * dx * rotationScale;
        targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (!EnableInteractable) return;
        switch (cardEvent)
        {
            case CardEvent.SubPanel:
                SubPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    public GameObject ShoujiMask;

    public void Unlock()
    {
        Debug.Log("Unlock");
        EnableInteractable = true;
        ShoujiMask.gameObject.SetActive(false);
    }

    public void Lock()
    {
        EnableInteractable = false;
        ShoujiMask.gameObject.SetActive(true);
    }
}
