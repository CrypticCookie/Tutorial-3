using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class RubyController : MonoBehaviour {
    public float speed = 3.0f;
    public static int level = 1;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public ParticleSystem damageEffect;
    public ParticleSystem healthEffect;
    public int health { get { return currentHealth; }}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;

    public TextMeshProUGUI fixedText;
    public TextMeshProUGUI cogsText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject contTextObject;
    private int fixCount;

    private bool gameOver = false;
   // private bool magnet = false;

    public GameObject projectilePrefab;
    private int cogCount = 4;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;
    public AudioClip bgMuse;
    public AudioClip ThrowCogClip;
    public AudioClip PlayerHitClip;
    public AudioClip WinClip;
    public AudioClip LoseClip;
    public AudioClip GetCogClip;
    public AudioClip ribbitClip;
    public AudioClip gruntClip;

    // Start is called before the first frame update
    void Start(){
       winTextObject.SetActive(false);
       loseTextObject.SetActive(false);
       contTextObject.SetActive(false);
       rigidbody2d = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       audioSource = GetComponent<AudioSource>();
       currentHealth = maxHealth;
       fixedText.text = "Robots Fixed: 0/4";
       cogsText.text = "Cogs: " + cogCount;
       audioSource.clip = bgMuse;
       audioSource.Play();
    }

    public void PlaySound(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        //Magnet.transform.position = new Vector2 (transform.position.x, transform.position.y);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible){
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if(Input.GetKeyDown(KeyCode.C)) 
        {
            Launch();
        }
if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == true && level == 2)

            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
                //speed = 3;
            }

            if (gameOver == true && level == 1){
                SceneManager.LoadScene("Level1");
            }

        }
        if (Input.GetKeyDown(KeyCode.X)) {
    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
    if (hit.collider != null) {
    if(level == 2){
        SceneManager.LoadScene("Level2");
       // animator.SetInteger("State", 0);
    }
    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
    if (character != null){
        character.DisplayDialog();
        audioSource.PlayOneShot(ribbitClip);
    }  
}
RaycastHit2D get = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Trader"));
    NonPlayerCharacter trader = get.collider.GetComponent<NonPlayerCharacter>();
    if (trader != null){
        trader.DisplayDialog();
        audioSource.PlayOneShot(gruntClip);
        speed = 6;
        //magnet = true;
    }  
}
//if(magnet == true){
  //  speed = 10;
//}
}
    /*public void Activate(Collider2D other){
        HealthCollectible activate = other.GetComponent<HealthCollectible>();
        if (magnet == true){
        activate.Testing(true);
        }
    }*/

    void FixedUpdate(){
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical* Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount) {
        if (amount > 0){
            ParticleSystem projectilePrefab = Instantiate(healthEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        if (currentHealth <= 0) {
            gameOver = true;
            loseTextObject.SetActive(true);
            //speed = 0;
            //transform.position = new Vector3(0f, 0f, -100f);
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            audioSource.clip = bgMuse;
            audioSource.Stop();
            audioSource.clip = LoseClip;
            audioSource.Play();
        }

        if (amount < 0 && gameOver == true){
            speed = 0;
        }
        
        if (amount < 0){
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

            ParticleSystem projectilePrefab = Instantiate(damageEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(PlayerHitClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }



    void Launch(){
        if(cogCount > 0){
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(lookDirection, 300);

    animator.SetTrigger("Launch");
    audioSource.PlayOneShot(ThrowCogClip);
    cogCount -= 1;
    SetCogText();

       // if(magnet == true){
            //float angle = 45;
           // float spread = Random.Range(0, 20);
           // Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, spread));

           // GameObject bulletObject = (GameObject)GameObject.Instantiate(projectilePrefab, rigidbody2d.position, bulletRotation);
       // }
        }
    }

    public void RobotNum(int amount){
        fixCount += amount;
        fixedText.text = "Robots Fixed: " + fixCount.ToString() + "/4";

        if(fixCount == 4 && level == 1){
            contTextObject.SetActive(true);
            level = 2;
        }      
        else if(level == 2 && fixCount == 4){
            winTextObject.SetActive(true);
            speed = 0;
            audioSource.clip = bgMuse;
            audioSource.Stop();
            audioSource.clip = WinClip;
            audioSource.Play();
            gameOver = true;
            level = 1;
        }  
    }

    void SetCogText(){
        cogsText.text = "Cogs: " + cogCount.ToString(); 
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Ammo"){
            cogCount += 4;
            SetCogText();
            Destroy(other.gameObject);
            audioSource.PlayOneShot(GetCogClip);
         }        
    }

}