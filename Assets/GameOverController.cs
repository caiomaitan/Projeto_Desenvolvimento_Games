using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador da tela de Game Over
/// Mostra mensagem de jogo completo e opções
/// </summary>
public class GameOverController : MonoBehaviour
{
    [Header("Configurações da UI")]
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text messageText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    
    [Header("Configurações da Mensagem")]
    [SerializeField] private string gameCompleteMessage = "PARABÉNS!";
    [SerializeField] private string gameCompleteSubMessage = "Você zerou o jogo!";
    
    /// <summary>
    /// Inicializa a tela de Game Over
    /// </summary>
    void Start()
    {
        SetupGameOverScreen();
        SetupButtons();
    }
    
    /// <summary>
    /// Configura a tela de Game Over
    /// </summary>
    private void SetupGameOverScreen()
    {
        Debug.Log("🎮 Configurando tela de Game Over...");
        
        // Cria UI se não existir
        if (gameOverText == null)
        {
            CreateGameOverUI();
        }
        
        // Define as mensagens
        if (gameOverText != null)
        {
            gameOverText.text = gameCompleteMessage;
        }
        
        if (messageText != null)
        {
            messageText.text = gameCompleteSubMessage;
        }
        
        Debug.Log("✅ Tela de Game Over configurada!");
    }
    
    /// <summary>
    /// Cria a UI do Game Over se não existir
    /// </summary>
    private void CreateGameOverUI()
    {
        // Cria Canvas se não existir
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Cria texto principal
        GameObject gameOverTextObj = new GameObject("GameOverText");
        gameOverTextObj.transform.SetParent(canvas.transform);
        gameOverText = gameOverTextObj.AddComponent<Text>();
        gameOverText.text = gameCompleteMessage;
        gameOverText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        gameOverText.fontSize = 48;
        gameOverText.color = Color.green;
        gameOverText.alignment = TextAnchor.MiddleCenter;
        
        // Posiciona o texto
        RectTransform gameOverRect = gameOverText.GetComponent<RectTransform>();
        gameOverRect.anchorMin = new Vector2(0.5f, 0.7f);
        gameOverRect.anchorMax = new Vector2(0.5f, 0.7f);
        gameOverRect.sizeDelta = new Vector2(400, 100);
        gameOverRect.anchoredPosition = Vector2.zero;
        
        // Cria texto da mensagem
        GameObject messageTextObj = new GameObject("MessageText");
        messageTextObj.transform.SetParent(canvas.transform);
        messageText = messageTextObj.AddComponent<Text>();
        messageText.text = gameCompleteSubMessage;
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        messageText.fontSize = 24;
        messageText.color = Color.white;
        messageText.alignment = TextAnchor.MiddleCenter;
        
        // Posiciona o texto da mensagem
        RectTransform messageRect = messageText.GetComponent<RectTransform>();
        messageRect.anchorMin = new Vector2(0.5f, 0.5f);
        messageRect.anchorMax = new Vector2(0.5f, 0.5f);
        messageRect.sizeDelta = new Vector2(400, 50);
        messageRect.anchoredPosition = Vector2.zero;
        
        // Cria botão "Jogar Novamente"
        GameObject playAgainButtonObj = new GameObject("PlayAgainButton");
        playAgainButtonObj.transform.SetParent(canvas.transform);
        playAgainButton = playAgainButtonObj.AddComponent<Button>();
        Image playAgainImage = playAgainButtonObj.AddComponent<Image>();
        playAgainImage.color = new Color(0.2f, 0.8f, 0.2f, 1f); // Verde
        
        // Posiciona o botão "Jogar Novamente"
        RectTransform playAgainRect = playAgainButton.GetComponent<RectTransform>();
        playAgainRect.anchorMin = new Vector2(0.5f, 0.3f);
        playAgainRect.anchorMax = new Vector2(0.5f, 0.3f);
        playAgainRect.sizeDelta = new Vector2(200, 50);
        playAgainRect.anchoredPosition = Vector2.zero;
        
        // Adiciona texto ao botão "Jogar Novamente"
        GameObject playAgainTextObj = new GameObject("Text");
        playAgainTextObj.transform.SetParent(playAgainButtonObj.transform);
        Text playAgainText = playAgainTextObj.AddComponent<Text>();
        playAgainText.text = "Jogar Novamente";
        playAgainText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        playAgainText.fontSize = 18;
        playAgainText.color = Color.white;
        playAgainText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform playAgainTextRect = playAgainText.GetComponent<RectTransform>();
        playAgainTextRect.anchorMin = Vector2.zero;
        playAgainTextRect.anchorMax = Vector2.one;
        playAgainTextRect.offsetMin = Vector2.zero;
        playAgainTextRect.offsetMax = Vector2.zero;
        
        // Conecta o evento do botão "Jogar Novamente"
        playAgainButton.onClick.AddListener(PlayAgain);
        Debug.Log("✅ Evento do botão 'Jogar Novamente' conectado automaticamente!");
        Debug.Log($"✅ Botão 'Jogar Novamente' - Interactable: {playAgainButton.interactable}");
        Debug.Log($"✅ Botão 'Jogar Novamente' - Eventos conectados: {playAgainButton.onClick.GetPersistentEventCount()}");
        
        // Cria botão "Menu Principal"
        GameObject mainMenuButtonObj = new GameObject("MainMenuButton");
        mainMenuButtonObj.transform.SetParent(canvas.transform);
        mainMenuButton = mainMenuButtonObj.AddComponent<Button>();
        Image mainMenuImage = mainMenuButtonObj.AddComponent<Image>();
        mainMenuImage.color = new Color(0.8f, 0.2f, 0.2f, 1f); // Vermelho
        
        // Posiciona o botão "Menu Principal"
        RectTransform mainMenuRect = mainMenuButton.GetComponent<RectTransform>();
        mainMenuRect.anchorMin = new Vector2(0.5f, 0.2f);
        mainMenuRect.anchorMax = new Vector2(0.5f, 0.2f);
        mainMenuRect.sizeDelta = new Vector2(200, 50);
        mainMenuRect.anchoredPosition = Vector2.zero;
        
        // Adiciona texto ao botão "Menu Principal"
        GameObject mainMenuTextObj = new GameObject("Text");
        mainMenuTextObj.transform.SetParent(mainMenuButtonObj.transform);
        Text mainMenuText = mainMenuTextObj.AddComponent<Text>();
        mainMenuText.text = "Menu Principal";
        mainMenuText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        mainMenuText.fontSize = 18;
        mainMenuText.color = Color.white;
        mainMenuText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform mainMenuTextRect = mainMenuText.GetComponent<RectTransform>();
        mainMenuTextRect.anchorMin = Vector2.zero;
        mainMenuTextRect.anchorMax = Vector2.one;
        mainMenuTextRect.offsetMin = Vector2.zero;
        mainMenuTextRect.offsetMax = Vector2.zero;
        
        // Conecta o evento do botão "Menu Principal"
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        Debug.Log("✅ Evento do botão 'Menu Principal' conectado automaticamente!");
        Debug.Log($"✅ Botão 'Menu Principal' - Interactable: {mainMenuButton.interactable}");
        Debug.Log($"✅ Botão 'Menu Principal' - Eventos conectados: {mainMenuButton.onClick.GetPersistentEventCount()}");
        
        Debug.Log("✅ UI do Game Over criada automaticamente!");
    }
    
