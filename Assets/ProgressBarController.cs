using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    public GameObject ProgressObject;
    public float CurrentValue = 0;

    // Update is called once per frame
    void Update()
    {
        RectTransform rt = gameObject.transform.GetComponent<RectTransform>();
        float width = rt.sizeDelta.x * rt.localScale.x;
        float height = rt.sizeDelta.y * rt.localScale.y;


        if (CurrentValue < 0)
            CurrentValue = 0;

        if (CurrentValue > 100)
            CurrentValue = 100;

        if (ProgressObject != null)
        {
            ProgressObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, width - ((CurrentValue/100) * width), (CurrentValue / 100) * width);
        }
    }
}
