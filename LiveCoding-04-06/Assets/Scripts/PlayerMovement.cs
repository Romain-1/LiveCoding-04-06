using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 5f;

    [SerializeField] private float JumpForce = 800f;
    [SerializeField] private uint MaxNbJumps = 2;

    private uint _nbJumpLeft;
    private Vector3 _baseScale;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _nbJumpLeft = MaxNbJumps;
        _playerAnimator = GetComponent<Animator>();
        _baseScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    int GetFittedAxisRaw(string name)
    {
        var x = Input.GetAxisRaw(name);

        if (x < -0.05f)
        {
            x = -1;
        }
        else if (x > 0.05f)
        {
            x = 1;
        }
        else
        {
            x = 0;
        }
        return (int)x;
    }

    void Jump()
    {
        if (_nbJumpLeft == 0)
        {
            return;
        }
        _nbJumpLeft -= 1;
        _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        print("tkt je suis call");
        return Physics.Raycast(transform.position, Vector2.down, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        var directionX = GetFittedAxisRaw("Horizontal");
        var directionY = GetFittedAxisRaw("Jump");

        transform.Translate(
            new Vector3(directionX * Time.deltaTime * Speed, 0, 0)
        );

        if (IsGrounded())
        {
            print("jtouche le sol wesh");
            _nbJumpLeft = MaxNbJumps;
        }
        
        if (directionY == 1 && Input.anyKeyDown)
        {
            Jump();
        }


        if (directionX != 0)
        {
            _playerAnimator.SetBool("isRunning", true);
            transform.localScale = new Vector3(directionX * _baseScale.x, _baseScale.y, _baseScale.z);
        }
        else
        {
            _playerAnimator.SetBool("isRunning", false);
        }
    }
}