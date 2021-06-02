using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Animator _playerAnimator;
    private PlayerMovement _playerMovement;

    private int BasicAttackHash;
    private int SuperAttackHash1;
    private int SuperAttackHash2;

    private bool _charging;
    private float _chargingCounter;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAnimator = GetComponent<Animator>();
        BasicAttackHash = Animator.StringToHash("basicAttack");
        SuperAttackHash1 = Animator.StringToHash("superAttack1");
        SuperAttackHash2 = Animator.StringToHash("superAttack2");
    }

    void StartSuperAttack()
    {
        _charging = true;
        _chargingCounter = 0f;
        _playerAnimator.SetBool(SuperAttackHash1, true);
    }

    void ReleaseSuperAttack()
    {
        _charging = false;
        _playerAnimator.SetBool(SuperAttackHash1, false);
        _playerAnimator.SetTrigger(SuperAttackHash2);
    }
    
    void BasicAttack()
    {
        _playerAnimator.SetTrigger(BasicAttackHash);
    }

    // Update is called once per frame
    void Update()
    {
        var attacking = _playerMovement.GetFittedAxisRaw("BasicAttack");

        if (!_playerMovement.isFrozen && attacking == 1 && Input.anyKeyDown)
        {
            BasicAttack();
        }

        if (!_playerMovement.isFrozen && attacking == -1 && Input.anyKeyDown)
        {
            StartSuperAttack();
        }

        if (_charging && attacking != -1)
        {
            ReleaseSuperAttack();
        }
        if (_charging)
        {
            _chargingCounter += Time.deltaTime;
        }
    }
}
