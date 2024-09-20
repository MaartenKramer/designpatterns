using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera = null;
    [SerializeField] private float _mouseSensitivity = 3.5f;
    [SerializeField] private float _walkSpeed = 6.0f;
    [SerializeField] private float _gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] private float _moveSmoothTime = 0.1f;
    [SerializeField][Range(0.0f, 0.5f)] private float _mouseSmoothTime = 0.03f;
    [SerializeField] private bool _lockCursor = true;

    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private CharacterController _controller = null;

    private Vector2 _currentDir = Vector2.zero;
    private Vector2 _currentDirVelocity = Vector2.zero;

    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;


    //Locks mouse and makes it invisible
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    // Mouse Movement
    private void UpdateMouseLook()
    {
        Vector2 _targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, _targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

        cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        _playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * _currentMouseDelta.x * _mouseSensitivity);
    }

    // Character Movement
    private void UpdateMovement()
    {
        Vector2 _targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _targetDir.Normalize();

        _currentDir = Vector2.SmoothDamp(_currentDir, _targetDir, ref _currentDirVelocity, _moveSmoothTime);

        if (_controller.isGrounded)
            velocityY = 0.0f;

        velocityY += _gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * _currentDir.y + transform.right * _currentDir.x) * _walkSpeed + Vector3.up * velocityY;

        _controller.Move(velocity * Time.deltaTime);
    }
}
