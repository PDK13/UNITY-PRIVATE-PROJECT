using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHold : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image Image;
    public CanvasGroup CanvasGroup;

    public bool Lock = false;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    [Serializable]
    public class ColorEventSingle
    {
        public Color Normal = Color.white;
        public Color Ready = Color.gray;
        public Color Hold = Color.yellow;
    }

    [Serializable]
    public class ScaleEventSingle
    {
        public Vector2 Normal = Vector2.one * 1.00f;
        public Vector2 Ready = Vector2.one * 1.25f;
        public Vector2 Hold = Vector2.one * 1.50f;
    }

    [Serializable]
    public class AlphaEventSingle
    {
        [Range(0f, 1f)] public float Normal = 1.00f;
        [Range(0f, 1f)] public float Ready = 0.50f;
        [Range(0f, 1f)] public float Hold = 0.25f;
    }

    public ColorEventSingle ColorEvent;
    public ScaleEventSingle ScaleEvent;
    public AlphaEventSingle AlphaEvent;

    [Serializable]
    private class PointerEventSingle
    {
        [Space]
        public UnityEvent PointerHold;
        public UnityEvent PointerDown;
        public UnityEvent PointerUp;
        public UnityEvent PointerEnter;
        public UnityEvent PointerExit;
    }

    [SerializeField] private PointerEventSingle PointerEvent;

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

    private void Start()
    {
        if (Image == null)
            Image = QComponent.GetComponent<Image>(gameObject);
        //
        if (CanvasGroup == null)
            CanvasGroup = QComponent.GetComponent<CanvasGroup>(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        SetEventPointerEnter();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        SetEventPointerExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetEventPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetEventPointerUp();
    }

    //Press

    public void SetButtonPress()
    {
        Image.color = ColorEvent.Ready;
        transform.localScale = ScaleEvent.Ready;
        CanvasGroup.alpha = AlphaEvent.Ready;
        //
        SetEventPointerDown();
    }

    public void SetButtonRelease()
    {
        SetEventPointerUp();
    }

    //Event

    private void SetEventPointerEnter()
    {
        if (Lock)
            return;
        //
        if (Application.platform == RuntimePlatform.Android)
            SetEventPointerDown();
        else
        {
            Ready = true;
            //
            if (Hold || HoldActive)
            {
                Image.color = ColorEvent.Hold;
                transform.localScale = ScaleEvent.Hold;
                CanvasGroup.alpha = AlphaEvent.Hold;
            }
            else
            {
                Image.color = ColorEvent.Ready;
                transform.localScale = ScaleEvent.Ready;
                CanvasGroup.alpha = AlphaEvent.Ready;
            }
            //
            PointerEvent.PointerEnter?.Invoke();
        }
    }

    private void SetEventPointerExit()
    {
        if (Lock)
            return;
        //
        if (Application.platform == RuntimePlatform.Android)
            SetEventPointerUp();
        else
        {
            Ready = false;
            //
            if (Hold)
            {
                Image.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
                transform.localScale = HoldActive ? ScaleEvent.Hold : ScaleEvent.Ready;
                CanvasGroup.alpha = HoldActive ? AlphaEvent.Hold : AlphaEvent.Ready;
            }
            else
            {
                Image.color = ColorEvent.Normal;
                transform.localScale = ScaleEvent.Normal;
                CanvasGroup.alpha = AlphaEvent.Normal;
            }
            //
            PointerEvent.PointerExit?.Invoke();
        }
    }

    private void SetEventPointerDown()
    {
        if (Lock)
            return;
        //
        Hold = true;
        PointerEvent.PointerDown?.Invoke();
        //
        StartCoroutine(ISetButtonHold());
    }

    private void SetEventPointerUp()
    {
        if (Lock)
            return;
        //
        Hold = false;
        HoldActive = false;
        //
        if (Hold)
        {
            Image.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
            transform.localScale = HoldActive ? ScaleEvent.Hold : ScaleEvent.Ready;
            CanvasGroup.alpha = HoldActive ? AlphaEvent.Hold : AlphaEvent.Ready;
        }
        else
        if (Ready)
        {
            Image.color = ColorEvent.Ready;
            transform.localScale = ScaleEvent.Ready;
            CanvasGroup.alpha = AlphaEvent.Ready;
        }
        else
        {
            Image.color = ColorEvent.Normal;
            transform.localScale = ScaleEvent.Normal;
            CanvasGroup.alpha = AlphaEvent.Ready;
        }
        //
        PointerEvent.PointerUp?.Invoke();
        //
        StopAllCoroutines();
    }

    private IEnumerator ISetButtonHold()
    {
        if (DelayHold > 0)
            yield return new WaitForSeconds(DelayHold);
        //
        HoldActive = true;
        Image.color = ColorEvent.Hold;
        transform.localScale = ScaleEvent.Hold;
        CanvasGroup.alpha = AlphaEvent.Hold;
        //
        while (Hold)
        {
            PointerEvent.PointerHold?.Invoke();
            yield return null;
        }
        //
        HoldActive = false;
        //
    }
}