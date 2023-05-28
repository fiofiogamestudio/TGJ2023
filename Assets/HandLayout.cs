using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLayout : MonoBehaviour
{
    public int HandCount = 0;

    [ReadOnly]
    public List<Vector3> TargetPositions = new List<Vector3>();
    [ReadOnly]
    public List<Quaternion> TargetRotations = new List<Quaternion>();
    [ReadOnly]
    public List<Transform> CardTransforms = new List<Transform>();

    public void AddHand(Transform cardTransform)
    {
        HandCount++;
        CardTransforms.Add(cardTransform);

        refreshAnchoredTransforms();
        refreshCardOrder();
    }

    public void RemoveHand(Transform cardTransform)
    {
        HandCount--;
        CardTransforms.Remove(cardTransform);

        refreshAnchoredTransforms();
        refreshCardOrder();
    }

    void Update()
    {
        checkHandArea();
    }

    void FixedUpdate()
    {
        // check selecting card : leave hand area?
        lerpCardTransforms();
    }

    void initAnchoredPositions()
    {
        // AnchoredPositions.Add(Vector3.zero);
    }

    void addAnchoredPositions()
    {

    }

    public float anchoredCenterY = -40;
    public float anchoredDeltaAngle = 1.5f;
    public float anchoredRadius = 43f;
    void refreshAnchoredTransforms()
    {
        TargetPositions.Clear();
        TargetRotations.Clear();
        Vector3 center = Vector3.up * anchoredCenterY;
        Vector3 worldCenter = transform.TransformPoint(center);
        float startAngle = -(HandCount - 1) * anchoredDeltaAngle / 2;
        for (int i = 0; i < HandCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(startAngle + anchoredDeltaAngle * i, Vector3.forward);
            Vector3 dir = rotation * Vector3.up;
            Vector3 target = center + dir * anchoredRadius;
            TargetPositions.Add(target);
            TargetRotations.Add(rotation);
        }
    }

    void refreshCardOrder()
    {
        for (int i = 0; i < HandCount; i++)
        {
            CardTransforms[i].GetComponent<Canvas>().sortingOrder = i;
        }
    }

    [Header("Hand Area")]
    public float HandAreaThresholdY = 0.2f;

    void checkHandArea()
    {

        // Debug.Log(InputManager.GetMousePosNormalized());
    }

    void lerpCardTransforms()
    {
        for (int i = 0; i < HandCount; i++)
        {
            Vector3 targetPosition = TargetPositions[i];
            Quaternion targetRotation = TargetRotations[i];
            float targetScale = 1.0f;
            // handle hovering case
            if (hovering && !selecting)
            {
                if (i < hoveredCardIndex)
                {
                    targetPosition += Vector3.right * hoveringOffsetX;
                }
                if (i > hoveredCardIndex)
                {
                    targetPosition += Vector3.left * hoveringOffsetX;
                }
                if (i == hoveredCardIndex)
                {
                    targetPosition += Vector3.up * hoveringOffsetY;
                    targetScale = 1.0f + hoveringEnlarge;
                }
            }
            // handle selecting case
            if (selecting)
            {
                if (i < selectingCardIndex)
                {
                    targetPosition += Vector3.left * selectingOffsetX;
                }
                if (i > selectingCardIndex)
                {
                    targetPosition += Vector3.right * selectingOffsetX;
                }
                if (i == selectingCardIndex)
                {
                    Vector2 mousePos = InputManager.GetMousePosNormalized();
                    // 手牌在水平面的位置和旋转
                    Vector3 planedTargetPosition = InputManager.GetMousePosPlaned();
                    Vector3 handPlanedTargetPosition = transform.InverseTransformPoint(planedTargetPosition);
                    Quaternion planedTargetRotation = Quaternion.Inverse(transform.rotation) * Quaternion.Euler(90, 0, 0);
                    // 手牌在Layout平面的位置
                    Vector3 layoutTargetPosition = InputManager.GetMousePosPlaned(transform);
                    Vector3 handLayoutTargetPosition = transform.InverseTransformPoint(layoutTargetPosition);
                    // Vector3 handLayoutTargetPosition = handPlanedTargetPosition;
                    Debug.DrawLine(layoutTargetPosition, Vector3.zero, Color.red);
                    handLayoutTargetPosition.z = 0;

                    // 手牌在屏幕空间的位置
                    Vector2 cardScreenPos = InputManager.GetScreenPosNormalized(CardTransforms[i].position);
                    // Debug.Log(cardScreenPos);

                    float enlargeFactor = 1.0f;
                    // float enlargeFactor = Mathf.Lerp(hoveringEnlarge, 0, clampedMouseY / HandAreaThresholdY);
                    // 判断是否离开手牌区域（屏幕空间）
                    if (cardScreenPos.y > HandAreaThresholdY)
                    {
                        targetPosition = handPlanedTargetPosition;
                        targetRotation = planedTargetRotation;
                    }
                    else
                    {
                        targetPosition = handLayoutTargetPosition;
                        targetRotation = Quaternion.identity;
                        enlargeFactor = 0.0f;
                    }

                    // targetPosition += Vector3.right * mousePos.x * 100 + Vector3.up * mousePos.y * 100;
                    // targetPosition += Vector3.up * hoveringOffsetY;
                    float clampedMouseY = Mathf.Clamp(0, HandAreaThresholdY, cardScreenPos.y);
                    // enlargeFactor = 1.0f;
                    targetScale = 1.0f + hoveringEnlarge * enlargeFactor;

                }
            }


            Vector3 currentPos = CardTransforms[i].localPosition;
            Quaternion currentRot = CardTransforms[i].localRotation;
            float currentScale = CardTransforms[i].localScale.x;
            CardTransforms[i].localPosition = Vector3.Lerp(currentPos, targetPosition, LerpTransformValue);
            CardTransforms[i].localRotation = Quaternion.Lerp(currentRot, targetRotation, LerpTransformValue);
            CardTransforms[i].localScale = Mathf.Lerp(currentScale, targetScale * 0.01f, LerpScaleValue) * Vector3.one;

        }
    }



    public float LerpTransformValue = 0.15f;
    public float LerpScaleValue = 0.2f;
    [ReadOnly]
    [Header("Hovering Effects")]
    public bool hovering = false;
    [ReadOnly]
    public int hoveredCardIndex = -1;

    public float hoveringOffsetX = 0.5f;
    public float hoveringOffsetY = 0.5f;
    public float hoveringEnlarge = 0.2f;

    public void HoverCard(Transform cardTransform)
    {
        hovering = true;
        hoveredCardIndex = queryCardIndex(cardTransform);
    }

    public void UnhoverCard(Transform cardTransform)
    {
        hovering = false;
    }

    [Header("Selecting Effects")]
    [ReadOnly]
    public bool selecting;
    [ReadOnly]
    public int selectingCardIndex;
    [ReadOnly]
    public Transform selectingCardTransform;

    public float selectingOffsetX = 0;

    public void SelectCard(Transform cardTransform)
    {
        selecting = true;
        selectingCardIndex = queryCardIndex(cardTransform);
        selectingCardTransform = cardTransform;
        // 放在首层
        // setAsFirstOrder(cardTransform);
    }

    public void ReleaseCard(Transform cardTransform)
    {
        selecting = false;
        selectingCardTransform = null;
        // 不立即刷新Order，而是当牌飞回时刷新
        refreshCardOrder();
    }


    private int queryCardIndex(Transform cardTransform)
    {
        return CardTransforms.IndexOf(cardTransform);
    }

    private void setAsFirstOrder(Transform cardTransform)
    {
        int index = queryCardIndex(cardTransform);
        CardTransforms[index].GetComponent<Canvas>().sortingOrder = 99;
    }
}
