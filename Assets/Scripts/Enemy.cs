using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) // you can change the default 'collision' to anything, in this case 'other' to denote the 'other' thing bumping into Enemy
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>(); //store value of DamageDealer component of thing that has bumped into Enemy
        health -= damageDealer.GetDamage(); //decrease health by calling GetDamage() method from DamageDealer.cs component on other gameObject
    }
}
