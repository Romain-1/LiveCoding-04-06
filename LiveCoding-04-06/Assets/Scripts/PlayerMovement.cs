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

    public enum Type
    {
        Player1 = 1,
        Player2 = 2
    };
    
    [SerializeField] public Type player = Type.Player1;

    private float _percentage = 0.01f;
    private uint _nbJumpLeft;
    private Vector3 _baseScale;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody;

    private int _isJumpHash;
    private int _isRunHash;
    private int _takeDamageHash;

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _isJumpHash = Animator.StringToHash("isJumping");
        _isRunHash = Animator.StringToHash("isRunning");
        _takeDamageHash = Animator.StringToHash("takingDamage");
    }

    // Start is called before the first frame update
    void Start()
    {
        _nbJumpLeft = MaxNbJumps;
        _baseScale = transform.localScale;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float nbDamage, float percentageIncrease, Vector2 direction)
    {
        
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _playerAnimator.SetTrigger(_takeDamageHash);
        _rigidbody.AddForce(direction * nbDamage * _percentage, ForceMode2D.Impulse);
        _percentage += percentageIncrease / 100f;
    }

    public int GetFittedAxisRaw(string eventName)
    {
        var x = Input.GetAxisRaw(eventName + "-" + player);

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
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f; 
        _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    }

    public void Restart()
    {
        transform.position = Vector3.zero;
        _rigidbody.velocity = Vector2.zero;
        _percentage = 0.01f;
    }
    
    // Update is called once per frame
    void Update()
    {
        var directionX = GetFittedAxisRaw("Horizontal");
        var directionY = GetFittedAxisRaw("Jump");

        if (feet.isTriggered)
        {
            _playerAnimator.SetBool(_isJumpHash, false);
            _nbJumpLeft = MaxNbJumps;
        }
        else
        {
            _playerAnimator.SetBool(_isJumpHash, true);
        }

        if (isFrozen)
        {
            return;
        }

        if (directionY == 1 && Input.anyKeyDown)
        {
            Jump();
        }

        if (directionX != 0 && Input.anyKeyDown)
        {
            _rigidbody.AddForce(new Vector2(directionX * Time.deltaTime * Speed, 0));
        }

        if (directionX != 0)
        {
            transform.Translate(new Vector3(directionX * Time.deltaTime * Speed, 0, 0));
            _playerAnimator.SetBool(_isRunHash, true);
            transform.localScale = new Vector3(directionX * _baseScale.x, _baseScale.y, _baseScale.z);
        }
        else
        {
            _playerAnimator.SetBool(_isRunHash, false);
        }
    }
}