using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f; // change this in the Inspector to get the Player move speed to what feels best
    [SerializeField] float padding = 0.5f;
    //[SerializeField] float yPadding = 1f;  //maybe implement later to stop Player moving too far up the screen
    [SerializeField] int health = 200;

    [Header("Audio")]
    [SerializeField] AudioClip playerExplosionSound = default;
    [SerializeField] [Range(0, 1)] float playerExplosionSoundVolume = 0.75f; //using Range caps the slider in the Inspector for this variable
    [SerializeField] AudioClip shootSound = default;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine = default;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other) // you can change the default 'collision' to anything, in this case 'other' to denote the 'other' thing bumping into Player
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>(); //store value of DamageDealer component of thing that has bumped into Player
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
        AudioSource.PlayClipAtPoint(playerExplosionSound, Camera.main.transform.position, playerExplosionSoundVolume);
        Destroy(gameObject);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously()); //store the Coroutine object created as a variable so we can stop it later
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                    laserPrefab,
                    transform.position + new Vector3(0, padding, 0), //offset the laser from the centre pivot of the player ship slightly
                    Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        //in Unity go to Edit - Project Settings - Input Manager (chose Horizontal and Vertical keys and mouse/gamepad axis for this one)
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; //Time.delta.Time ensures framerate independent whether on fast or slow computer
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); //new Player x position equal to current position plus the key press above, clamped to fit
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax); 

        transform.position = new Vector2(newXPos, newYPos); //Move Player to new position
    }

    private void SetUpMoveBoundaries()
    {
        // future change: try to use GetComponent().bounds.size.x, and GetComponent().bounds.size.y (in relation to the Sprite.Renderer for the padding)
        Camera gameCamera = Camera.main; // need to say what camera we are referring to, so declare it as a local variable here
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding; // what is the World space value for the x element of our ViewportToWorldPoint
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding; // far right of screen

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding; // what is the World space value for the y element of our ViewportToWorldPoint
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding; // top of screen
    }
}
