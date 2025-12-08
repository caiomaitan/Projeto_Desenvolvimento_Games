using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Gerencia o sistema de vidas do jogador
/// Singleton que persiste entre cenas
/// </summary>
public class LivesManager : MonoBehaviour
{
    [Header("Configurações de Vidas")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLives = 3;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private string livesFormat = "Vidas: {0}";
    
    [Header("Configurações de UI")]
    [SerializeField] private Color livesTextColor = Color.red;
    [SerializeField] private int fontSize = 24;
    
    // Singleton para acesso global
    public static LivesManager Instance { get; private set; }
    
    // Eventos
    public System.Action<int> OnLivesChanged;
    public System.Action OnGameOver;
    
    /// <summary>
    /// Inicializa o LivesManager
    /// </summary>
    void Awake()
    {
        // Configura singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("❤️ LivesManager criado e configurado para persistir entre cenas");
        }
        else
        {
            // Se já existe uma instância, destrói esta e usa a existente
            Debug.Log("❤️ LivesManager já existe, destruindo duplicata");
            Destroy(gameObject);
            return;
        }
    }
    
    /// <summary>
    /// Inicializa o sistema de vidas
    /// </summary>
    void Start()
    {
        // Inicializa ou recria a UI
        InitializeLivesUI();
        UpdateLivesDisplay();
        
        Debug.Log($"❤️ LivesManager inicializado com {currentLives} vidas");
    }
    
    /// <summary>
    /// Método chamado quando uma nova cena é carregada
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// Chamado quando uma nova cena é carregada
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Recria a UI quando uma nova cena é carregada (exceto GameOver)
        if (scene.name != "GameOver")
        {
            Debug.Log($"❤️ Nova cena carregada: {scene.name}, recriando UI de vidas...");
            livesText = null; // Força recriação
            InitializeLivesUI();
            UpdateLivesDisplay();
        }
    }
    
    /// <summary>
    /// Inicializa a UI de vidas
    /// </summary>
    private void InitializeLivesUI()
    {
        // Verifica se já existe o texto de vidas
        if (livesText != null)
        {
            Debug.Log("❤️ UI de vidas já existe, atualizando...");
            UpdateLivesDisplay();
            return;
        }
        
        // Procura por um Canvas existente
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Cria Canvas se não existir
            GameObject canvasObj = new GameObject("LivesCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            
            // Adiciona CanvasScaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Adiciona GraphicRaycaster
            canvasObj.AddComponent<GraphicRaycaster>();
            
            Debug.Log("❤️ Canvas criado para UI de vidas");
        }
        
        // Cria GameObject para o texto
        GameObject livesTextObj = new GameObject("LivesText");
        livesTextObj.transform.SetParent(canvas.transform, false);
        
        // Adiciona TextMeshProUGUI
        livesText = livesTextObj.AddComponent<TextMeshProUGUI>();
        
        // Configura posição (canto superior direito)
        RectTransform rectTransform = livesTextObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-100, -50);
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        // Configura texto
        livesText.text = string.Format(livesFormat, currentLives);
        livesText.color = livesTextColor;
        livesText.fontSize = fontSize;
        livesText.alignment = TextAlignmentOptions.Right;
        livesText.fontStyle = FontStyles.Bold;
        
        Debug.Log("❤️ UI de vidas criada com sucesso!");
    }
    
    /// <summary>
    /// Remove uma vida do jogador
    /// </summary>
    /// <returns>True se ainda tem vidas, False se acabou</returns>
    public bool LoseLife()
    {
        currentLives--;
        UpdateLivesDisplay();
        
        // Dispara evento
        OnLivesChanged?.Invoke(currentLives);
        
        Debug.Log($"💔 Vida perdida! Vidas restantes: {currentLives}");
        
        if (currentLives <= 0)
        {
            Debug.Log("💀💀💀 GAME OVER - Todas as vidas perdidas! 💀💀💀");
            OnGameOver?.Invoke();
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Adiciona uma vida ao jogador
    /// </summary>
    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesDisplay();
            OnLivesChanged?.Invoke(currentLives);
            Debug.Log($"💚 Vida ganha! Vidas: {currentLives}");
        }
    }
    
    /// <summary>
    /// Atualiza a exibição das vidas na tela
    /// </summary>
    private void UpdateLivesDisplay()
    {
        if (livesText != null)
        {
            livesText.text = string.Format(livesFormat, currentLives);
            
            // Muda cor baseado nas vidas restantes
            if (currentLives == 1)
            {
                livesText.color = new Color(1f, 0.3f, 0.3f, 1f); // Vermelho intenso
            }
            else if (currentLives == 2)
            {
                livesText.color = new Color(1f, 0.6f, 0f, 1f); // Laranja
            }
            else
            {
                livesText.color = livesTextColor; // Verde/normal
            }
        }
    }
    
    /// <summary>
    /// Reseta as vidas para o máximo
    /// </summary>
    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateLivesDisplay();
        OnLivesChanged?.Invoke(currentLives);
        Debug.Log($"❤️ Vidas resetadas para {currentLives}!");
    }
    
    /// <summary>
    /// Obtém o número atual de vidas
    /// </summary>
    public int GetCurrentLives()
    {
        return currentLives;
    }
    
    /// <summary>
    /// Obtém o número máximo de vidas
    /// </summary>
    public int GetMaxLives()
    {
        return maxLives;
    }
    
    /// <summary>
    /// Define o número de vidas
    /// </summary>
    public void SetLives(int lives)
    {
        currentLives = Mathf.Clamp(lives, 0, maxLives);
        UpdateLivesDisplay();
        OnLivesChanged?.Invoke(currentLives);
    }
    
    /// <summary>
    /// Verifica se o jogador ainda tem vidas
    /// </summary>
    public bool HasLives()
    {
        return currentLives > 0;
    }
    
    /// <summary>
    /// Força a recriação da UI de vidas
    /// </summary>
    public void RecreateUI()
    {
        Debug.Log("❤️ Forçando recriação da UI de vidas...");
        
        // Destrói a UI existente se houver
        if (livesText != null)
        {
            Destroy(livesText.gameObject);
            livesText = null;
        }
        
        // Recria a UI
        InitializeLivesUI();
        UpdateLivesDisplay();
        
        Debug.Log("❤️ UI de vidas recriada!");
    }
    
    /// <summary>
    /// Carrega a cena de Game Over
    /// </summary>
    public void LoadGameOverScene()
    {
        Debug.Log("💀 Carregando cena de Game Over...");
        SceneManager.LoadScene("GameOver");
    }
}


