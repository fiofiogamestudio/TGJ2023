using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Vector2 GetMousePosNormalized()
    {
        Vector3 mousePosition = Input.mousePosition;
        return normalizeScreenPos(mousePosition);
    }

    private static Vector2 normalizeScreenPos(Vector2 pos)
    {
        Resolution resolution = Screen.currentResolution;
        return new Vector2(pos.x / resolution.width, pos.y / resolution.height);
    }

    public static Vector3 GetMousePosPlaned()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        Vector3 hitPoint = Vector3.zero;
        if (plane.Raycast(ray, out enter))
        {
            hitPoint = ray.GetPoint(enter);
        }
        return hitPoint;
    }

    public static Vector3 GetMousePosPlaned(Transform targetTransform)
    {
        Plane plane = new Plane(-targetTransform.forward, targetTransform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        Vector3 hitPoint = Vector3.zero;
        if (plane.Raycast(ray, out enter))
        {
            hitPoint = ray.GetPoint(enter);
        }
        return hitPoint;
    }

    public static Vector2 GetScreenPos(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }

    public static Vector2 GetScreenPosNormalized(Vector3 position)
    {
        return normalizeScreenPos(GetScreenPos(position));
    }

    public static bool GetMouseHolding()
    {
        return Input.GetMouseButton(0);
    }

    public static bool GetMouseRelease()
    {
        return Input.GetMouseButtonUp(0);
    }

    public static bool GetMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public static bool GetMouseRightDown()
    {
        return Input.GetMouseButtonDown(1);
    }
}
