using UnityEngine;
using System.Collections;

/// <summary>
/// Configuração completa do jogo 2D estilo Mario
/// Adiciona jogador, plataformas e todos os elementos necessários
/// </summary>
public class GameSetupComplete : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    [SerializeField] private bool createPlayer = true;
    [SerializeField] private Vector3 playerStartPosition = new Vector3(-8f, 0.2f, 0);
    
    [Header("Configurações das Plataformas")]
    [SerializeField] private bool createPlatforms = true;
    [SerializeField] private int platformCount = 8;
    
    [Header("Configurações dos Itens")]
    [SerializeField] private bool createCollectibles = true;
    [SerializeField] private int collectibleCount = 10;
    
    [Header("Configurações dos Inimigos")]
    [SerializeField] private bool createEnemies = true;
    [SerializeField] private int enemyCount = 2;
    
    [Header("Configurações da Plataforma de Spawn")]
    [SerializeField] private bool createSpawnPlatform = true;
    [SerializeField] private Vector3 spawnPlatformPosition = new Vector3(-8f, -0.5f, 0f);
    
    [Header("Configurações de Fases")]
    [SerializeField] private int currentPhase = 1;
    private bool isAdvancingPhase = false;
    
    /// <summary>
    /// Inicializa a configuração completa do jogo
    /// </summary>
    void Start()
    {
        SetupCompleteGame();
    }
    
    /// <summary>
    /// Configura todos os elementos do jogo
    /// </summary>
    [ContextMenu("Setup Complete Game")]
    public void SetupCompleteGame()
    {
        Debug.Log($"🔧 INICIANDO SetupCompleteGame para Fase {currentPhase}...");
        Debug.Log($"🔧 createSpawnPlatform: {createSpawnPlatform}");
        Debug.Log($"🔧 createPlayer: {createPlayer}");
        Debug.Log($"🔧 createPlatforms: {createPlatforms}");
        Debug.Log($"🔧 createCollectibles: {createCollectibles}");
        
        // Configurar sistema de pontuação
        Debug.Log("🔧 Configurando sistema de pontuação...");
        SetupScoreManager();
        
        // Configurar dificuldade baseada na fase
        Debug.Log("🔧 Configurando dificuldade...");
        ConfigurePhaseDifficulty();
        
        // Criar plataforma de spawn primeiro
        if (createSpawnPlatform)
        {
            Debug.Log("🔧 Criando plataforma de spawn...");
            CreateSpawnPlatform();
        }
        
        if (createPlayer)
        {
            Debug.Log("🔧 Configurando jogador...");
            SetupPlayer();
        }
        
        if (createPlatforms)
        {
            Debug.Log("🔧 Configurando plataformas...");
            SetupPlatforms();
        }
        
        // Aguarda um frame para garantir que as plataformas foram criadas
        if (createCollectibles)
        {
            Debug.Log("🔧 Configurando coletáveis...");
            StartCoroutine(SetupCollectiblesDelayed());
        }
        
        // Cria inimigos
        if (createEnemies)
        {
            Debug.Log("🔧 Configurando inimigos...");
            StartCoroutine(SetupEnemiesDelayed());
        }
        
        // Cria linha de chegada
        Debug.Log("🔧 Criando linha de chegada...");
        CreateFinishLine();
        
        Debug.Log($"✅ Fase {currentPhase} configurada com sucesso!");
        Debug.Log($"✅ Elementos criados - Verificando...");
        
        // Verifica se os elementos foram criados
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject platforms = GameObject.Find("Platforms");
        GameObject collectibles = GameObject.Find("Collectibles");
        GameObject finishLine = GameObject.Find("FinishLine");
        
        Debug.Log($"✅ Jogador criado: {player != null}");
        Debug.Log($"✅ Plataformas criadas: {platforms != null}");
        Debug.Log($"✅ Coletáveis criados: {collectibles != null}");
        Debug.Log($"✅ Linha de chegada criada: {finishLine != null}");
    }
    
    /// <summary>
    /// Configura a dificuldade baseada na fase atual
    /// </summary>
    private void ConfigurePhaseDifficulty()
    {
        switch (currentPhase)
        {
            case 1:
                // Fase 1 - Fácil (4 plataformas todas no chão, 1 inimigo)
                platformCount = 4;
                collectibleCount = 4;
                enemyCount = 1;
                break;
            case 2:
                // Fase 2 - Mais difícil (5 plataformas com alturas variadas, 2 inimigos)
                platformCount = 5;
                collectibleCount = 5;
                enemyCount = 2;
                break;
            case 3:
                // Fase 3 - Muito difícil (6 plataformas com alturas extremas, 3 inimigos)
                platformCount = 6;
                collectibleCount = 6;
                enemyCount = 3;
                break;
            default:
                // Fases 4+ - Extremamente difícil (7 plataformas, 4 inimigos)
                platformCount = 7;
                collectibleCount = 7;
                enemyCount = 4;
                break;
        }
        
        Debug.Log($"Fase {currentPhase}: {platformCount} plataformas, {collectibleCount} coletáveis");
    }
    
    /// <summary>
    /// Cria a plataforma de spawn para o jogador
    /// </summary>
    private void CreateSpawnPlatform()
    {
        // Verifica se já existe uma plataforma de spawn
        GameObject existingSpawnPlatform = GameObject.Find("SpawnPlatform");
        if (existingSpawnPlatform != null)
        {
            Debug.Log("Plataforma de spawn já existe na cena");
            return;
        }
        
        // Cria GameObject da plataforma de spawn
        GameObject spawnPlatform = new GameObject("SpawnPlatform");
        
        // Posiciona a plataforma (menor)
        spawnPlatform.transform.position = spawnPlatformPosition;
        spawnPlatform.transform.localScale = new Vector3(2f, 0.8f, 1f);
        
        // Adiciona Sprite Renderer
        SpriteRenderer spriteRenderer = spawnPlatform.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreatePlatformSprite();
        spriteRenderer.color = new Color(0.5f, 0.25f, 0f, 1f); // Marrom escuro
        spriteRenderer.sortingOrder = 0;
        
        // Adiciona Box Collider 2D (menor)
        BoxCollider2D collider = spawnPlatform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(2f, 0.8f);
        
        // Adiciona tag Ground para detecção de chão
        spawnPlatform.tag = "Ground";
        
        Debug.Log("Plataforma de spawn criada automaticamente!");
    }
    
    /// <summary>
    /// Cria a linha de chegada na última plataforma
    /// </summary>
    private void CreateFinishLine()
    {
        // Verifica se já existe uma linha de chegada
        GameObject existingFinishLine = GameObject.Find("FinishLine");
        if (existingFinishLine != null)
        {
            Debug.Log("Linha de chegada já existe na cena");
            return;
        }
        
        // Calcula posição da última plataforma (ajustado para ser acessível)
        float lastPlatformX = -6f + ((platformCount - 1) * 3f); // Usa o mesmo espaçamento das plataformas
        // Ajusta para que a linha de chegada fique na última plataforma acessível
        Vector3 finishLinePosition = new Vector3(lastPlatformX, 3f, 0f);
        
        Debug.Log($"🏁 Posição da última plataforma: {lastPlatformX}");
        Debug.Log($"🏁 Posição da linha de chegada: {finishLinePosition}");
        
        // Cria GameObject da linha de chegada
        GameObject finishLine = new GameObject("FinishLine");
        finishLine.transform.position = finishLinePosition;
        finishLine.transform.localScale = new Vector3(1f, 2f, 1f);
        
        // Adiciona Sprite Renderer
        SpriteRenderer spriteRenderer = finishLine.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateFinishLineSprite();
        spriteRenderer.color = new Color(1f, 1f, 0f, 1f); // Amarelo
        spriteRenderer.sortingOrder = 1;
        
        // Adiciona Box Collider 2D como trigger
        BoxCollider2D collider = finishLine.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1f, 2f);
        collider.isTrigger = true;
        
        // Adiciona tag FinishLine
        finishLine.tag = "FinishLine";
        
        Debug.Log($"Linha de chegada criada na posição: {finishLinePosition}");
    }
    
    /// <summary>
    /// Cria sprite para linha de chegada
    /// </summary>
    private Sprite CreateFinishLineSprite()
    {
        int textureWidth = 32;
        int textureHeight = 64;
        
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        Color[] pixels = new Color[textureWidth * textureHeight];
        
        // Preenche com cor amarela
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.yellow;
        }
        
        // Adiciona bordas pretas
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                if (x == 0 || x == textureWidth - 1 || y == 0 || y == textureHeight - 1)
                {
                    pixels[y * textureWidth + x] = Color.black;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f), 32f);
    }
    
    /// <summary>
    /// Configura o jogador
    /// </summary>
    private void SetupPlayer()
    {
        Debug.Log("🔧 CONFIGURANDO JOGADOR...");
        
        // Verifica se já existe um jogador e remove se necessário
        GameObject existingPlayerTag = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayerTag != null)
        {
            Debug.Log("🗑️ Removendo jogador existente para criar um novo com configurações corretas");
            Destroy(existingPlayerTag);
        }
        
        // Também remove o "Jogador" se existir
        GameObject existingJogador = GameObject.Find("Jogador");
        if (existingJogador != null)
        {
            Debug.Log("🗑️ Removendo GameObject 'Jogador' existente");
            Destroy(existingJogador);
        }
        
        // Verifica se ainda há algum jogador na cena
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        if (allPlayers.Length > 0)
        {
            Debug.Log($"⚠️ AINDA HÁ {allPlayers.Length} JOGADORES NA CENA!");
            foreach (GameObject existingPlayer in allPlayers)
            {
                Debug.Log($"⚠️ Jogador encontrado: {existingPlayer.name}");
            }
        }
        
        // Cria o jogador
        GameObject newPlayer = new GameObject("Player");
        newPlayer.transform.position = playerStartPosition;
        newPlayer.tag = "Player";
        
        // Debug para verificar a posição
        Debug.Log($"Jogador criado na posição: {playerStartPosition}");
        
        // Adiciona PlayerController2D
        PlayerController2D playerController = newPlayer.AddComponent<PlayerController2D>();
        
        // Adiciona Rigidbody2D
        Rigidbody2D rb = newPlayer.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        
        // Adiciona Collider2D (EXTREMAMENTE menor para cair nos vãos)
        BoxCollider2D collider = newPlayer.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.1f, 0.2f);
        
        // Debug para confirmar o tamanho
        Debug.Log($"Collider criado com tamanho: {collider.size}");
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = newPlayer.AddComponent<SpriteRenderer>();
        CreatePlayerSprite(spriteRenderer);
        
        Debug.Log("Jogador criado com sucesso!");
    }
    
    /// <summary>
    /// Cria sprite para o jogador
    /// </summary>
    private void CreatePlayerSprite(SpriteRenderer spriteRenderer)
    {
        // Jogador menor: 16x24 pixels (metade do tamanho original)
        Texture2D texture = new Texture2D(16, 24);
        Color[] pixels = new Color[16 * 24];
        
        // Desenha um personagem simples e menor
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 24; y++)
            {
                // Corpo (azul)
                if (x >= 4 && x <= 11 && y >= 8 && y <= 20)
                {
                    pixels[y * 16 + x] = Color.blue;
                }
                // Cabeça (vermelha)
                else if (x >= 5 && x <= 10 && y >= 12 && y <= 20)
                {
                    pixels[y * 16 + x] = Color.red;
                }
                // Pernas (azul escuro)
                else if ((x >= 5 && x <= 6 && y >= 4 && y <= 8) || 
                         (x >= 9 && x <= 10 && y >= 4 && y <= 8))
                {
                    pixels[y * 16 + x] = new Color(0, 0, 0.8f, 1f);
                }
                // Braços (azul escuro)
                else if ((x >= 2 && x <= 3 && y >= 10 && y <= 16) || 
                         (x >= 12 && x <= 13 && y >= 10 && y <= 16))
                {
                    pixels[y * 16 + x] = new Color(0, 0, 0.8f, 1f);
                }
                else
                {
                    pixels[y * 16 + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Sprite menor com pixels per unit maior para ficar mais nítido
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 24), new Vector2(0.5f, 0.5f), 16f);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 2;
    }
    
    /// <summary>
    /// Configura as plataformas com posições fixas
    /// </summary>
    private void SetupPlatforms()
    {
        // Verifica se já existem plataformas
        GameObject existingPlatforms = GameObject.Find("Platforms");
        if (existingPlatforms != null)
        {
            Debug.Log("Plataformas já existem na cena");
            return;
        }
        
        // Cria objeto pai para plataformas
        GameObject platformsParent = new GameObject("Platforms");
        
        // Gera posições das plataformas baseadas na fase
        Vector3[] platformPositions = GeneratePlatformPositions();
        
        // Cria plataformas com posições fixas
        for (int i = 0; i < platformPositions.Length && i < platformCount; i++)
        {
            CreatePlatform(i, platformPositions[i], platformsParent.transform);
        }
        
        Debug.Log($"{platformCount} plataformas criadas com posições fixas!");
    }
    
    /// <summary>
    /// Gera posições das plataformas baseadas na fase atual
    /// </summary>
    private Vector3[] GeneratePlatformPositions()
    {
        Vector3[] positions = new Vector3[platformCount];
        
        for (int i = 0; i < platformCount; i++)
        {
            float x = -6f + (i * 3f); // Espaçamento de 3 unidades
            float y = 0f;
            
            // Dificuldade baseada na fase - SEM ALEATORIEDADE
            if (currentPhase == 1)
            {
                // Fase 1 - Fácil: todas no chão
                y = 0f;
            }
            else if (currentPhase == 2)
            {
                // Fase 2 - Mais difícil: padrão específico mais desafiador MAS POSSÍVEL
                switch (i)
                {
                    case 0: y = 0f; break;    // Primeira plataforma no chão
                    case 1: y = 1.5f; break;  // Segunda plataforma média (reduzida de 2f)
                    case 2: y = 0f; break;    // Terceira plataforma no chão
                    case 3: y = 2f; break;    // Quarta plataforma alta (reduzida de 3f)
                    case 4: y = 1f; break;    // Quinta plataforma média
                    default: y = 0f; break;   // Demais no chão
                }
            }
            else if (currentPhase >= 3)
            {
                // Fase 3+ - Muito difícil: padrão extremo
                switch (i)
                {
                    case 0: y = 0f; break;    // Primeira no chão
                    case 1: y = 3f; break;    // Segunda muito alta
                    case 2: y = -1f; break;   // Terceira baixa
                    case 3: y = 4f; break;    // Quarta extremamente alta
                    case 4: y = 1f; break;    // Quinta média
                    case 5: y = 2f; break;    // Sexta alta
                    default: y = 0f; break;   // Demais no chão
                }
            }
            
            positions[i] = new Vector3(x, y, 0f);
        }
        
        Debug.Log($"🔧 Geradas {platformCount} plataformas para Fase {currentPhase}:");
        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log($"   Plataforma {i + 1}: X = {positions[i].x}, Y = {positions[i].y}");
        }
        
        return positions;
    }
    
    /// <summary>
    /// Cria uma plataforma individual com posição fixa
    /// </summary>
    private void CreatePlatform(int index, Vector3 position, Transform parent)
    {
        GameObject platform = new GameObject($"Platform_{index + 1}");
        platform.transform.SetParent(parent);
        
        // Usa posição fixa
        platform.transform.position = position;
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = platform.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreatePlatformSprite();
        spriteRenderer.color = new Color(0.6f, 0.4f, 0.2f, 1f); // Marrom
        spriteRenderer.sortingOrder = 0;
        
        // Adiciona Collider2D (menor para criar vãos maiores)
        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(2f, 0.5f);
        
        // Adiciona tag
        platform.tag = "Ground";
    }
    
    /// <summary>
    /// Cria sprite para plataforma
    /// </summary>
    private Sprite CreatePlatformSprite()
    {
        int textureWidth = 64; // 2 * 32 (menor)
        int textureHeight = 16; // 0.5 * 32
        
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
    /// Configura os itens coletáveis (um em cada plataforma)
    /// </summary>
    private void SetupCollectibles()
    {
        // Verifica se já existem coletáveis
        GameObject existingCollectibles = GameObject.Find("Collectibles");
        if (existingCollectibles != null)
        {
            Debug.Log("Coletáveis já existem na cena");
            return;
        }
        
        // Cria objeto pai para coletáveis
        GameObject collectiblesParent = new GameObject("Collectibles");
        
        // Gera posições dos coletáveis baseadas na fase
        Vector3[] collectiblePositions = GenerateCollectiblePositions();
        
        // Cria itens coletáveis com posições fixas
        for (int i = 0; i < collectiblePositions.Length && i < collectibleCount; i++)
        {
            CreateCollectible(i, collectiblePositions[i], collectiblesParent.transform);
        }
        
        Debug.Log($"{collectibleCount} itens coletáveis criados com posições fixas!");
    }
    
    /// <summary>
    /// Configura os coletáveis com delay para garantir que as plataformas foram criadas
    /// </summary>
    private IEnumerator SetupCollectiblesDelayed()
    {
        // Aguarda um frame para garantir que as plataformas foram criadas
        yield return null;
        
        SetupCollectibles();
    }
    
    /// <summary>
    /// Gera posições dos coletáveis baseadas na fase atual
    /// </summary>
    private Vector3[] GenerateCollectiblePositions()
    {
        Vector3[] positions = new Vector3[collectibleCount];
        
        // Gera as posições das plataformas primeiro
        Vector3[] platformPositions = GeneratePlatformPositions();
        
        for (int i = 0; i < collectibleCount && i < platformPositions.Length; i++)
        {
            // Coletável nasce EXATAMENTE em cima da plataforma
            float x = platformPositions[i].x; // Mesma posição X da plataforma
            float y = platformPositions[i].y + 2f; // 2 unidades acima da plataforma (mais seguro)
            
            // Garante que o coletável nunca fique no chão
            if (y < 2f)
            {
                y = 2f; // Mínimo de 2 unidades acima do chão
            }
            
            positions[i] = new Vector3(x, y, 0f);
        }
        
        Debug.Log($"🔧 Gerados {collectibleCount} coletáveis em cima das plataformas:");
        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log($"   Coletável {i + 1}: X = {positions[i].x}, Y = {positions[i].y}");
        }
        
        return positions;
    }
    
    /// <summary>
    /// Cria um item coletável individual com posição fixa
    /// </summary>
    private void CreateCollectible(int index, Vector3 position, Transform parent)
    {
        GameObject collectible = new GameObject($"Collectible_{index + 1}");
        collectible.transform.SetParent(parent);
        
        // Usa posição fixa
        collectible.transform.position = position;
        
        // Adiciona componentes necessários
        collectible.AddComponent<CollectibleItem>();
        collectible.tag = "Collectible";
        
        // Adiciona SpriteRenderer
        SpriteRenderer spriteRenderer = collectible.AddComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
        spriteRenderer.sortingOrder = 1;
        
        // Adiciona Collider2D
        CircleCollider2D collider = collectible.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.3f;
        
        // Debug para verificar se o coletável foi criado corretamente
        Debug.Log($"Coletável {index + 1} criado em posição {position} com trigger: {collider.isTrigger}");
    }
    
    /// <summary>
    /// Método chamado quando o jogador chega na linha de chegada
    /// </summary>
    public void OnReachFinishLine()
    {
        Debug.Log($"🏁 OnReachFinishLine CHAMADO!");
        Debug.Log($"🏁 JOGADOR CHEGOU NA LINHA DE CHEGADA!");
        Debug.Log($"🏁 Fase atual: {currentPhase}");
        Debug.Log($"🎉 FASE {currentPhase} COMPLETA! Avançando para próxima fase...");
        
        // Verifica se já está avançando para evitar múltiplas chamadas
        if (isAdvancingPhase)
        {
            Debug.Log("⚠️ Já está avançando para próxima fase, ignorando...");
            return;
        }
        
        isAdvancingPhase = true;
        AdvanceToNextPhase();
    }
    
    /// <summary>
    /// Avança para a próxima fase
    /// </summary>
    private void AdvanceToNextPhase()
    {
        Debug.Log($"🚀 INICIANDO AVANÇO PARA PRÓXIMA FASE...");
        Debug.Log($"🚀 Fase atual ANTES do avanço: {currentPhase}");
        Debug.Log($"🚀 isAdvancingPhase: {isAdvancingPhase}");
        
        currentPhase++;
        
        Debug.Log($"🎉 FASE {currentPhase - 1} COMPLETA! 🎉");
        Debug.Log($"🎉 Nova fase: {currentPhase}");
        
        // Mostra mensagem de fase completa
        Debug.Log("📢 Mostrando mensagem de fase completa...");
        ShowPhaseCompleteMessage();
        
        // Carrega a próxima cena baseada na fase
        Debug.Log($"🔄 Carregando cena da Fase {currentPhase}...");
        LoadPhaseScene(currentPhase);
        
        // Reseta flag de avanço
        isAdvancingPhase = false;
        
        Debug.Log($"✅ FASE {currentPhase} CARREGADA COM SUCESSO!");
    }
    
    /// <summary>
    /// Carrega a cena da fase especificada
    /// </summary>
    private void LoadPhaseScene(int phase)
    {
        string sceneName = "";
        
        switch (phase)
        {
            case 1:
                sceneName = "Game"; // Cena principal
                break;
            case 2:
                sceneName = "GamePhase2"; // Cena da Fase 2
                break;
            case 3:
                // Fase 3 não existe - vai para Game Over
                Debug.Log("🎉 JOGO COMPLETO! Todas as fases foram concluídas!");
                sceneName = "GameOver";
                break;
            default:
                // Qualquer fase além da 2 vai para Game Over
                Debug.Log("🎉 JOGO COMPLETO! Todas as fases foram concluídas!");
                sceneName = "GameOver";
                break;
        }
        
        Debug.Log($"🔄 Carregando cena: {sceneName}");
        
        // Carrega a cena
        Debug.Log($"🔄 Tentando carregar cena: {sceneName}");
        
        try
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            Debug.Log($"✅ Cena {sceneName} carregada com sucesso!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ ERRO ao carregar cena {sceneName}: {e.Message}");
            Debug.LogError($"❌ Verifique se a cena {sceneName} existe e está no Build Settings!");
            
            // Se não conseguir carregar, volta para a cena principal
            Debug.Log("🔄 Voltando para a cena principal...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
    
    /// <summary>
    /// Mostra mensagem de fase completa
    /// </summary>
    private void ShowPhaseCompleteMessage()
    {
        // Cria UI temporária para mostrar mensagem
        GameObject phaseCompleteUI = new GameObject("PhaseCompleteMessage");
        phaseCompleteUI.transform.position = Vector3.zero;
        
        // Adiciona SpriteRenderer para mostrar texto visual
        SpriteRenderer messageRenderer = phaseCompleteUI.AddComponent<SpriteRenderer>();
        messageRenderer.sprite = CreatePhaseCompleteSprite();
        messageRenderer.sortingOrder = 10; // Acima de tudo
        
        // Remove após 3 segundos
        Destroy(phaseCompleteUI, 3f);
    }
    
    /// <summary>
    /// Cria sprite para mensagem de fase completa
    /// </summary>
    private Sprite CreatePhaseCompleteSprite()
    {
        Texture2D texture = new Texture2D(256, 64);
        Color[] pixels = new Color[256 * 64];
        
        // Preenche com fundo verde
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0f, 1f, 0f, 0.8f); // Verde semi-transparente
        }
        
        // Desenha texto simples "FASE COMPLETA!"
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                // Texto "FASE COMPLETA!" em branco
                if ((x >= 20 && x <= 40 && y >= 20 && y <= 44) || // F
                    (x >= 50 && x <= 70 && y >= 20 && y <= 44) || // A
                    (x >= 80 && x <= 100 && y >= 20 && y <= 44) || // S
                    (x >= 110 && x <= 130 && y >= 20 && y <= 44) || // E
                    (x >= 140 && x <= 160 && y >= 20 && y <= 44) || // C
                    (x >= 170 && x <= 190 && y >= 20 && y <= 44) || // O
                    (x >= 200 && x <= 220 && y >= 20 && y <= 44)) // M
                {
                    pixels[y * 256 + x] = Color.white;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 256, 64), new Vector2(0.5f, 0.5f), 32f);
    }
    
    /// <summary>
    /// Método para testar manualmente o avanço de fase
    /// </summary>
    [ContextMenu("Test Advance Phase")]
    public void TestAdvancePhase()
    {
        Debug.Log("🧪 TESTE MANUAL: Avançando para próxima fase...");
        AdvanceToNextPhase();
    }
    
    /// <summary>
    /// Método para forçar a criação de todos os elementos
    /// </summary>
    [ContextMenu("Force Create All Elements")]
    public void ForceCreateAllElements()
    {
        Debug.Log("🔧 FORÇANDO CRIAÇÃO DE TODOS OS ELEMENTOS...");
        
        // Força todas as configurações como true
        createSpawnPlatform = true;
        createPlayer = true;
        createPlatforms = true;
        createCollectibles = true;
        
        Debug.Log("🔧 Configurações forçadas como true");
        
        // Chama SetupCompleteGame
        SetupCompleteGame();
        
        Debug.Log("🔧 FORÇA CRIAÇÃO CONCLUÍDA!");
    }
    
    /// <summary>
    /// Método para simular chegada na linha de chegada
    /// </summary>
    [ContextMenu("Simulate Reach Finish Line")]
    public void SimulateReachFinishLine()
    {
        Debug.Log("🧪 TESTE MANUAL: Simulando chegada na linha de chegada...");
        OnReachFinishLine();
    }
    
    /// <summary>
    /// Método para verificar o estado atual do sistema
    /// </summary>
    [ContextMenu("Check System Status")]
    public void CheckSystemStatus()
    {
        Debug.Log($"📊 STATUS DO SISTEMA:");
        Debug.Log($"   Fase atual: {currentPhase}");
        Debug.Log($"   Plataformas na fase: {platformCount}");
        Debug.Log($"   Coletáveis na fase: {collectibleCount}");
        
        // Verifica se há coletáveis na cena
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        Debug.Log($"   Coletáveis na cena: {collectibles.Length}");
        
        // Verifica se há linha de chegada na cena
        GameObject finishLine = GameObject.Find("FinishLine");
        if (finishLine != null)
        {
            Debug.Log($"   Linha de chegada encontrada: {finishLine.name}");
        }
        else
        {
            Debug.Log($"   ❌ LINHA DE CHEGADA NÃO ENCONTRADA!");
        }
        
        // Verifica se há jogador na cena
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log($"   Jogador encontrado: {player.name}");
        }
        else
        {
            Debug.Log($"   ❌ JOGADOR NÃO ENCONTRADO!");
        }
    }
    
    /// <summary>
    /// Limpa todos os elementos criados
    /// </summary>
    [ContextMenu("Clear All Elements")]
    public void ClearAllElements()
    {
        // Remove jogador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        
        // Remove plataforma de spawn
        GameObject spawnPlatform = GameObject.Find("SpawnPlatform");
        if (spawnPlatform != null)
        {
            Destroy(spawnPlatform);
        }
        
        // Remove plataformas
        GameObject platforms = GameObject.Find("Platforms");
        if (platforms != null)
        {
            Destroy(platforms);
        }
        
        // Remove coletáveis
        GameObject collectibles = GameObject.Find("Collectibles");
        if (collectibles != null)
        {
            Destroy(collectibles);
        }
        
        // Remove inimigos
        GameObject enemies = GameObject.Find("Enemies");
        if (enemies != null)
        {
            Destroy(enemies);
        }
        
        // Remove linha de chegada
        GameObject finishLine = GameObject.Find("FinishLine");
        if (finishLine != null)
        {
            Destroy(finishLine);
        }
        
        Debug.Log("Todos os elementos foram removidos");
    }
    
    /// <summary>
    /// Configura os inimigos com delay para garantir que as plataformas foram criadas
    /// </summary>
    private IEnumerator SetupEnemiesDelayed()
    {
        // Aguarda um frame para garantir que as plataformas foram criadas
        yield return null;
        
        SetupEnemies();
    }
    
    /// <summary>
    /// Configura os inimigos
    /// </summary>
    private void SetupEnemies()
    {
        // Verifica se já existem inimigos
        GameObject existingEnemies = GameObject.Find("Enemies");
        if (existingEnemies != null)
        {
            Debug.Log("Inimigos já existem na cena");
            return;
        }
        
        // Cria objeto pai para inimigos
        GameObject enemiesParent = new GameObject("Enemies");
        
        // Gera posições dos inimigos baseadas na fase
        Vector3[] enemyPositions = GenerateEnemyPositions();
        
        // Cria inimigos com posições fixas
        for (int i = 0; i < enemyPositions.Length && i < enemyCount; i++)
        {
            CreateEnemy(i, enemyPositions[i], enemiesParent.transform);
        }
        
        Debug.Log($"{enemyCount} inimigos criados com posições fixas!");
    }
    
    /// <summary>
    /// Gera posições dos inimigos baseadas nas plataformas (mais distantes do jogador)
    /// </summary>
    private Vector3[] GenerateEnemyPositions()
    {
        Vector3[] positions = new Vector3[enemyCount];
        
        // Gera as posições das plataformas primeiro
        Vector3[] platformPositions = GeneratePlatformPositions();
        
        // Jogador nasce em X = -8, então inimigos nascem nas plataformas do meio (não na linha de chegada)
        for (int i = 0; i < enemyCount && i < platformPositions.Length; i++)
        {
            // Pula as primeiras plataformas (próximas do jogador) e a última (linha de chegada)
            // Usa plataformas do meio, começando da penúltima e indo para trás
            int platformIndex = platformPositions.Length - 2 - i; // -2 para pular a última plataforma
            
            // Garante que não use plataformas muito próximas do jogador (X < -3)
            if (platformIndex < 1 || platformPositions[platformIndex].x < -3f)
            {
                // Se não há plataformas suficientes, usa a penúltima
                platformIndex = Mathf.Max(1, platformPositions.Length - 2);
            }
            
            // Inimigo nasce EXATAMENTE em cima da plataforma
            float x = platformPositions[platformIndex].x; // Mesma posição X da plataforma
            float y = platformPositions[platformIndex].y + 0.6f; // 0.6 unidades acima da plataforma
            
            positions[i] = new Vector3(x, y, 0f);
        }
        
        Debug.Log($"🔧 Gerados {enemyCount} inimigos em cima das plataformas (distantes do jogador, não na linha de chegada):");
        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log($"   Inimigo {i + 1}: X = {positions[i].x}, Y = {positions[i].y}");
        }
        
        return positions;
    }
    
    /// <summary>
    /// Cria um inimigo individual com posição fixa
    /// </summary>
    private void CreateEnemy(int index, Vector3 position, Transform parent)
    {
        GameObject enemy = new GameObject($"Enemy_{index + 1}");
        enemy.transform.SetParent(parent);
        
        // Usa posição fixa
        enemy.transform.position = position;
        
        // Adiciona EnemyController
        enemy.AddComponent<EnemyController>();
        
        Debug.Log($"Inimigo {index + 1} criado em posição {position}");
    }
    
    /// <summary>
    /// Configura o sistema de pontuação
    /// </summary>
    private void SetupScoreManager()
    {
        // Verifica se já existe um ScoreManager
        if (ScoreManager.Instance == null)
        {
            // Cria GameObject para o ScoreManager
            GameObject scoreManagerObj = new GameObject("ScoreManager");
            
            // Adiciona o componente ScoreManager
            scoreManagerObj.AddComponent<ScoreManager>();
            
            Debug.Log("Sistema de pontuação criado automaticamente!");
        }
        else
        {
            Debug.Log("Sistema de pontuação já existe, verificando UI...");
            
            // Força a recriação da UI para garantir que apareça
            ScoreManager.Instance.RecreateUI();
        }
    }
}