using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CustomCursorHandler : MonoBehaviour
{
    [SerializeField] Canvas cursorCanvas;
    [SerializeField] GameObject cursorRoot;
    [SerializeField] GameObject cursorImage;

    [SerializeField] RectTransform cursorMiddle;
    [SerializeField] RectTransform cursorTop;
    [SerializeField] RectTransform cursorBottom;
    [SerializeField] RectTransform cursorLeft;
    [SerializeField] RectTransform cursorRight;

    Vector2 mousePos;

    [SerializeField] GameInfo gameInfo;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        CalculateCursorSize();
        ShowCursor();
    }

    // Update is called once per frame
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorCanvas.transform as RectTransform, Input.mousePosition, cursorCanvas.worldCamera, out mousePos);

        cursorRoot.transform.localPosition = mousePos;
    }

    public void ShowCursor()
    {
        cursorImage.SetActive(true);
        Cursor.visible = false;
    }

    public void HideCursor()
    {
        cursorImage.SetActive(false);
        Cursor.visible = true;
    }

    public void CalculateCursorSize()
    {
        float radius = gameInfo.forgivingRepelRadius;

        Vector3 worldPoint = cam.transform.position + cam.transform.forward * 10f;

        Vector3 centerScreen = cam.WorldToScreenPoint(worldPoint);

        // IMPORTANT: measure full box extents in camera space
        Vector3 rightOffset = cam.transform.right * radius;
        Vector3 upOffset = cam.transform.up * radius;

        Vector3 rightScreen = cam.WorldToScreenPoint(worldPoint + rightOffset);
        Vector3 upScreen = cam.WorldToScreenPoint(worldPoint + upOffset);

        float screenHalfWidth = Vector2.Distance(centerScreen, rightScreen);
        float screenHalfHeight = Vector2.Distance(centerScreen, upScreen);

        float baseSize = cursorMiddle.rect.width;

        float scaleX = (screenHalfWidth * 2f) / baseSize;
        float scaleY = (screenHalfHeight * 2f) / baseSize;

        cursorMiddle.localScale = new Vector3(scaleX, scaleY, 1f);

        PlaceEdgeVisualizers();
    }

    void PlaceEdgeVisualizers()
    {
        float height = cursorMiddle.rect.height * cursorMiddle.localScale.y; 
        float halfHeight = height * 0.5f;

        Vector3 topEdge = cursorMiddle.up * halfHeight;
        Vector3 bottomEdge = (-cursorMiddle.up) * halfHeight;
        Vector3 leftEdge = (-cursorMiddle.right) * halfHeight;
        Vector3 rightEdge = cursorMiddle.right * halfHeight;

        Debug.Log("Height: " + halfHeight);

        cursorTop.transform.localPosition = topEdge;
        cursorBottom.transform.localPosition = bottomEdge;
        cursorLeft.transform.localPosition = leftEdge;
        cursorRight.transform.localPosition = rightEdge;
    }
}
