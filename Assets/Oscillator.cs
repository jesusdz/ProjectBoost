using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f; // 2 seconds

    [Range(0f, 1f)]
    [SerializeField]
    float movementFactor = 0f;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        movementFactor = Mathf.Sin(cycles * Mathf.PI * 2f);
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
