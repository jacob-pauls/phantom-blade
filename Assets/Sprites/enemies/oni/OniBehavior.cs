using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBehavior : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 25;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }

    public void TakeHit(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
