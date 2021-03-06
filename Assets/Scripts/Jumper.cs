using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private bool _isGrounded;
    private Rigidbody _rigidboby;


    private void Start()
    {
        _rigidboby = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&_isGrounded==true)
        {
            _isGrounded = false;
            _rigidboby.AddForce(Vector3.up * _jumpForce);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Road road))
        {
            _isGrounded = true;
        }
    }
}
