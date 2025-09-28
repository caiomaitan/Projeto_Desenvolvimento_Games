using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia o sistema de pontuação do jogo
/// Controla pontuação, UI e eventos relacionados
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("Configurações de Pontuação")]
    [SerializeField] private int pointsPerCollectible = 1;
    [SerializeField] private int currentScore = 0;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string scoreFormat = "Pontos: {0}";
    
    [Header("Configurações de UI")]
    [SerializeField] private Vector3 scoreUIPosition = new Vector3(-8f, 4f, 0f);
    [SerializeField] private Color scoreTextColor = Color.white;
    [SerializeField] private int fontSize = 24;
    
    // Singleton para acesso global
    public static ScoreManager Instance { get; private set; }
    
    // Eventos
    public System.Action<int> OnScoreChanged;
    
    /// <summary>
    /// Inicializa o ScoreManager
    /// </summary>
    void Awake()
    {
        // Configura singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("ScoreManager criado e configurado para persistir entre cenas");
        }
        else
        {
            // Se já existe uma instância, destrói esta e usa a existente
            Debug.Log("ScoreManager já existe, destruindo duplicata");
            Destroy(gameObject);
            return;
        }
    }
    
    /// <summary>
    /// Inicializa o sistema de pontuação
    /// </summary>
    void Start()
    {
        // Carrega pontuação salva se existir
        LoadScore();
        
        // Inicializa ou recria a UI
        InitializeScoreUI();
        UpdateScoreDisplay();
        
        Debug.Log($"ScoreManager inicializado com pontuação: {currentScore}");
    }
    
    /// <summary>
    /// Método chamado quando uma nova cena é carregada
    /// </summary>
    void OnEnable()
    {
        // Recria a UI quando uma nova cena é carregada
        if (scoreText == null)
        {
            Debug.Log("UI de pontuação não encontrada, recriando...");
            InitializeScoreUI();
            UpdateScoreDisplay();
        }
    }
    
    /// <summary>
    /// Inicializa a UI de pontuação
    /// </summary>
    private void InitializeScoreUI()
    {
        // Verifica se já existe o texto de pontuação
        if (scoreText != null)
        {
            Debug.Log("UI de pontuação já existe, atualizando...");
            UpdateScoreDisplay();
            return;
        }
        
        // Procura por um Canvas existente
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Cria Canvas se não existir
            GameObject canvasObj = new GameObject("ScoreCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            
            // Adiciona CanvasScaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Adiciona GraphicRaycaster
            canvasObj.AddComponent<GraphicRaycaster>();
            
            Debug.Log("Canvas criado para UI de pontuação");
        }
        
        // Cria GameObject para o texto
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvas.transform, false);
        
        // Adiciona TextMeshProUGUI
        scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        
        // Configura posição
        RectTransform rectTransform = scoreTextObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(100, -50);
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        // Configura texto
        scoreText.text = string.Format(scoreFormat, currentScore);
        scoreText.color = scoreTextColor;
        scoreText.fontSize = fontSize;
        scoreText.alignment = TextAlignmentOptions.Left;
        scoreText.fontStyle = FontStyles.Bold;
        
        Debug.Log("UI de pontuação criada com sucesso!");
    }
    
    /// <summary>
    /// Adiciona pontos à pontuação atual
    /// </summary>
    /// <param name="points">Quantidade de pontos a adicionar</param>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        
        // Dispara evento
        OnScoreChanged?.Invoke(currentScore);
        
        Debug.Log($"Pontuação atualizada! +{points} pontos. Total: {currentScore}");
    }
    
    /// <summary>
    /// Adiciona pontos por coletar um item
    /// </summary>
    public void AddCollectibleScore()
    {
        AddScore(pointsPerCollectible);
    }
    
    /// <summary>
    /// Atualiza a exibição da pontuação na tela
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(scoreFormat, currentScore);
        }
    }
    
    /// <summary>
    /// Reseta a pontuação para zero
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
        OnScoreChanged?.Invoke(currentScore);
        
        // Salva a pontuação zerada
        SaveScore();
        
        Debug.Log("Pontuação resetada para 0 e salva!");
    }
    
    /// <summary>
    /// Obtém a pontuação atual
    /// </summary>
    /// <returns>Pontuação atual</returns>
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    /// <summary>
    /// Define a pontuação atual
    /// </summary>
    /// <param name="score">Nova pontuação</param>
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreDisplay();
        OnScoreChanged?.Invoke(currentScore);
    }
    
    /// <summary>
    /// Salva a pontuação (para persistência entre cenas)
    /// </summary>
    public void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();
        Debug.Log($"Pontuação salva: {currentScore}");
    }
    
    /// <summary>
    /// Carrega a pontuação salva
    /// </summary>
    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("CurrentScore"))
        {
            currentScore = PlayerPrefs.GetInt("CurrentScore");
            UpdateScoreDisplay();
            Debug.Log($"Pontuação carregada: {currentScore}");
        }
    }
    
    /// <summary>
    /// Força a recriação da UI de pontuação
    /// </summary>
    public void RecreateUI()
    {
        Debug.Log("Forçando recriação da UI de pontuação...");
        
        // Destrói a UI existente se houver
        if (scoreText != null)
        {
            Destroy(scoreText.gameObject);
            scoreText = null;
        }
        
        // Recria a UI
        InitializeScoreUI();
        UpdateScoreDisplay();
        
        Debug.Log("UI de pontuação recriada!");
    }
    
    /// <summary>
    /// Método para testar o sistema de pontuação
    /// </summary>
    [ContextMenu("Test Add Score")]
    public void TestAddScore()
    {
        AddCollectibleScore();
    }
    
    /// <summary>
    /// Método para testar reset de pontuação
    /// </summary>
    [ContextMenu("Test Reset Score")]
    public void TestResetScore()
    {
        ResetScore();
    }
}
