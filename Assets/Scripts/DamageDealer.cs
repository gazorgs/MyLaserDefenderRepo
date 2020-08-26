using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;
    
    public int GetDamage() //return int damage value
    {
        return damage;
    }

    public void Hit() //void as we don't need to return anything
    {
        Destroy(gameObject);
    }
    
}
