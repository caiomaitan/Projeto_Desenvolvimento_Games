using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla a interface do menu principal
/// Gerencia botões e transições entre cenas
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("Elementos da UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Text titleText;
    
    [Header("Configurações")]
    [SerializeField] private string gameTitle = "Jogo de Coleta 2D";
    [SerializeField] private string startButtonText = "Começar a Jogar";
    [SerializeField] private string quitButtonText = "Sair";
    
    /// <summary>
    /// Inicializa o menu e configura os botões
    /// </summary>
    void Start()
    {
        InitializeMenu();
        SetupButtons();
    }
    
    /// <summary>
    /// Configura os elementos visuais do menu
    /// </summary>
    private void InitializeMenu()
    {
        // Configura o título se disponível
        if (titleText != null)
        {
            titleText.text = gameTitle;
        }
        
        // Encontra elementos se não foram atribuídos
        if (startButton == null)
        {
            GameObject startObj = GameObject.Find("StartButton");
            if (startObj != null) startButton = startObj.GetComponent<Button>();
        }
        
        if (quitButton == null)
        {
            GameObject quitObj = GameObject.Find("QuitButton");
            if (quitObj != null) quitButton = quitObj.GetComponent<Button>();
        }
    }
    
    /// <summary>
    /// Configura os eventos dos botões
    /// </summary>
    private void SetupButtons()
    {
        // Configura botão de iniciar jogo
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            
            // Atualiza texto do botão se disponível
            Text buttonText = startButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = startButtonText;
            }
        }
        
        // Configura botão de sair
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            
            // Atualiza texto do botão se disponível
            Text buttonText = quitButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = quitButtonText;
            }
        }
    }
    
    /// <summary>
    /// Inicia o jogo carregando a cena principal
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Iniciando jogo...");
        SceneManager.LoadScene("Game");
    }
    
    /// <summary>
    /// Sai do jogo
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    /// <summary>
    /// Retorna ao menu (útil para botões de retorno)
    /// </summary>
    public void ReturnToMenu()
    {
        Debug.Log("Retornando ao menu...");
        SceneManager.LoadScene("MainMenu");
    }
}