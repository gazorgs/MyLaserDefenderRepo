using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // configuration parameters
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyExplosionPrefab = default;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip enemyExplosionSound = default;
    [SerializeField] [Range(0, 1)] float enemyExplosionSoundVolume = 0.75f; //using Range caps the slider in the Inspector for this variable
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.5f;

    [Header("Projectile")]
    [SerializeField] GameObject enemyLaserPrefab = default;
    [SerializeField] float enemyProjectileSpeed = 10f;
    [SerializeField] float padding = 0.5f;    

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime; //each frame count down by how long a frame takes to execute
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(
                    enemyLaserPrefab,
                    transform.position + new Vector3(0, -padding, 0), //offset the laser from the centre pivot of the enemy ship slightly
                    Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyProjectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other) // you can change the default 'collision' to anything, in this case 'other' to denote the 'other' thing bumping into Enemy
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>(); //store value of DamageDealer component of thing that has bumped into Enemy
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage(); //decrease health by calling GetDamage() method from DamageDealer.cs component on other gameObject
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(enemyExplosionSound, Camera.main.transform.position, enemyExplosionSoundVolume); 
        Destroy(gameObject);
        GameObject enemyExplosion = Instantiate(
            enemyExplosionPrefab,
            transform.position,
            transform.rotation) as GameObject;
        Destroy(enemyExplosion, durationOfExplosion);
    }
}
