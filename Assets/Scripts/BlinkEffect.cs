using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BlinkEffect : MonoBehaviour
{
    private CanvasGroup group;

    void Awake()
    {
        group = this.GetComponent<CanvasGroup>();
        group.alpha = 0.0f;
    }

    private float blinkTimer = 1.0f;
    private float blinkTime = 1.0f;

    [ReadOnly]
    public bool show = true;

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer > blinkTime) blinkTimer = 0.0f;
        }
        else
        {
            if (blinkTimer < blinkTime) blinkTimer += Time.deltaTime;
        }


        float normalizedX = (blinkTimer / blinkTime) * Mathf.PI;
        float normalizedY = Mathf.Sin(normalizedX);
        group.alpha = normalizedY;
    }

    public void EnableShow()
    {
        show = true;
    }

    public void DisableShow()
    {
        show = false;
    }




}
