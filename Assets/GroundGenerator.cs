using UnityEngine;

/// <summary>
/// Gera um chão visual para o jogo 2D
/// Cria tiles de chão com textura e padrões
/// </summary>
public class GroundGenerator : MonoBehaviour
{
    [Header("Configurações do Chão")]
    [SerializeField] private int groundWidth = 20;
    [SerializeField] private int groundHeight = 15;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private Color groundColor = new Color(0.4f, 0.6f, 0.3f, 1f);
    [SerializeField] private Color borderColor = new Color(0.3f, 0.5f, 0.2f, 1f);
    
    [Header("Elementos Visuais")]
    [SerializeField] private bool createBorder = true;
    [SerializeField] private bool addGrassDetails = true;
    [SerializeField] private int grassDetailCount = 30;
    
    [Header("Referências")]
    [SerializeField] private Transform groundParent;
    
    /// <summary>
    /// Inicializa o gerador de chão
    /// </summary>
    void Start()
    {
        GenerateGround();
    }
    
    /// <summary>
    /// Gera o chão completo com tiles e detalhes
    /// </summary>
    public void GenerateGround()
    {
        // Cria o objeto pai se não existir
        if (groundParent == null)
        {
            GameObject parentObj = new GameObject("Ground");
            groundParent = parentObj.transform;
        }
        
        // Gera os tiles do chão
        GenerateGroundTiles();
        
        // Adiciona bordas se habilitado
        if (createBorder)
        {
            GenerateBorders();
        }
        
        // Adiciona detalhes de grama se habilitado
        if (addGrassDetails)
        {
            GenerateGrassDetails();
        }
        
        Debug.Log($"Chão 2D gerado: {groundWidth}x{groundHeight} tiles");
    }
    
    /// <summary>
    /// Gera os tiles principais do chão
    /// </summary>
    private void GenerateGroundTiles()
    {
        for (int x = 0; x < groundWidth; x++)
        {
            for (int y = 0; y < groundHeight; y++)
            {
                CreateGroundTile(x, y);
            }
        }
    }
    
    /// <summary>
    /// Cria um tile individual do chão
    /// </summary>
    /// <param name="x">Posição X do tile</param>
    /// <param name="y">Posição Y do tile</param>
    private void CreateGroundTile(int x, int y)
    {
        // Cria o GameObject do tile
        GameObject tile = new GameObject($"GroundTile_{x}_{y}");
        tile.transform.SetParent(groundParent);
        
        // Posiciona o tile
        Vector3 position = new Vector3(
            (x - groundWidth / 2f) * tileSize,
            (y - groundHeight / 2f) * tileSize,
            0f
        );
        tile.transform.position = position;
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
        
        // Cria sprite do tile
        Sprite tileSprite = CreateTileSprite();
        spriteRenderer.sprite = tileSprite;
        
        // Define cor do tile (com variação sutil)
        Color tileColor = groundColor;
        float variation = Random.Range(-0.1f, 0.1f);
        tileColor.r = Mathf.Clamp01(tileColor.r + variation);
        tileColor.g = Mathf.Clamp01(tileColor.g + variation);
        tileColor.b = Mathf.Clamp01(tileColor.b + variation);
        spriteRenderer.color = tileColor;
        
        // Define ordem de renderização (chão fica atrás)
        spriteRenderer.sortingOrder = -10;
    }
    
    /// <summary>
    /// Cria um sprite simples para o tile
    /// </summary>
    /// <returns>Sprite do tile</returns>
    private Sprite CreateTileSprite()
    {
        // Cria textura do tile
        int textureSize = 32;
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] pixels = new Color[textureSize * textureSize];
        
        // Preenche com cor base
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        // Adiciona padrão sutil
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                // Cria padrão de quadrados
                if ((x / 4 + y / 4) % 2 == 0)
                {
                    pixels[y * textureSize + x] = new Color(0.9f, 0.9f, 0.9f, 1f);
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Cria sprite
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f),
            tileSize * 32f // Pixels per unit
        );
        
        return sprite;
    }
    
    /// <summary>
    /// Gera bordas ao redor do chão
    /// </summary>
    private void GenerateBorders()
    {
        // Bordas superior e inferior
        for (int x = -1; x <= groundWidth; x++)
        {
            CreateBorderTile(x, -1); // Borda inferior
            CreateBorderTile(x, groundHeight); // Borda superior
        }
        
        // Bordas laterais
        for (int y = 0; y < groundHeight; y++)
        {
            CreateBorderTile(-1, y); // Borda esquerda
            CreateBorderTile(groundWidth, y); // Borda direita
        }
    }
    
    /// <summary>
    /// Cria um tile de borda
    /// </summary>
    /// <param name="x">Posição X</param>
    /// <param name="y">Posição Y</param>
    private void CreateBorderTile(int x, int y)
    {
        GameObject borderTile = new GameObject($"BorderTile_{x}_{y}");
        borderTile.transform.SetParent(groundParent);
        
        Vector3 position = new Vector3(
            (x - groundWidth / 2f) * tileSize,
            (y - groundHeight / 2f) * tileSize,
            0f
        );
        borderTile.transform.position = position;
        
        SpriteRenderer spriteRenderer = borderTile.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateTileSprite();
        spriteRenderer.color = borderColor;
        spriteRenderer.sortingOrder = -9;
    }
    
    /// <summary>
    /// Gera detalhes de grama espalhados pelo chão
    /// </summary>
    private void GenerateGrassDetails()
    {
        for (int i = 0; i < grassDetailCount; i++)
        {
            CreateGrassDetail();
        }
    }
    
    /// <summary>
    /// Cria um detalhe de grama individual
    /// </summary>
    private void CreateGrassDetail()
    {
        GameObject grass = new GameObject("GrassDetail");
        grass.transform.SetParent(groundParent);
        
        // Posição aleatória dentro da área do chão
        Vector3 position = new Vector3(
            Random.Range(-groundWidth / 2f + 1, groundWidth / 2f - 1) * tileSize,
            Random.Range(-groundHeight / 2f + 1, groundHeight / 2f - 1) * tileSize,
            -0.1f
        );
        grass.transform.position = position;
        
        SpriteRenderer spriteRenderer = grass.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateGrassSprite();
        spriteRenderer.color = new Color(0.2f, 0.7f, 0.2f, 0.8f);
        spriteRenderer.sortingOrder = -5;
        
        // Adiciona animação sutil
        grass.AddComponent<GrassAnimation>();
    }
    
    /// <summary>
    /// Cria sprite simples para grama
    /// </summary>
    /// <returns>Sprite da grama</returns>
    private Sprite CreateGrassSprite()
    {
        Texture2D texture = new Texture2D(8, 16);
        Color[] pixels = new Color[8 * 16];
        
        // Desenha formato de grama simples
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                if (x >= 3 && x <= 4 && y >= 4) // Haste central
                {
                    pixels[y * 8 + x] = Color.white;
                }
                else if ((x == 2 || x == 5) && y >= 8) // Hastes laterais
                {
                    pixels[y * 8 + x] = Color.white;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 8, 16), new Vector2(0.5f, 0f), 32f);
    }
    
    /// <summary>
    /// Regenera o chão (útil para mudanças em runtime)
    /// </summary>
    [ContextMenu("Regenerate Ground")]
    public void RegenerateGround()
    {
        // Limpa chão existente
        if (groundParent != null)
        {
            DestroyImmediate(groundParent.gameObject);
        }
        
        // Gera novo chão
        GenerateGround();
    }
}