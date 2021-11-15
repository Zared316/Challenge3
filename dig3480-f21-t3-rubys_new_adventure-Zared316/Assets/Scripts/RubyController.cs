using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health {get {return currentHealth; }}
    int currentHealth; 

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;
    
    public AudioSource audioSource;

    public GameObject HealthDecrease;
    public GameObject HealthIncrease;
    public int score, ammo;
    public Text scoreText, GameOverText, ammoText;
    bool gameOver;
    
    private BGMController BGMController;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        score = 0;
        ammo = 4;
        scoreText.text = "Fixed Robots: ";
        GameOverText.text = "";
        ammoText.text = "Ammo: " + ammo.ToString();


        GameObject BGMControllerobject = GameObject.FindWithTag("BGMController"); //this line of code finds the BGMController script by looking for a "BGMController" tag on BGM
        if (BGMControllerobject != null)

        {
            
            BGMController = BGMControllerobject.GetComponent<BGMController>(); //and this line of code finds the BGMController and then stores it in a variable

            print ("Found the BGMController Script!");

        }

        if (BGMController == null)
        {

            print ("Cannot find BGMController Script!");

        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
        if(currentHealth == 0)
        {
            speed = 0;
            GameOverText.text = "You Lose, press R to restart";
            gameOver = true;
            BGMController.ChangeBGM2(1);

        }

        if (Input.GetKey(KeyCode.R))
        {

            if (gameOver == true)
            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }

        if(score == 4)
        {
            GameOverText.text = "You win! This game was made by MAS \n press R to restart";
            if (gameOver == true)
            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }
        ammoText.text = "Ammo: " + ammo.ToString();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if( amount > 0)
        {
            Instantiate(HealthIncrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        if(amount < 0)
        {
            animator.SetTrigger("Hit");
            if(isInvincible)
                return;

            isInvincible = true;
            Instantiate(HealthDecrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth/ (float)maxHealth);
    }

    public void IncreaseAmmo(int ammoAmount)
    {
        ammo += 4;
    }

    public void ChangeScore(int scoreAmount)
    {
        score += 1;
        scoreText.text = "Fixed Robots: " + score.ToString();
        BGMController.ChangeBGM(1);
    }

    void Launch()
    {
        if(ammo > 0)
        {
            ammo -= 1;
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

