using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    public float speed = 5f;
    public float zigzagFrequency = 2f;  // Frequency of zigzag
    public float zigzagAmplitude = 0.5f;  // Size of zigzag
    private Vector3 direction;
    private float time;

    public Transform playerTransform;  // The player or facing direction

    void Start()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        direction = (mousePosition - transform.position).normalized;
    }

    void Update()
    {
        time += Time.deltaTime;
        // Move forward while zigzagging
        float zigzag = Mathf.Sin(time * zigzagFrequency) * zigzagAmplitude;
        transform.position += direction * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + zigzag, transform.position.y, transform.position.z);
    }
}
