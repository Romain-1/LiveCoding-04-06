using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 5f;

    [SerializeField] private float JumpForce = 800f;
    [SerializeField] private uint MaxNbJumps = 2;
    [SerializeField] private Trigger feet;
    [SerializeField] public bool isFrozen = false;
    
    private uint _nbJumpLeft;
    private Vector3 _baseScale;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody;

    private int IsJumpHash;
    private int IsRunHash;

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        IsJumpHash = Animator.StringToHash("isJumping");
        IsRunHash = Animator.StringToHash("isRunning");
    }

    // Start is called before the first frame update
    void Start()
    {
        _nbJumpLeft = MaxNbJumps;
        _baseScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public int GetFittedAxisRaw(string name)
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
        feet.isTriggered = false;
        _nbJumpLeft -= 1;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = 0f; 
        _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    }

   
    // Update is called once per frame
    void Update()
    {
        var directionX = GetFittedAxisRaw("Horizontal");
        var directionY = GetFittedAxisRaw("Jump");

        if (feet.isTriggered)
        {
            _playerAnimator.SetBool(IsJumpHash, false);
            _nbJumpLeft = MaxNbJumps;
        }
        else
        {
            _playerAnimator.SetBool(IsJumpHash, true);
        }

        if (isFrozen)
        {
            return;
        }

        if (directionY == 1 && Input.anyKeyDown)
        {
            Jump();
        }

        if (directionX != 0)
        {
            transform.Translate(new Vector3(directionX * Time.deltaTime * Speed, 0, 0));
            _playerAnimator.SetBool(IsRunHash, true);
            transform.localScale = new Vector3(directionX * _baseScale.x, _baseScale.y, _baseScale.z);
        }
        else
        {
            _playerAnimator.SetBool(IsRunHash, false);
        }
        
    }
}