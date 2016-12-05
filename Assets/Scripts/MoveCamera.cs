using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

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
