using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawner : MonoBehaviour
{
    public GameObject[] spritePrefab;
    public int spriteCount = 30;
    public RectTransform canvasRect;

    private void Start()
    {
        for (int i = 0; i < spriteCount; i++)
        {
            SpawnSprite();
        }
    }

    private void SpawnSprite()
    {
        GameObject sprite = Instantiate(spritePrefab[UnityEngine.Random.Range(0, spritePrefab.Length)], transform);
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(canvasRect.rect.xMin, canvasRect.rect.xMax), UnityEngine.Random.Range(canvasRect.rect.yMin, canvasRect.rect.yMax));
        sprite.transform.localPosition = randomPos;
    }
}
