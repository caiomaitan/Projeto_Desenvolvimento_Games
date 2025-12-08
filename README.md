# 🎮 Jogo 2D Estilo Mario - Projeto de Desenvolvimento de Games

## 📋 Descrição do Projeto

Este é um jogo 2D desenvolvido em Unity que simula o estilo clássico do Mario, com sistema de plataformas, coleta de itens, inimigos e múltiplas fases.

## ✨ Funcionalidades Implementadas

### 🎯 Sistema de Jogabilidade
- **Movimento do Jogador**: Movimento horizontal e pulo com física 2D
- **Sistema de Plataformas**: Plataformas com alturas variadas e dificuldade progressiva
- **Coleta de Itens**: Sistema de coletáveis que aumentam a pontuação
- **Inimigos**: Inimigos que se movem pelas plataformas
- **Múltiplas Fases**: Sistema de progressão com 2 fases principais

### 🏆 Sistema de Pontuação
- **Pontuação por Coleta**: Cada item coletado adiciona 1 ponto
- **UI de Pontuação**: Exibição em tempo real no canto superior esquerdo
- **Reset Automático**: Pontuação zera quando o jogador morre
- **Persistência**: Sistema mantém pontuação entre cenas

### 🎨 Interface e Visual
- **Menu Principal**: Tela de início do jogo
- **Game Over**: Tela de fim de jogo
- **Sprites Procedurais**: Personagens e objetos gerados via código
- **Efeitos Visuais**: Animações e feedback visual

## 🛠️ Tecnologias Utilizadas

- **Unity 2022.3 LTS** - Engine principal
- **C#** - Linguagem de programação
- **TextMesh Pro** - Sistema de texto avançado
- **Unity 2D Physics** - Sistema de física 2D

## 📁 Estrutura do Projeto

```
Assets/
├── _Project/
│   ├── Scenes/           # Cenas do jogo
│   │   ├── MainMenu.unity
│   │   ├── Game.unity
│   │   ├── GamePhase2.unity
│   │   └── GameOver.unity
│   └── Scripts/          # Scripts organizados por categoria
├── CollectibleItem.cs    # Sistema de coleta de itens
├── PlayerController2D.cs # Controle do jogador
├── ScoreManager.cs       # Gerenciamento de pontuação
├── GameSetupComplete.cs  # Configuração automática do jogo
├── EnemyController.cs    # Controle de inimigos
└── BuildScript.cs        # Script para builds automatizados
```

## 🎮 Como Jogar

### Controles
- **Setas ← →** ou **A/D**: Mover para esquerda/direita
- **Espaço** ou **W** ou **↑**: Pular
- **ESC**: Voltar ao menu (em algumas telas)

### Objetivo
1. **Colete todos os itens** amarelos nas plataformas
2. **Evite os inimigos** vermelhos
3. **Chegue até a linha de chegada** amarela
4. **Complete as fases** para progredir no jogo

### Sistema de Pontuação
- Cada item coletado = **+1 ponto**
- Pontuação é exibida no canto superior esquerdo
- Ao morrer, a pontuação é resetada para 0
- Pontuação persiste entre as fases

## 🚀 Como Executar

### Opção 1: Build Executável (Recomendado)
1. Baixe o arquivo `Jogo2D_MarioStyle.exe` da pasta `Builds/Windows/`
2. Execute o arquivo diretamente no Windows
3. Não é necessário instalar o Unity

### Opção 2: Unity Editor
1. Instale o Unity 2022.3 LTS ou superior
2. Clone este repositório
3. Abra o projeto no Unity
4. Execute a cena `MainMenu` no editor

## 🔧 Como Fazer Build

### Método Automático (Unity Editor)
1. Abra o projeto no Unity
2. Vá em **Build → Build Windows** no menu
3. O build será gerado automaticamente em `Builds/Windows/`

### Método Manual
1. Abra **File → Build Settings**
2. Adicione as cenas na ordem:
   - MainMenu
   - Game
   - GamePhase2
   - GameOver
3. Selecione **PC, Mac & Linux Standalone**
4. Configure **Target Platform** para **Windows**
5. Clique em **Build**

## 📊 Especificações Técnicas

### Requisitos Mínimos
- **Sistema Operacional**: Windows 7/8/10/11 (64-bit)
- **Processador**: Intel Core i3 ou AMD equivalente
- **Memória RAM**: 4 GB
- **Espaço em Disco**: 500 MB
- **Placa de Vídeo**: DirectX 11 compatível

### Resolução Suportada
- **Resolução Mínima**: 1024x768
- **Resolução Recomendada**: 1920x1080
- **Modo**: Janela ou Tela Cheia

## 🎯 Funcionalidades Técnicas

### Sistema de Pontuação
- **Singleton Pattern** para acesso global
- **Eventos** para notificar mudanças
- **PlayerPrefs** para persistência
- **UI Automática** criada via código

### Sistema de Fases
- **Geração Procedural** de plataformas
- **Dificuldade Progressiva** por fase
- **Sistema de Checkpoint** automático
- **Transição entre Cenas** suave

### Física e Movimento
- **Rigidbody2D** para física realista
- **Collider2D** para detecção de colisão
- **Ground Check** para detecção de chão
- **Jump Buffer** para pulos responsivos

## 🐛 Problemas Conhecidos

1. **Hitbox do Jogador**: Intencionalmente pequena para permitir passagem por vãos
2. **Inimigos**: Podem atravessar plataformas em casos raros
3. **UI**: Pode não aparecer em resoluções muito baixas

## 🔄 Versões e Atualizações

### Versão Atual: 1.0.0
- ✅ Sistema de pontuação completo
- ✅ Múltiplas fases funcionais
- ✅ Build para Windows
- ✅ Sistema de coleta de itens
- ✅ Controle de inimigos

## 📝 Notas de Desenvolvimento

### Arquitetura do Código
- **Modular**: Cada funcionalidade em script separado
- **Documentado**: Comentários extensivos em português
- **Reutilizável**: Componentes podem ser facilmente modificados
- **Escalável**: Fácil adição de novas fases e funcionalidades

### Padrões Utilizados
- **Singleton**: Para ScoreManager
- **Observer**: Para eventos de pontuação
- **Factory**: Para criação de objetos do jogo
- **State Machine**: Para controle de fases

## 👨‍💻 Desenvolvedor

**Caio Maitan** - Projeto de Desenvolvimento de Games

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais e de aprendizado.

---

## 🎉 Agradecimentos

- Unity Technologies pelo engine
- Comunidade Unity pelos recursos e tutoriais
- TextMesh Pro pela tipografia avançada

**Divirta-se jogando! 🎮**






