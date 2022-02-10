using System;
using UnityEngine;

public class CircularMover : MonoBehaviour
{
    [SerializeField]
    private float frequency = 5;
    [SerializeField, Range(2f, 10)]
    private float amplitude = 5;

    private float counter = 0;
    private int side = 0;
    private Vector3 newPosition = new Vector3();

    private void Start()
    {
        side = Math.Sign(transform.position.x);
        if (side == 0)
            side = 1;
    }

    private void Update()
    {
        counter += Time.deltaTime * frequency;

        newPosition = new Vector3
        {
            x = Mathf.Cos(counter) * amplitude * side,
            y = Mathf.Sin(counter) * amplitude * side,
            z = transform.position.z
        };

        transform.position = newPosition;
    }
}