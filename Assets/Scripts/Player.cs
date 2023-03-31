using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;
    public float topEdge;
    public AudioSource click;
    public AudioSource death;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        topEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)).y;
        click = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;

    }
    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && transform.position.y < topEdge)
        {
            direction = Vector3.up * strength;
            GetComponent<AudioSource>().Play();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
                GetComponent<AudioSource>().Play();
            }
        }
        direction.y += gravity * Time.deltaTime;
        transform.position += direction*Time.deltaTime;
    }

    private void AnimateSprite()
    {
        spriteIndex++;
        if(spriteIndex>=sprites.Length)
        {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
            death.Play();
        }
        else if(collision.gameObject.tag=="Scoring")
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }
}
