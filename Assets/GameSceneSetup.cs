using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Configura automaticamente a cena do jogo
/// Adiciona chão, itens coletáveis e outros elementos
/// </summary>
public class GameSceneSetup : MonoBehaviour
{
    [Header("Configurações da Cena")]
    [SerializeField] private bool autoSetupOnStart = true;
    [SerializeField] private bool createGround = true;
    [SerializeField] private bool createCollectibles = true;
    [SerializeField] private bool createUI = true;
    
    [Header("Configurações dos Itens")]
    [SerializeField] private int collectibleCount = 5;
    [SerializeField] private float collectibleSpacing = 3f;
    
    [Header("Referências")]
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private Transform collectiblesParent;
    
    /// <summary>
    /// Inicializa a configuração da cena
    /// </summary>
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupGameScene();
        }
    }
    
    /// <summary>
    /// Configura todos os elementos da cena do jogo
    /// </summary>
    [ContextMenu("Setup Game Scene")]
    public void SetupGameScene()
    {
        Debug.Log("Configurando cena do jogo...");
        
        if (createGround)
        {
            SetupGround();
        }
        
        if (createCollectibles)
        {
            SetupCollectibles();
        }
        
        if (createUI)
        {
            SetupUI();
        }
        
        Debug.Log("Cena do jogo configurada com sucesso!");
    }
    
    /// <summary>
    /// Configura o chão da cena
    /// </summary>
    private void SetupGround()
    {
        // Verifica se já existe um PlatformGenerator
        PlatformGenerator existingGenerator = FindObjectOfType<PlatformGenerator>();
        if (existingGenerator == null)
        {
            // Cria o gerador de plataformas
            GameObject platformGeneratorObj = new GameObject("PlatformGenerator");
            PlatformGenerator platformGenerator = platformGeneratorObj.AddComponent<PlatformGenerator>();
            
            Debug.Log("Plataformas 2D estilo Mario criadas automaticamente");
        }
        else
        {
            Debug.Log("Gerador de plataformas já existe na cena");
        }
    }
    
    /// <summary>
    /// Configura os itens coletáveis
    /// </summary>
    private void SetupCollectibles()
    {
        // Cria objeto pai para os coletáveis
        if (collectiblesParent == null)
        {
            GameObject parentObj = new GameObject("Collectibles");
            collectiblesParent = parentObj.transform;
        }
        
        // Limpa coletáveis existentes
        foreach (Transform child in collectiblesParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        
        // Cria novos coletáveis
        for (int i = 0; i < collectibleCount; i++)
        {
            CreateCollectible(i);
        }
        
        Debug.Log($"Criados {collectibleCount} itens coletáveis");
    }
    
    /// <summary>
    /// Cria um item coletável individual
    /// </summary>
    /// <param name="index">Índice do item</param>
    private void CreateCollectible(int index)
    {
        GameObject collectible;
        
        if (collectiblePrefab != null)
        {
            // Usa prefab se disponível
            collectible = Instantiate(collectiblePrefab, collectiblesParent);
        }
        else
        {
            // Cria item coletável básico
            collectible = new GameObject($"Collectible_{index + 1}");
            collectible.transform.SetParent(collectiblesParent);
            
            // Adiciona componentes necessários
            collectible.AddComponent<CollectibleItem>();
            collectible.tag = "Collectible";
        }
        
        // Posiciona o item
        Vector3 position = GetCollectiblePosition(index);
        collectible.transform.position = position;
        
        // Adiciona efeito visual se não tiver
        if (collectible.GetComponent<SpriteRenderer>() == null)
        {
            SpriteRenderer spriteRenderer = collectible.AddComponent<SpriteRenderer>();
            spriteRenderer.color = Color.yellow;
            spriteRenderer.sortingOrder = 1;
        }
    }
    
    /// <summary>
    /// Calcula posição para um item coletável
    /// </summary>
    /// <param name="index">Índice do item</param>
    /// <returns>Posição calculada</returns>
    private Vector3 GetCollectiblePosition(int index)
    {
        // Distribui os itens em um padrão circular
        float angle = (index * 2f * Mathf.PI) / collectibleCount;
        float radius = collectibleSpacing * 2f;
        
        Vector3 position = new Vector3(
            Mathf.Cos(angle) * radius,
            Mathf.Sin(angle) * radius,
            0f
        );
        
        // Adiciona variação aleatória
        position += new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0f
        );
        
        return position;
    }
    
    /// <summary>
    /// Configura a UI da cena
    /// </summary>
    private void SetupUI()
    {
        // Verifica se já existe UI
        Canvas existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas == null)
        {
            CreateGameUI();
        }
        else
        {
            Debug.Log("UI já existe na cena");
        }
    }
    
    /// <summary>
    /// Cria a UI básica do jogo
    /// </summary>
    private void CreateGameUI()
    {
        // Cria Canvas
        GameObject canvasObj = new GameObject("GameCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Cria texto de pontuação
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(canvasObj.transform);
        
        UnityEngine.UI.Text scoreText = scoreObj.AddComponent<UnityEngine.UI.Text>();
        scoreText.text = "Pontuação: 0";
        scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        scoreText.fontSize = 24;
        scoreText.color = Color.white;
        
        // Posiciona no canto superior esquerdo
        RectTransform scoreRect = scoreText.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(10, -10);
        scoreRect.sizeDelta = new Vector2(200, 30);
        
        // Cria texto de itens
        GameObject itemsObj = new GameObject("ItemsText");
        itemsObj.transform.SetParent(canvasObj.transform);
        
        UnityEngine.UI.Text itemsText = itemsObj.AddComponent<UnityEngine.UI.Text>();
        itemsText.text = "Itens: 0/5";
        itemsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        itemsText.fontSize = 20;
        itemsText.color = Color.white;
        
        // Posiciona abaixo da pontuação
        RectTransform itemsRect = itemsText.GetComponent<RectTransform>();
        itemsRect.anchorMin = new Vector2(0, 1);
        itemsRect.anchorMax = new Vector2(0, 1);
        itemsRect.anchoredPosition = new Vector2(10, -40);
        itemsRect.sizeDelta = new Vector2(150, 25);
        
        Debug.Log("UI básica criada");
    }
    
    /// <summary>
    /// Limpa todos os elementos criados automaticamente
    /// </summary>
    [ContextMenu("Clear Auto Setup")]
    public void ClearAutoSetup()
    {
        // Remove gerador de chão
        GroundGenerator groundGenerator = FindObjectOfType<GroundGenerator>();
        if (groundGenerator != null)
        {
            if (Application.isPlaying)
                Destroy(groundGenerator.gameObject);
            else
                DestroyImmediate(groundGenerator.gameObject);
        }
        
        // Remove coletáveis
        if (collectiblesParent != null)
        {
            if (Application.isPlaying)
                Destroy(collectiblesParent.gameObject);
            else
                DestroyImmediate(collectiblesParent.gameObject);
        }
        
        Debug.Log("Elementos automáticos removidos");
    }
}