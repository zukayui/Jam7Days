using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerScripts : MonoBehaviour
{
    //変数

    Rigidbody2D rb;
    float axisH = 0.0f;
    public float speed = 3.0f;

    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJamp=false;
    bool onGround=false;
    private bool GoalOdj = false;

    Animator animator;
    public string idleAnime = "PlayerIdle";
    public string maveAnimae = "PlayerMove";
    public string jumpAnimae = "PlayerJump";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing";



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = idleAnime;
        oldAnime = idleAnime;

        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");
        if(axisH > 0.0f)
        {
            Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (GoalOdj && Input.GetKeyDown(KeyCode.E))
        {
            Goal();
        }

    }

    private void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }

        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        if(onGround || axisH != 0)
        {
            rb.linearVelocity = new Vector2(speed * axisH, rb.linearVelocityY);
        }
        if (onGround && goJamp)
        {
            Debug.Log("ジャンプ");
            Vector2 jampPw = new Vector2(0, jump);
            rb.AddForce(jampPw, ForceMode2D.Impulse);
            goJamp = false;
        }

        if(onGround)
        {
            if (axisH == 0)
            {
                nowAnime = idleAnime;
            }
            else
            {
                nowAnime = maveAnimae;
            }
        }
        else
        {
            nowAnime = jumpAnimae;
        }

        if(nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }

    public void Jump()
    {
        goJamp=true;
        Debug.Log("ジャンプ押下");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "Goal")
            {
               // Goal();
                GoalOdj=true;
            }
            else if (collision.gameObject.tag == "Dead")
            {
                GameOver();
            }
   
    }

    public void Goal()
    {
            Debug.Log("クリア");
            animator.Play(idleAnime);
            gameState = "GameClear";
            GameStop();
        
    }

    public void GameOver()
    {
        gameState = "GameOver";
        GameStop();

        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.AddForce(new Vector2(0,5),ForceMode2D.Impulse);
    }

    void GameStop()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }
}
