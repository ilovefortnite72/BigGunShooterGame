using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
[CreateAssetMenu(fileName = "BlinkAbility", menuName = "Abilities/BlinkAbility")]
public class CyberTeleport : SOAbilities
{
    public float blinkDistance;
    private bool hasBlinked;
    public GameObject blinkEffect;
    public LayerMask whatIsWall;

    protected override void UseAbility(Transform player)
    {
    
        Blink(player);
        CoroutineHelper.Instance.StartCoroutine(BlinkEffect());

    }

    private void Blink(Transform player)
    {
        
        Vector2 currentPos = player.position;
        ObjectPoolManager.SpawnObject(blinkEffect, currentPos, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        Vector2 direction = player.up;


        Vector2 newPos = currentPos + direction * blinkDistance;
        if(Physics2D.OverlapCircle(newPos, 0.5f, whatIsWall) == null)
        {
            player.position = newPos;
        }
        else
        {
            player.position = currentPos;
        }


        ObjectPoolManager.SpawnObject(blinkEffect, newPos, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        hasBlinked = true;
        
    }

    private IEnumerator BlinkEffect()
    {
        if (hasBlinked)
        {
            yield return new WaitForSeconds(0.1f);
            hasBlinked = false;
            ObjectPoolManager.ReturnObjectToPool(blinkEffect);
        }
    }
}
