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
        
        // Adiciona Collider2D como trigger
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = 0.2f;
            collider.isTrigger = true;
        }
        
        // Configura tag
        gameObject.tag = "Projectile";
        
        Debug.Log("Projétil criado!");
    }
    
    /// <summary>
    /// Cria sprite para o projétil
    /// </summary>
    private void CreateProjectileSprite()
    {
        Texture2D texture = new Texture2D(16, 16);
        Color[] pixels = new Color[16 * 16];
        
        // Desenha um projétil laranja
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                // Projétil circular laranja
                float centerX = 7.5f;
                float centerY = 7.5f;
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                
                if (distance <= 6f)
                {
                    pixels[y * 16 + x] = new Color(1f, 0.5f, 0f, 1f); // Laranja
                }
                else if (distance <= 7f)
                {
                    pixels[y * 16 + x] = new Color(1f, 0.5f, 0f, 1f); // Laranja mais escuro
                }
                else
                {
                    pixels[y * 16 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16f);
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
    /// Detecta colisões
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Projétil acertou o jogador
            PlayerController2D playerController = other.GetComponent<PlayerController2D>();
            if (playerController != null)
            {
                Debug.Log("💥 Projétil acertou o jogador!");
                playerController.Die();
            }
            
            // Destrói o projétil
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            // Projétil acertou o chão ou outro inimigo
            Debug.Log("💥 Projétil acertou obstáculo!");
            Destroy(gameObject);
        }
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
