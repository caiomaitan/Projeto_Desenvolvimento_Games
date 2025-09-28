using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador de jogador para jogo 2D estilo Mario
/// Inclui movimento horizontal, pulo e física 2D
/// </summary>
public class PlayerController2D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 14f; // Aumentado para pulos mais altos
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    
    [Header("Configurações de Física")]
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private float maxFallSpeed = -15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    
    [Header("Configurações de Detecção")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f; // Aumentado para detectar melhor
    [SerializeField] private LayerMask groundLayerMask = -1; // Detecta todas as layers por enquanto
    
    [Header("Configurações de Morte")]
    [SerializeField] private float deathY = -10f; // Altura para considerar morte
    [SerializeField] private float respawnDelay = 2f; // Tempo antes de reiniciar
    
    [Header("Componentes")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    // Estados do jogador
    private bool isGrounded = false;
    private bool wasGrounded = false;
    private float jumpBufferCounter = 0f;
    private float moveInput = 0f;
    private bool jumpInput = false;
    private bool isDead = false;
    private Vector3 startPosition;
    private bool hasJumped = false; // Controla se já pulou para evitar pulo infinito
    
    /// <summary>
    /// Inicializa os componentes do jogador
    /// </summary>
    void Start()
    {
        InitializeComponents();
        SetupGroundCheck();
        startPosition = transform.position; // Salva posição inicial
    }
    
    /// <summary>
    /// Configura os componentes necessários
    /// </summary>
    private void InitializeComponents()
    {
        // Obtém Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configura Rigidbody2D
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;
        
        // Obtém SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            CreatePlayerSprite();
        }
        
        // Obtém Animator
        animator = GetComponent<Animator>();
        
        // Adiciona Collider2D se não existir (EXTREMAMENTE menor para cair nos vãos)
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.1f, 0.2f); // EXTREMAMENTE menor
        }
        
        // Configura tag
        gameObject.tag = "Player";
        
        // Debug para verificar hitbox
        Debug.Log($"Jogador criado com hitbox EXTREMAMENTE pequena: {GetComponent<BoxCollider2D>().size}");
    }
    
    /// <summary>
    /// Configura o sistema de detecção de chão
    /// </summary>
    private void SetupGroundCheck()
    {
        // Sempre cria o GroundCheck se não existir
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.2f, 0); // Mais próximo do chão
            groundCheck = groundCheckObj.transform;
            Debug.Log("GroundCheck criado automaticamente!");
        }
    }
    
    /// <summary>
    /// Cria sprite simples para o jogador
    /// </summary>
    private void CreatePlayerSprite()
    {
        Texture2D texture = new Texture2D(32, 48);
        Color[] pixels = new Color[32 * 48];
        
        // Desenha um personagem simples
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 48; y++)
            {
                // Corpo (azul)
                if (x >= 8 && x <= 23 && y >= 16 && y <= 40)
                {
                    pixels[y * 32 + x] = Color.blue;
                }
                // Cabeça (vermelha)
                else if (x >= 10 && x <= 21 && y >= 24 && y <= 40)
                {
                    pixels[y * 32 + x] = Color.red;
                }
                // Pernas (azul escuro)
                else if ((x >= 10 && x <= 13 && y >= 8 && y <= 16) || 
                         (x >= 18 && x <= 21 && y >= 8 && y <= 16))
                {
                    pixels[y * 32 + x] = new Color(0, 0, 0.8f, 1f);
                }
                // Braços (azul escuro)
                else if ((x >= 4 && x <= 7 && y >= 20 && y <= 32) || 
                         (x >= 24 && x <= 27 && y >= 20 && y <= 32))
                {
                    pixels[y * 32 + x] = new Color(0, 0, 0.8f, 1f);
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 48), new Vector2(0.5f, 0.5f), 32f);
        spriteRenderer.sprite = sprite;
    }
    
    /// <summary>
    /// Captura input do jogador
    /// </summary>
    void Update()
    {
        if (!isDead)
        {
            HandleInput();
            CheckGrounded();
            HandleCoyoteTime();
            HandleJumpBuffer();
            CheckDeath();
        }
    }
    
    /// <summary>
    /// Aplica física e movimento
    /// </summary>
    void FixedUpdate()
    {
        if (!isDead)
        {
            ApplyMovement();
            ApplyGravity();
        }
    }
    
    /// <summary>
    /// Captura input do jogador
    /// </summary>
    private void HandleInput()
    {
        // Movimento horizontal
        moveInput = Input.GetAxisRaw("Horizontal");
        
        // Pulo
        jumpInput = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        
        // Atualiza animações
        UpdateAnimations();
    }
    
    /// <summary>
    /// Verifica se o jogador está no chão
    /// </summary>
    private void CheckGrounded()
    {
        wasGrounded = isGrounded;
        
        // Verifica se groundCheck existe antes de usar
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
            
            // Debug contínuo para verificar o estado
            if (isGrounded != wasGrounded)
            {
                Debug.Log($"🔍 GroundCheck: {isGrounded} (posição: {groundCheck.position}, raio: {groundCheckRadius})");
                
                // Verifica o que está sendo detectado
                Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayerMask);
                foreach (Collider2D col in colliders)
                {
                    Debug.Log($"🔍 Detectado: {col.name} (tag: {col.tag}, layer: {col.gameObject.layer})");
                }
            }
            
            // Debug contínuo quando está no chão
            if (isGrounded)
            {
                Debug.Log($"✅ ESTÁ NO CHÃO - posição: {groundCheck.position} - hasJumped: {hasJumped}");
            }
            else
            {
                Debug.Log($"❌ FORA DO CHÃO - posição: {groundCheck.position} - hasJumped: {hasJumped}");
            }
        }
        else
        {
            // Se não tiver groundCheck, cria um
            SetupGroundCheck();
            isGrounded = false; // Assume que não está no chão
        }
        
        // Reset hasJumped quando toca o chão (transição)
        if (isGrounded && !wasGrounded)
        {
            hasJumped = false;
            Debug.Log("🦶 Tocou o chão - pode pular novamente");
        }
        // Também reseta se está no chão e parado (velocidade Y próxima de zero)
        else if (isGrounded && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            hasJumped = false;
            Debug.Log("🦶 Reset no chão - pode pular novamente");
        }
    }
    
    /// <summary>
    /// Gerencia o tempo de coyote (pulo após sair da plataforma) - DESABILITADO
    /// </summary>
    private void HandleCoyoteTime()
    {
        // Coyote time desabilitado para evitar pulo infinito
        // Método mantido para compatibilidade
    }
    
    /// <summary>
    /// Gerencia o buffer de pulo (pulo antes de tocar o chão)
    /// </summary>
    private void HandleJumpBuffer()
    {
        if (jumpInput)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        // Garante que o buffer não seja negativo
        if (jumpBufferCounter < 0f)
        {
            jumpBufferCounter = 0f;
        }
    }
    
    /// <summary>
    /// Aplica movimento horizontal
    /// </summary>
    private void ApplyMovement()
    {
        if (rb == null) return;
        
        // Calcula velocidade alvo
        float targetVelocity = moveInput * moveSpeed;
        
        // Aplica aceleração/desaceleração
        float currentVelocity = rb.velocity.x;
        float velocityChange = targetVelocity - currentVelocity;
        
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            // Acelera
            float accelerationForce = velocityChange * acceleration;
            rb.AddForce(Vector2.right * accelerationForce);
        }
        else
        {
            // Desacelera
            float decelerationForce = -currentVelocity * deceleration;
            rb.AddForce(Vector2.right * decelerationForce);
        }
        
        // Limita velocidade máxima
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);
        }
    }
    
    /// <summary>
    /// Aplica gravidade e pulo (SEM DOUBLE JUMP)
    /// </summary>
    private void ApplyGravity()
    {
        if (rb == null) return;
        
        // Aplica pulo APENAS se está no chão e ainda não pulou
        if (jumpBufferCounter > 0f && isGrounded && !hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f; // Limpa o buffer após pular
            hasJumped = true; // Marca que já pulou
            Debug.Log("🦘 PULO APLICADO! (estava no chão e não havia pulado)");
        }
        else if (jumpBufferCounter > 0f && isGrounded && hasJumped)
        {
            Debug.Log("❌ PULO BLOQUEADO! (já pulou e ainda está no chão)");
            jumpBufferCounter = 0f; // Limpa o buffer se já pulou
        }
        
        // Limita velocidade de queda
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }
    
    /// <summary>
    /// Atualiza animações do jogador
    /// </summary>
    private void UpdateAnimations()
    {
        // Vira o sprite baseado na direção
        if (moveInput > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        
        // Atualiza animações se tiver Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VelocityY", rb.velocity.y);
        }
    }
    
    /// <summary>
    /// Detecta colisões com itens coletáveis - VERSÃO SIMPLIFICADA
    /// </summary>
    /// <param name="other">Objeto que colidiu</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            // Coleta o item diretamente
            CollectibleItem collectible = other.GetComponent<CollectibleItem>();
            if (collectible != null)
            {
                collectible.Collect();
                Debug.Log("Item coletado!");
            }
            else
            {
                // Se não tiver CollectibleItem, apenas destrói
                Destroy(other.gameObject);
                Debug.Log("Item coletado (sem script)!");
            }
        }
        else if (other.CompareTag("FinishLine"))
        {
            // Chegou na linha de chegada - avança para próxima fase
            Debug.Log("🏁 CHEGOU NA LINHA DE CHEGADA!");
            Debug.Log($"🏁 Tag do objeto: {other.tag}");
            Debug.Log($"🏁 Nome do objeto: {other.name}");
            Debug.Log($"🏁 Posição do jogador: {transform.position}");
            Debug.Log($"🏁 Posição da linha de chegada: {other.transform.position}");
            
            GameSetupComplete gameSetup = FindObjectOfType<GameSetupComplete>();
            if (gameSetup != null)
            {
                Debug.Log("🏁 GameSetupComplete encontrado! Chamando OnReachFinishLine...");
                gameSetup.OnReachFinishLine();
                Debug.Log("🏁 OnReachFinishLine chamado com sucesso!");
            }
            else
            {
                Debug.Log("❌ ERRO: GameSetupComplete não encontrado!");
            }
        }
    }
    
    /// <summary>
    /// Verifica se o jogador morreu (caiu muito baixo)
    /// </summary>
    private void CheckDeath()
    {
        if (transform.position.y < deathY)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Mata o jogador e inicia processo de reinício da fase
    /// </summary>
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("💀 JOGADOR MORREU! Voltando para Fase 1 em " + respawnDelay + " segundos...");
        
        // Reseta a pontuação quando o jogador morre
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            Debug.Log("💀 Pontuação resetada para 0!");
        }
        
        // Para o movimento
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        // Mostra mensagem de morte
        ShowDeathMessage();
        
        // Reinicia a fase após delay
        Invoke(nameof(Respawn), respawnDelay);
    }
    
    /// <summary>
    /// Mostra mensagem de morte
    /// </summary>
    private void ShowDeathMessage()
    {
        // Cria objeto visual para mostrar mensagem
        GameObject deathMessage = new GameObject("DeathMessage");
        deathMessage.transform.position = transform.position + Vector3.up * 2f;
        
        // Adiciona SpriteRenderer para mostrar texto visual
        SpriteRenderer messageRenderer = deathMessage.AddComponent<SpriteRenderer>();
        messageRenderer.sprite = CreateDeathMessageSprite();
        messageRenderer.sortingOrder = 10; // Acima de tudo
        
        // Remove após delay
        Destroy(deathMessage, respawnDelay);
    }
    
    /// <summary>
    /// Cria sprite para mensagem de morte
    /// </summary>
    private Sprite CreateDeathMessageSprite()
    {
        Texture2D texture = new Texture2D(128, 32);
        Color[] pixels = new Color[128 * 32];
        
        // Preenche com fundo vermelho
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(1f, 0f, 0f, 0.8f); // Vermelho semi-transparente
        }
        
        // Desenha texto simples "VOLTANDO FASE 1"
        for (int x = 0; x < 128; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                // Texto "VOLTANDO FASE 1" em branco (simplificado)
                if ((x >= 5 && x <= 15 && y >= 10 && y <= 22) || // V
                    (x >= 20 && x <= 30 && y >= 10 && y <= 22) || // O
                    (x >= 35 && x <= 45 && y >= 10 && y <= 22) || // L
                    (x >= 50 && x <= 60 && y >= 10 && y <= 22) || // T
                    (x >= 65 && x <= 75 && y >= 10 && y <= 22) || // A
                    (x >= 80 && x <= 90 && y >= 10 && y <= 22) || // N
                    (x >= 95 && x <= 105 && y >= 10 && y <= 22)) // D
                {
                    pixels[y * 128 + x] = Color.white;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 128, 32), new Vector2(0.5f, 0.5f), 32f);
    }
    
    /// <summary>
    /// Reinicia sempre para a Fase 1 quando o jogador morre
    /// </summary>
    private void Respawn()
    {
        Debug.Log("🔄 REINICIANDO PARA FASE 1...");
        
        // Pega o nome da cena atual para debug
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"🔄 Morreu na cena: {currentSceneName}");
        
        // SEMPRE volta para a Fase 1 (cena "Game")
        string phase1SceneName = "Game";
        Debug.Log($"🔄 Voltando para: {phase1SceneName}");
        
        // Carrega a Fase 1
        SceneManager.LoadScene(phase1SceneName);
        
        Debug.Log("✅ Voltou para Fase 1 com sucesso!");
    }
    
    /// <summary>
    /// Desenha gizmos para debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        else
        {
            // Se não tiver groundCheck, desenha na posição do jogador
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.1f, groundCheckRadius);
        }
        
        // Desenha linha de morte
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-20f, deathY, 0f), new Vector3(20f, deathY, 0f));
    }
}