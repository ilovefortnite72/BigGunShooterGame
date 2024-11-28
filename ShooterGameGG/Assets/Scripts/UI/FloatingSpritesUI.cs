using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSpritesUI : MonoBehaviour
{
    public Vector2 minVelocity = new Vector2(20f, 20f);
    public Vector2 maxVelocity = new Vector2(60f, 60f);
    private Vector2 velocity;
    private RectTransform rectTransform;
    private RectTransform canvasRect;

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasRect = transform.root.GetComponent<Canvas>().GetComponent<RectTransform>();


        velocity = new Vector2(Random.Range(minVelocity.x, maxVelocity.x) * (Random.value > 0.5f ? 1 : -1), Random.Range(minVelocity.y, maxVelocity.y) * (Random.value > 0.5f ? 1 : -1));
    }

    void Update()
    {

        rectTransform.anchoredPosition += velocity * Time.deltaTime;

        Vector2 position = rectTransform.anchoredPosition;

        Vector2 minBounds = -canvasRect.rect.size / 2;
        Vector2 maxBounds = canvasRect.rect.size / 2;

        if (position.x <= minBounds.x || position.x >= maxBounds.x)
        {
            velocity.x = -velocity.x; 
        }

        if (position.y <= minBounds.y || position.y >= maxBounds.y)
        {
            velocity.y = -velocity.y; 
        }
    }
}
