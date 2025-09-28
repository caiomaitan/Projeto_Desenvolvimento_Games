using UnityEngine;

/// <summary>
/// Controla o comportamento de itens coletáveis
/// Gerencia animações, efeitos visuais e lógica de coleta
/// </summary>
public class CollectibleItem : MonoBehaviour
{
    [Header("Configurações Visuais")]
    // Campos de animação removidos pois estão desabilitados
    [SerializeField] private Color collectibleColor = Color.yellow;
    
    [Header("Efeitos")]
    [SerializeField] private GameObject collectEffect;
    [SerializeField] private AudioClip collectSound;
    
    // Componentes
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Vector3 startPosition;
    private bool isCollected = false;
    
    /// <summary>
    /// Inicializa o item coletável
    /// </summary>
    void Start()
    {
        InitializeComponents();
        SetupVisuals();
    }
    
    /// <summary>
    /// Obtém e configura os componentes necessários
    /// </summary>
    private void InitializeComponents()
    {
        // Obtém o SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Obtém ou adiciona AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Armazena posição inicial para animação de flutuação
        startPosition = transform.position;
        
        // Configura a tag para detecção de colisão
        gameObject.tag = "Collectible";
        
        // Adiciona Collider2D se não existir
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true; // Permite que o jogador passe através
        }
    }
    
    /// <summary>
    /// Configura a aparência visual do item
    /// </summary>
    private void SetupVisuals()
    {
        // Define cor do item
        if (spriteRenderer != null)
        {
            spriteRenderer.color = collectibleColor;
            
            // Cria um sprite simples se não houver um atribuído
            if (spriteRenderer.sprite == null)
            {
                CreateSimpleSprite();
            }
        }
    }
    
    /// <summary>
    /// Cria um sprite simples para o item coletável
    /// </summary>
    private void CreateSimpleSprite()
    {
        // Cria uma textura simples
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Desenha um círculo simples
        Vector2 center = new Vector2(16, 16);
        float radius = 12f;
        
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * 32 + x] = distance <= radius ? collectibleColor : Color.clear;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Cria sprite a partir da textura
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = sprite;
    }
    
    /// <summary>
    /// Atualiza animações do item a cada frame
    /// </summary>
    void Update()
    {
        // Animações desabilitadas - itens ficam parados
        // if (!isCollected)
        // {
        //     AnimateItem();
        // }
    }
    
    /// <summary>
    /// Aplica animações de rotação e flutuação (DESABILITADO)
    /// </summary>
    private void AnimateItem()
    {
        // Rotação contínua - DESABILITADA
        // transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Animação de flutuação (bob) - DESABILITADA
        // float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    /// <summary>
    /// Processa a coleta do item pelo jogador
    /// </summary>
    public void Collect()
    {
        if (isCollected) return;
        
        isCollected = true;
        
        // Adiciona pontos ao sistema de pontuação
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddCollectibleScore();
        }
        
        // Reproduz som de coleta
        PlayCollectSound();
        
        // Cria efeito visual
        CreateCollectEffect();
        
        // Destrói o item
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Reproduz o som de coleta
    /// </summary>
    private void PlayCollectSound()
    {
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
    
    /// <summary>
    /// Cria efeito visual de coleta
    /// </summary>
    private void CreateCollectEffect()
    {
        if (collectEffect != null)
        {
            GameObject effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            
            // Destrói o efeito após 2 segundos
            Destroy(effect, 2f);
        }
    }
    
    /// <summary>
    /// Detecta quando o jogador toca no item
    /// </summary>
    /// <param name="other">Collider que tocou no item</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se foi o jogador que tocou
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
}