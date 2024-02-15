using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float min_Y, max_Y;
    [SerializeField] private GameObject player_Bullet;
    [SerializeField] private Transform attack_Point;
    public float attack_Timer = 0.35f;
    private float current_Attack_Timer;
    private bool canAttack;
    private AudioSource laserAudio;
    private Animator anim;
    private AudioSource explosionSound;
    [SerializeField] private GameObject gameOverText;
    

    void Awake()
    {
        laserAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        explosionSound = GetComponent<AudioSource>();
    }
    void Start()
    {
        current_Attack_Timer = attack_Timer;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Attack();
    }
    void MovePlayer()
    {
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > max_Y)
                temp.y = max_Y;

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if (temp.y < min_Y)
                temp.y = min_Y;

            transform.position = temp;
        }
    }
    void Attack()
    {
        attack_Timer += Time.deltaTime;
        if (attack_Timer > current_Attack_Timer)
        {
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canAttack)
            {
                canAttack = false;
                attack_Timer = 0f;
                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);
                laserAudio.Play();
            }

        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Bullet" || target.tag == "Enemy1" || target.tag == "Enemy2" || target.tag == "Enemy3")
        {
            explosionSound.Play();
            anim.Play("Destroy");
            StartCoroutine(PauseGameForSeconds(1f));

            
        }
       
    }
    

    IEnumerator PauseGameForSeconds(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        Time.timeScale = 0f;
        gameOverText.SetActive(true); 
    }
}
