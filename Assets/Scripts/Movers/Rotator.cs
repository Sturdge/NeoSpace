using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotateValue = new Vector3(0, -10f, -25f);

    void Update()
    {
        transform.Rotate(rotateValue * Time.deltaTime);
    }
}