    /// <summary>
    /// Configura os botões
    /// </summary>
    private void SetupButtons()
    {
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(PlayAgain);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
    }
    
    /// <summary>
    /// Reinicia o jogo
    /// </summary>
    public void PlayAgain()
    {
        Debug.Log("🔄🔄🔄 BOTÃO 'JOGAR NOVAMENTE' CLICADO! 🔄🔄🔄");
        Debug.Log("🔄 Tentando carregar cena 'Game'...");
        
        try
        {
            SceneManager.LoadScene("Game");
            Debug.Log("✅ Cena 'Game' carregada com sucesso!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ ERRO ao carregar cena 'Game': {e.Message}");
            Debug.LogError("❌ Verifique se a cena 'Game' existe e está no Build Settings!");
        }
    }
    
    /// <summary>
    /// Vai para o menu principal
    /// </summary>
    public void GoToMainMenu()
    {
        Debug.Log("🏠🏠🏠 BOTÃO 'MENU PRINCIPAL' CLICADO! 🏠🏠🏠");
        Debug.Log("🏠 Tentando carregar cena 'MainMenu'...");
        
        try
        {
            SceneManager.LoadScene("MainMenu");
            Debug.Log("✅ Cena 'MainMenu' carregada com sucesso!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ ERRO ao carregar cena 'MainMenu': {e.Message}");
            Debug.LogError("❌ Verifique se a cena 'MainMenu' existe e está no Build Settings!");
        }
    }
}