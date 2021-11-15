using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public AudioClip ThrowCogClip;
    public AudioSource soundSource;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        soundSource.clip = ThrowCogClip;
        soundSource.Play();

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
        EnemyController2 d = other.collider.GetComponent<EnemyController2>();
        if (d != null)
        {
            d.Fix();
        }
        Destroy(gameObject);
        HardEnemyController f = other.collider.GetComponent<HardEnemyController>();
        if (f != null)
        {
            f.Fix();
        }
        Destroy(gameObject);
        HardEnemyController2 g = other.collider.GetComponent<HardEnemyController2>();
        if (g != null)
        {
            g.Fix();
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
}
