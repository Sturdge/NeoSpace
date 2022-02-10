using UnityEngine;

public class LateralMover : MonoBehaviour
{
    private enum Direction
    {
        Horizontal,
        Vertical
    }

    private enum StartDirection
    {
        LeftUp = -1,
        RightDown = 1
    }

    [SerializeField]
    private Direction direction = Direction.Horizontal;
    [SerializeField]
    private StartDirection startDirection = StartDirection.LeftUp;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float distance = 5;

    private Vector3 startPosition = new Vector3();
    private Vector3 newPosition = new Vector3();

    private void Start()
    {
        startPosition = transform.position;
        newPosition = transform.position;
    }

    private void Update()
    {
        if (direction == Direction.Horizontal)
        {
            newPosition.x = startPosition.x + (Mathf.Sin(Time.time * speed * (int)startDirection) * distance);
        }
        else
        {
            newPosition.y = startPosition.y + (Mathf.Sin(Time.time * speed * (int)startDirection) * distance);
        }

        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

}