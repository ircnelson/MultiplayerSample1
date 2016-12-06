using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    private Player _player;
    private Vector3 _velocity;
    private Rigidbody _rigidbody;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
        }

        _player = GetComponent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    private void Update ()
    {
        // Movement
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _velocity = moveInput.normalized * _player.moveSpeed;

        // Look at
        LookAt();
    }

    private void LookAt()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.up * _player.Holder.position.y);

        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            Debug.DrawLine(ray.origin, point, Color.yellow);

            Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);

            transform.LookAt(heightCorrectedPoint);
        }
    }
}
