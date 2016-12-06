using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Vector3 _offset;

    public Transform followTarget;

    private void Start()
    {
        _offset = transform.position;
    }

    private void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position + _offset;
        }
    }
}
