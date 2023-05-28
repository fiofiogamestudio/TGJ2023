using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    private float width = 2.1f; // temp 210 * 0.01
    private float height = 2.8f; // temp 280 * 0.01

    public bool checkInGrid(Vector3 worldPos)
    {
        Vector3 dir = worldPos - transform.position;
        return (Mathf.Abs(dir.x) < width * 0.5f && Mathf.Abs(dir.z) < height * 0.5f);
    }

    [ReadOnly]
    public bool hovering = false;
    public void Hover()
    {
        hovering = true;
        this.GetComponent<Image>().color = Color.red;
    }

    public void Leave()
    {
        if (hovering)
        {
            hovering = false;
            this.GetComponent<Image>().color = Color.white;
        }
    }

}
