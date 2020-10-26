using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUIToChildren : MonoBehaviour
{
    public bool scaleHorizontally;
    public bool scaleVertically;
    public float horizontalPadding;
    public float verticalPadding;

    // Instead of just grabbing all children and their children we just want
    // a list of the children we want actively influencing our dimentions.
    public List<GameObject> children;

    private RectTransform trans;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
    }

    public void Resize()
    {
        if (scaleVertically)
        {
            float totalNewHeight = 0f;

            foreach (GameObject child in children)
            {
                RectTransform rt = child.transform.GetComponent<RectTransform>();
                totalNewHeight += rt.sizeDelta.y * rt.localScale.y;
            }

            totalNewHeight += verticalPadding * 2;

            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalNewHeight);
        }

        if (scaleHorizontally)
        {
            float totalNewWidth = 0f;

            foreach (GameObject child in children)
            {
                RectTransform rt = child.transform.GetComponent<RectTransform>();
                totalNewWidth += rt.sizeDelta.x * rt.localScale.x;
            }

            totalNewWidth += horizontalPadding * 2;

            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalNewWidth);
        }
    }
}
