using UnityEngine;

/// <summary>
/// Controla o comportamento de inimigos
/// Inclui movimento, disparo de projéteis e morte por pulo na cabeça
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 2.5f; // Distância maior para andar mais pela plataforma
    
    [Header("Configurações de Tiro")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 2f; // Intervalo entre tiros
    [SerializeField] private float projectileSpeed = 5f;
    
    [Header("Configurações de Vida")]
    [SerializeField] private bool isAlive = true;
    
    [Header("Componentes")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    
    // Estados do inimigo
    private Vector3 startPosition;
    private float moveDirection = 1f; // 1 = direita, -1 = esquerda
    private float lastShootTime = 0f;
    private bool facingRight = true;
    
    void Start()
    {
        InitializeComponents();
        SetupEnemy();
    }
    
    void Update()
    {
        if (isAlive)
        {
            // Inimigos ficam parados - só atiram
            CheckShoot();
        }
    }
    
    /// <summary>
    /// Inicializa os componentes do inimigo
    /// </summary>
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f; // Inimigo não cai
        rb.isKinematic = true; // Inimigo fica parado (sem física)
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Procura o jogador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    
    /// <summary>
    /// Configura o inimigo
    /// </summary>
    private void SetupEnemy()
    {
        startPosition = transform.position;
        CreateEnemySprite();
        
        // Adiciona Collider2D sólido para física
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.8f, 1.2f);
            collider.isTrigger = false; // Sólido para física
        }
        
        // Adiciona um segundo collider como trigger para detectar colisões com jogador
        GameObject triggerObj = new GameObject("EnemyTrigger");
        triggerObj.transform.SetParent(transform);
        triggerObj.transform.localPosition = Vector3.zero;
        BoxCollider2D triggerCollider = triggerObj.AddComponent<BoxCollider2D>();
        triggerCollider.size = new Vector2(0.8f, 1.2f);
        triggerCollider.isTrigger = true;
        
        // Adiciona script para detectar colisões
        EnemyTrigger enemyTrigger = triggerObj.AddComponent<EnemyTrigger>();
        enemyTrigger.SetEnemy(this);
        
        // Configura tag
        gameObject.tag = "Enemy";
        
        Debug.Log("Inimigo criado!");
    }
    
    /// <summary>
    /// Cria sprite para o inimigo
    /// </summary>
    private void CreateEnemySprite()
    {
        Texture2D texture = new Texture2D(32, 48);
        Color[] pixels = new Color[32 * 48];
        
        // Desenha um inimigo vermelho
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 48; y++)
            {
                // Corpo (vermelho)
                if (x >= 8 && x <= 23 && y >= 16 && y <= 40)
                {
                    pixels[y * 32 + x] = Color.red;
                }
                // Cabeça (vermelho escuro)
                else if (x >= 10 && x <= 21 && y >= 24 && y <= 40)
                {
                    pixels[y * 32 + x] = new Color(0.8f, 0f, 0f, 1f);
                }
                // Pernas (vermelho escuro)
                else if ((x >= 10 && x <= 13 && y >= 8 && y <= 16) || 
                         (x >= 18 && x <= 21 && y >= 8 && y <= 16))
                {
                    pixels[y * 32 + x] = new Color(0.6f, 0f, 0f, 1f);
                }
                // Braços (vermelho escuro)
                else if ((x >= 4 && x <= 7 && y >= 20 && y <= 32) || 
                         (x >= 24 && x <= 27 && y >= 20 && y <= 32))
                {
                    pixels[y * 32 + x] = new Color(0.6f, 0f, 0f, 1f);
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
        spriteRenderer.sortingOrder = 1;
    }
    
    /// <summary>
    /// Move o inimigo para frente e para trás
    /// </summary>
    private void MoveEnemy()
    {
        if (rb == null) return;
        
        // Verifica se há chão na frente antes de mover
        if (CheckGroundAhead())
        {
            // Move o inimigo
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Para o movimento se não há chão na frente
            rb.velocity = new Vector2(0f, rb.velocity.y);
            
            // Inverte direção se não há chão na frente
            moveDirection *= -1f;
            facingRight = !facingRight;
            spriteRenderer.flipX = !facingRight;
            Debug.Log("🔄 Inimigo inverteu direção - sem chão na frente");
        }
        
        // Verifica se deve inverter direção por distância (só se não está perto de borda)
        float distanceFromStart = Mathf.Abs(transform.position.x - startPosition.x);
        if (distanceFromStart >= moveDistance && CheckGroundAhead())
        {
            moveDirection *= -1f;
            facingRight = !facingRight;
            spriteRenderer.flipX = !facingRight;
            Debug.Log("🔄 Inimigo inverteu direção - distância máxima");
        }
    }
    
    /// <summary>
    /// Verifica se há chão na frente do inimigo
    /// </summary>
    private bool CheckGroundAhead()
    {
        // Posição do check de chão (mais perto da borda)
        Vector2 checkPosition = new Vector2(
            transform.position.x + (moveDirection * 0.8f), // 0.8 unidades na frente (mais perto da borda)
            transform.position.y - 0.8f // 0.8 unidades abaixo
        );
        
        // Verifica se há chão usando Raycast
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 0.3f);
        
        // Debug visual
        Debug.DrawRay(checkPosition, Vector2.down * 0.3f, hit.collider != null ? Color.green : Color.red);
        
        return hit.collider != null;
    }
    
    /// <summary>
    /// Verifica se deve atirar
    /// </summary>
    private void CheckShoot()
    {
        if (player == null) return;
        
        // Verifica se o jogador está próximo (inimigo atira em qualquer direção)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer < 8f && Time.time - lastShootTime >= shootInterval)
        {
            // Determina direção do tiro baseada na posição do jogador
            Vector2 shootDirection = (player.position - transform.position).normalized;
            Shoot(shootDirection);
            lastShootTime = Time.time;
        }
    }
    
    /// <summary>
    /// Atira um projétil na direção especificada
    /// </summary>
    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null)
        {
            // Cria projétil se não existir prefab
            CreateProjectile(direction);
        }
        else
        {
            // Instancia projétil do prefab
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.SetDirection(direction);
                projectileController.SetSpeed(projectileSpeed);
            }
        }
        
        Debug.Log("💥 Inimigo atirou!");
    }
    
    /// <summary>
    /// Cria um projétil simples na direção especificada
    /// </summary>
    private void CreateProjectile(Vector2 direction)
    {
        GameObject projectile = new GameObject("Projectile");
        projectile.transform.position = transform.position;
        
        // Adiciona ProjectileController
        ProjectileController projectileController = projectile.AddComponent<ProjectileController>();
        projectileController.SetDirection(direction);
        projectileController.SetSpeed(projectileSpeed);
        
        Debug.Log("💥 Projétil criado!");
    }
    
    // Colisões agora são detectadas pelo EnemyTrigger
    
    /// <summary>
    /// Mata o inimigo - desaparece imediatamente
    /// </summary>
    public void Die()
    {
        if (!isAlive) return;
        
        isAlive = false;
        Debug.Log("💀 Inimigo morto! Despawnando...");
        
        // Destrói o inimigo imediatamente
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Desenha gizmos para debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Desenha área de movimento
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition + Vector3.left * moveDistance, startPosition + Vector3.right * moveDistance);
        
        // Desenha direção do movimento
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.right * moveDirection * 2f);
    }
}
