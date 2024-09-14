using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeste : MonoBehaviour
{
    [Header("Movement")]
    public float speed;                         //velocidade de movimento do jogador
    public int jumpForce;                       //força do pulo
    public float horizontal;                    //armazena o input no eixo da horizontal
    public bool jumpPressed;                    //identifica se o botão do pulo foi pressionado ou não
    public int direction = 1;                   //identifica a direção do jogador (1 direita, -1 esquerda)
    public bool canMove = true;                 //identifica se pode movimentar ou não
    public bool canJump = true;                 //identifica se pode pular ou não


    [Header("Ground Check")]
    private bool grounded = false;              //variável que identifica se jogador está no chão ou não
    public Transform groundCheck;               //objeto que serve como referência pra fazer checagem com o chão
    public float footOffest = 0.4f;             //distância até o pé do personagem
    public LayerMask groundLayer;               //máscara de camada do chão
    public float groundDistance = 0.1f;         //distância com que faz a checagem com o chão

    [Header("Tunel")]
    public float climbSpeed = 3;                //velocidade de subida no tunel
    public LayerMask tunnelMask;                //máscara de camada do tunel
    public float vertical;                      //armazena o input do eixo da vertical
    public bool climbing;                       //identifica se jogador está escalando o tunel
    public float checkRadius = 0.3f;            //raio de checagem com o tunel
    public Transform tunnelCheck;

    [Header("foot effect")]
    public ParticleSystem footSteps;
    private ParticleSystem.EmissionModule footEmission;
    public ParticleSystem impactEffect;
    private bool wasOnGround;

    public float coyoteTime = .2f;
    private float coyoteCounter;

    public float jumpBufferLength = .1f;
    private float jumpBufferCount;


    private Collider2D col;                     //armazena collider do jogador
    private Rigidbody2D rb2d;                   //armazena rigidbody do jogador
    private Animator anim;                      //armazena animator do jogador
    public GameControler gameControler;

    private bool clearInputs;                   //identifica quando pode fazer limpeza nos inputs

    private AudioEffect audioF;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();                     //referencia o rigidbody do jogador na variável
        anim = GetComponent<Animator>();                        //referencia o animator do jogador na variável
        col = GetComponent<Collider2D>();                       //referencia o collider do jogador na variável
        gameControler = FindObjectOfType<GameControler>();      //referencia o objeto GameControler na variável
        footEmission = footSteps.emission;                      //referencia as particulas emitidas na variável
        audioF = GetComponent<AudioEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));


        CheckInputs();
        SetAnimations();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            gameControler.RespawnPlayer();
        }


        if (collision.gameObject.tag == "Plataforma")
        {
            this.transform.parent = collision.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            this.transform.parent = null;
        }
    }

    void FixedUpdate()
    {
        //funções de movimento são chamadas no FixedUpdate
        GroundMovement();
        AirMovement();
        ClimbTunnel();
        footeffect();

        //toda vez que o FixedUpdate é chamado, informa que pode limpar os inputs
        clearInputs = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            audioF.PlaySFX(audioF.audioWater);
            Debug.Log("aaaaaaaaa");
            rb2d.gravityScale = rb2d.gravityScale / 4;
            jumpForce = jumpForce / 3;
            speed = speed / 2;
            Debug.Log("verdade");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        rb2d.gravityScale = 1;
        jumpForce = 13;
        speed = 5;
    }

    bool TouchingTunnel()
    {
        //função que retorna se jogador está se colidindo com o tunel
        return col.IsTouchingLayers(tunnelMask);
    }

    void ClimbTunnel()
    {
        //Up verifica se tem tunel acima, e down se tem tunel abaixo
        bool up = Physics2D.OverlapCircle(tunnelCheck.position, checkRadius, tunnelMask);
        bool down = Physics2D.OverlapCircle(transform.position + new Vector3(0, -1), checkRadius, tunnelMask);

        if (vertical != 0 && TouchingTunnel())              //se o input da vertical for pressionado e o jogador esiver se colidindo com o tunel
        {
            climbing = true;                                //passa climbing pra verdadeiro 
            rb2d.isKinematic = true;                        //coloca o rigidbody como kinematic pra evitar interferência da física
            canMove = false;                                //impede movimentação do jogador na horizontal
            canJump = false;
        }

        if (climbing)
        {
            //Quando climbing for verdadeiro
            //Se não estiver tunel acima ou abaixo, termina a escalada
            if (!up && vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!down && vertical <= 0)
            {
                FinishClimb();
                return;
            }

            float y = vertical * climbSpeed;                //armazena o input da vertical multiplicado pela velocidade de subida
            rb2d.velocity = new Vector2(0, y);              //atualiza velocidade do rigdbody de acordo com velocidade em y armazenada

            //se o pulo for pressionado
            if (jumpPressed && canJump)
            {
                col.isTrigger = true;                       //coloca o collider em trigger
                FinishClimb();                              //finaliza a escalada
                canMove = false;                            //permanece sem poder se movimentar na horizontal

                float x = direction;                        //armazena o valor da direção numa nova variável x
                if (horizontal != 0)                        //se o input da horizontal for pressionado 
                    x = horizontal > 0 ? 1 : -1;            //se o valor da horizontal for maior que 0, valor de x = 1, senão, x = -1

                //Caso x seja diferente da direção previamente armazenada, faz o Flip do jogador
                if (x* direction < 0)
                {
                Flip();
                }
            }
        }
    }

    void FinishClimb()
    {
        //Função que finaliza a escalada
        /*Coloca climbing pra falso
         * tira o rigidbody de kinematic
         * pode movimentar novamente o jogador na horizontal
         * Chama uma função pra resetar o Climbing depois de um décimo
         * atualiza o parâmetro Climbing do Animator pra falso
         */
        climbing = false;
        rb2d.isKinematic = false;
        canMove = true;
        canJump = true;
        Invoke("ResetClimbing", 0.1f);
    }

    void ResetClimbng()
    {
        if (!col.isTrigger)
            return;
        //Função serve para passar movimento para verdadeiro e tirar collider de trigger
        //Enquanto estiver se colidindo com o chão, permanece em trigger
        //Isso impede de ficar preso numa plataforma
        canMove = true;
        canJump = true;
        if (col.IsTouchingLayers(groundLayer))
        {
            Invoke("ResetClimbing", 0.1f);
        }
        else
        {
            col.isTrigger = false;
        }
    }


    void GroundMovement()
    {
        //Função para fazer movimento na horizontal

        //Se não puder se movimentar, retorna da função
        if (!canMove)
            return;

        float x = horizontal * speed;               //Armazena velocidade numa variável x. Multiplica valor do input da horizontal pela velocidade

        rb2d.velocity = new Vector2(x, rb2d.velocity.y);//Atualiza velocidade do rigidbody

        if (x * direction < 0f)                     //Se direção for diferente do input do jogador, faz o Flip
            Flip();

        if (grounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (jumpPressed)
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }
    }

    void AirMovement()
    {
        //Função para executar o pulo do chão

        //Se não puder pular, retorna da função
        if (!canJump)
            return;

        //Se o pulo for pressionado e estiver no chão
        //if (jumpPressed && coyoteCounter > 0f)
        if (jumpBufferCount >= 0 && coyoteCounter > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpBufferCount = 0;
            audioF.PlaySFX(audioF.audioJump);
        }
        if(Input.GetButtonUp("Jump") && rb2d.velocity.y > 0)
        {
            audioF.PlaySFX(audioF.audioJump);
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    void Flip()
    {
        //Função que executa o Flip do jogador

        direction *= -1;                        //Inverte o valor da direção     
        Vector3 scale = transform.localScale;   //Armazena a escala do jogador
        scale.x *= -1;                          //No valor armazena, inverte o valor da escala em X
        transform.localScale = scale;           //Atualiza a escala do jogador de acordo com o novo Vector3 armazenado
    }

    void CheckInputs()
    {
        //Função que serve para checar os inputs

        //Limpa os inputs, faz o botão de pulo voltar para o valor padrão de falso
        if (clearInputs)
        {
            jumpPressed = false;
            clearInputs = false;
        }


        //Armazena se pulo foi pressionado
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");


        //Armazena eixo da horizontal
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    }

    void footeffect()
    {
        //Ativa se o player estiver no chao foodstep effect
        if (Input.GetAxisRaw("Horizontal") != 0 && grounded)
        {
            footEmission.rateOverTime = 35f;
        }
        else
        {
            footEmission.rateOverTime = 0f;
        }

        //ativa o foodstep effect quando o player atinge o chao
        if (!wasOnGround && grounded)
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.transform.position = footSteps.transform.position;
            impactEffect.Play();
        }
        wasOnGround = grounded;
    }

    void OnDrawGizmos()
    {
        //Função que mostra os raios de colisão que fazem a checagem com o tunel
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(tunnelCheck.position, checkRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -1), checkRadius);
    }

    public RaycastHit2D Raycast(Vector2 origin, Vector2 rayDirection, float length, LayerMask mask, bool drawRay = true)
    {
        //Função que retorna um RaycastHit2D

        //Envia o raycast e salva o resultado na variável hit
        RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, length, mask);


        //Se quisermos mostrar o raycast em cena
        if (drawRay)
        {
            Color color = hit ? Color.red : Color.green;

            Debug.DrawRay(origin, rayDirection * length, color);
        }
        //determina a cor baseado se o raycast se colidiu ou não

        //Retorna o resultado do hit
        return hit;
    }

    void SetAnimations()
    {
        anim.SetFloat("Vely", rb2d.velocity.y);
        anim.SetBool("JumpFall", !grounded);
        anim.SetBool("Walk", rb2d.velocity.x != 0f && grounded);
    }
}
