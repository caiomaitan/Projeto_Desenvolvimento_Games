using UnityEngine;

/// <summary>
/// Gera plataformas para um jogo 2D estilo Mario
/// Cria chão, plataformas flutuantes e obstáculos
/// </summary>
public class PlatformGenerator : MonoBehaviour
{
    [Header("Configurações das Plataformas")]
    [SerializeField] private int platformCount = 8;
    [SerializeField] private float platformSpacing = 4f;
    [SerializeField] private float platformHeight = 0.5f;
    [SerializeField] private float platformWidth = 3f;
    [SerializeField] private Color platformColor = new Color(0.6f, 0.4f, 0.2f, 1f);
    
    [Header("Configurações do Chão")]
    [SerializeField] private float groundWidth = 20f;
    [SerializeField] private float groundHeight = 1f;
    [SerializeField] private Color groundColor = new Color(0.4f, 0.6f, 0.3f, 1f);
    
    [Header("Configurações dos Coletáveis")]
    [SerializeField] private int collectibleCount = 10;
    [SerializeField] private float collectibleSpacing = 2f;
    
    [Header("Referências")]
    [SerializeField] private Transform platformsParent;
    [SerializeField] private Transform collectiblesParent;
    
    /// <summary>
    /// Inicializa o gerador de plataformas
    /// </summary>
    void Start()
    {
        GeneratePlatforms();
    }
    
    /// <summary>
    /// Gera todas as plataformas e elementos do jogo
    /// </summary>
    [ContextMenu("Generate Platforms")]
    public void GeneratePlatforms()
    {
        // Cria objetos pais se não existirem
        if (platformsParent == null)
        {
            GameObject parentObj = new GameObject("Platforms");
            platformsParent = parentObj.transform;
        }
        
        if (collectiblesParent == null)
        {
            GameObject parentObj = new GameObject("Collectibles");
            collectiblesParent = parentObj.transform;
        }
        
        // Limpa elementos existentes
        ClearExistingElements();
        
        // Gera o chão principal
        GenerateGround();
        
        // Gera plataformas flutuantes
        GenerateFloatingPlatforms();
        
        // Gera itens coletáveis
        GenerateCollectibles();
        
        Debug.Log("Plataformas 2D estilo Mario geradas!");
    }
    
    /// <summary>
    /// Limpa elementos existentes
    /// </summary>
    private void ClearExistingElements()
    {
        // Limpa plataformas
        foreach (Transform child in platformsParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        
        // Limpa coletáveis
        foreach (Transform child in collectiblesParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
    }
    
    /// <summary>
    /// Gera o chão principal
    /// </summary>
    private void GenerateGround()
    {
        GameObject ground = new GameObject("Ground");
        ground.transform.SetParent(platformsParent);
        
        // Posiciona o chão
        ground.transform.position = new Vector3(0, -5f, 0);
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = ground.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreatePlatformSprite(groundWidth, groundHeight);
        spriteRenderer.color = groundColor;
        spriteRenderer.sortingOrder = 0;
        
        // Adiciona Collider2D
        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(groundWidth, groundHeight);
        
        // Adiciona tag
        ground.tag = "Ground";
    }
    
    /// <summary>
    /// Gera plataformas flutuantes
    /// </summary>
    private void GenerateFloatingPlatforms()
    {
        for (int i = 0; i < platformCount; i++)
        {
            CreateFloatingPlatform(i);
        }
    }
    
    /// <summary>
    /// Cria uma plataforma flutuante individual
    /// </summary>
    /// <param name="index">Índice da plataforma</param>
    private void CreateFloatingPlatform(int index)
    {
        GameObject platform = new GameObject($"Platform_{index + 1}");
        platform.transform.SetParent(platformsParent);
        
        // Calcula posição da plataforma
        Vector3 position = GetPlatformPosition(index);
        platform.transform.position = position;
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = platform.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreatePlatformSprite(platformWidth, platformHeight);
        spriteRenderer.color = platformColor;
        spriteRenderer.sortingOrder = 0;
        
        // Adiciona Collider2D
        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(platformWidth, platformHeight);
        
        // Adiciona tag
        platform.tag = "Ground";
    }
    
    /// <summary>
    /// Calcula posição para uma plataforma
    /// </summary>
    /// <param name="index">Índice da plataforma</param>
    /// <returns>Posição calculada</returns>
    private Vector3 GetPlatformPosition(int index)
    {
        float x = (index - platformCount / 2f) * platformSpacing;
        float y = Random.Range(-2f, 3f); // Altura aleatória
        
        return new Vector3(x, y, 0);
    }
    
    /// <summary>
    /// Gera itens coletáveis nas plataformas
    /// </summary>
    private void GenerateCollectibles()
    {
        for (int i = 0; i < collectibleCount; i++)
        {
            CreateCollectible(i);
        }
    }
    
    /// <summary>
    /// Cria um item coletável individual
    /// </summary>
    /// <param name="index">Índice do item</param>
    private void CreateCollectible(int index)
    {
        GameObject collectible = new GameObject($"Collectible_{index + 1}");
        collectible.transform.SetParent(collectiblesParent);
        
        // Calcula posição do coletável
        Vector3 position = GetCollectiblePosition(index);
        collectible.transform.position = position;
        
        // Adiciona componentes necessários
        collectible.AddComponent<CollectibleItem>();
        collectible.tag = "Collectible";
        
        // Adiciona efeito visual
        SpriteRenderer spriteRenderer = collectible.AddComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
        spriteRenderer.sortingOrder = 1;
        
        // Adiciona Collider2D
        CircleCollider2D collider = collectible.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.3f;
    }
    
    /// <summary>
    /// Calcula posição para um item coletável
    /// </summary>
    /// <param name="index">Índice do item</param>
    /// <returns>Posição calculada</returns>
    private Vector3 GetCollectiblePosition(int index)
    {
        // Distribui os itens nas plataformas
        float x = (index - collectibleCount / 2f) * collectibleSpacing;
        float y = Random.Range(0f, 4f); // Altura aleatória
        
        return new Vector3(x, y, 0);
    }
    
    /// <summary>
    /// Cria um sprite para plataforma
    /// </summary>
    /// <param name="width">Largura</param>
    /// <param name="height">Altura</param>
    /// <returns>Sprite criado</returns>
    private Sprite CreatePlatformSprite(float width, float height)
    {
        int textureWidth = Mathf.RoundToInt(width * 32);
        int textureHeight = Mathf.RoundToInt(height * 32);
        
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        Color[] pixels = new Color[textureWidth * textureHeight];
        
        // Preenche com cor branca
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        // Adiciona bordas
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                if (x == 0 || x == textureWidth - 1 || y == 0 || y == textureHeight - 1)
                {
                    pixels[y * textureWidth + x] = new Color(0.8f, 0.8f, 0.8f, 1f);
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f), 32f);
    }
    
    /// <summary>
    /// Regenera todas as plataformas
    /// </summary>
    [ContextMenu("Regenerate Platforms")]
    public void RegeneratePlatforms()
    {
        GeneratePlatforms();
    }
}