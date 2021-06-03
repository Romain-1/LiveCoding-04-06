using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAttack : MonoBehaviour
{
    [SerializeField] private float damage = 0.1f;

    [SerializeField] private float percentageIncrease = 12f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pa = other.GetComponent<PlayerActions>();
        Vector2 direction;

        if (pa)
        {
            print(transform.localScale);
            direction.x = Mathf.Clamp(other.transform.position.x - transform.position.x, -1, 1);
            direction.y = Mathf.Clamp(other.transform.position.y - transform.position.y, -1, 1);
            pa.TakeDamage(damage, percentageIncrease, direction);
        }
    }
}
