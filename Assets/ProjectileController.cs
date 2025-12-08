using UnityEngine;

/// <summary>
/// Controla o comportamento de projéteis
/// Inclui movimento, colisão com jogador e destruição automática
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f; // Tempo de vida do projétil
    [SerializeField] private Vector2 direction = Vector2.right;
    
    [Header("Componentes")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        InitializeComponents();
        SetupProjectile();
        
        // Destrói o projétil após o tempo de vida
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        // Move o projétil
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }
    
    /// <summary>
    /// Inicializa os componentes do projétil
    /// </summary>
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f; // Projétil não cai
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
    
    /// <summary>
    /// Configura o projétil
    /// </summary>
    private void SetupProjectile()
    {
        CreateProjectileSprite();
        
        // Adiciona Collider2D como trigger - HITBOX MAIOR para acertar mais fácil
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f; // Aumentado de 0.2f para 0.5f
            collider.isTrigger = true;
        }
        
        // Configura tag
        gameObject.tag = "Projectile";
        
        Debug.Log("🔥 Projétil criado com hitbox grande!");
    }
    
    /// <summary>
    /// Cria sprite para o projétil - BOLA DE FOGO MAIOR
    /// </summary>
    private void CreateProjectileSprite()
    {
        // Textura maior para bola de fogo mais visível
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Desenha uma bola de fogo grande
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                // Projétil circular - bola de fogo
                float centerX = 15.5f;
                float centerY = 15.5f;
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                
                if (distance <= 8f)
                {
                    // Centro amarelo brilhante
                    pixels[y * 32 + x] = new Color(1f, 1f, 0.3f, 1f); // Amarelo
                }
                else if (distance <= 12f)
                {
                    // Meio laranja
                    pixels[y * 32 + x] = new Color(1f, 0.5f, 0f, 1f); // Laranja
                }
                else if (distance <= 15f)
                {
                    // Borda vermelha
                    pixels[y * 32 + x] = new Color(1f, 0.2f, 0f, 0.8f); // Vermelho
                }
                else
                {
                    pixels[y * 32 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Sprite maior (32x32) com pixels per unit menor para aparecer maior na tela
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32f);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 2;
    }
    
    /// <summary>
    /// Define a direção do projétil
    /// </summary>
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    /// <summary>
    /// Define a velocidade do projétil
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    /// <summary>
    /// Detecta colisões - APENAS com o jogador (ignora plataformas)
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Projétil acertou o jogador
            PlayerController2D playerController = other.GetComponent<PlayerController2D>();
            if (playerController != null)
            {
                Debug.Log("🔥💥 Bola de fogo acertou o jogador!");
                playerController.Die();
            }
            
            // Destrói o projétil
            Destroy(gameObject);
        }
        // Ignora plataformas (Ground) e inimigos - passa através deles
    }
    
    /// <summary>
    /// Desenha gizmos para debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Desenha direção do projétil
        Gizmos.color = new Color(1f, 0.5f, 0f, 1f); // Laranja
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
}
