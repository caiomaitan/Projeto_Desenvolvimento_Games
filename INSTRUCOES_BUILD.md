# 🛠️ Instruções para Build do Jogo

## 📋 Pré-requisitos

1. **Unity 2022.3 LTS** ou superior instalado
2. **Projeto aberto** no Unity Editor
3. **Todas as cenas** configuradas corretamente

## 🚀 Método 1: Build Automático (Recomendado)

### Passo a Passo:
1. Abra o projeto no Unity Editor
2. No menu superior, vá em **Build → Build Windows**
3. Aguarde o processo de build (pode levar alguns minutos)
4. O arquivo executável será gerado em `Builds/Windows/Jogo2D_MarioStyle.exe`
5. A pasta do build será aberta automaticamente

### Build de Desenvolvimento:
- Use **Build → Build Windows (Development)** para versão com debug

## 🔧 Método 2: Build Manual

### Configuração das Cenas:
1. Abra **File → Build Settings**
2. Adicione as cenas na seguinte ordem:
   ```
   Assets/_Project/Scenes/MainMenu.unity
   Assets/_Project/Scenes/Game.unity
   Assets/_Project/Scenes/GamePhase2.unity
   Assets/_Project/Scenes/GameOver.unity
   ```

### Configurações do Build:
- **Platform**: PC, Mac & Linux Standalone
- **Target Platform**: Windows
- **Architecture**: x86_64 (64-bit)
- **Scripting Backend**: Mono
- **Api Compatibility Level**: .NET Standard 2.1

### Executar Build:
1. Clique em **Build**
2. Escolha a pasta `Builds/Windows/`
3. Nome do arquivo: `Jogo2D_MarioStyle.exe`
4. Aguarde a conclusão

## 📁 Estrutura do Build Final

```
Builds/Windows/
├── Jogo2D_MarioStyle.exe          # Executável principal
├── Jogo2D_MarioStyle_Data/        # Dados do jogo
│   ├── global-metadata.dat
│   ├── il2cpp_data/
│   ├── Managed/
│   ├── Resources/
│   └── StreamingAssets/
├── UnityCrashHandler64.exe        # Handler de crashes
├── UnityPlayer.dll                # Runtime do Unity
└── winhttp.dll                    # Biblioteca do Windows
```

## ✅ Verificação do Build

### Testes Obrigatórios:
1. **Execução**: O jogo deve abrir sem erros
2. **Menu Principal**: Deve carregar corretamente
3. **Jogabilidade**: Movimento e pulo funcionando
4. **Coleta de Itens**: Sistema de pontuação ativo
5. **Transição de Fases**: Mudança entre cenas
6. **Game Over**: Tela de fim de jogo

### Problemas Comuns:
- **Erro de DLL**: Verifique se todas as dependências estão incluídas
- **Cenas não carregam**: Verifique Build Settings
- **UI não aparece**: Verifique TextMesh Pro no build

## 📦 Distribuição

### Para Entrega:
1. **Compacte** toda a pasta `Builds/Windows/`
2. **Nome do arquivo**: `Jogo2D_MarioStyle_v1.0.zip`
3. **Inclua** o README.md na raiz do projeto
4. **Verifique** se o executável roda em outro computador

### Tamanho Esperado:
- **Build completo**: ~100-200 MB
- **Executável**: ~50-100 MB
- **Dependências**: ~50-100 MB

## 🔍 Troubleshooting

### Build Falha:
1. Verifique se todas as cenas estão adicionadas
2. Confirme se não há erros no Console
3. Limpe o cache: **Assets → Reimport All**
4. Tente build em pasta vazia

### Executável não Abre:
1. Verifique se tem .NET Framework instalado
2. Execute como administrador
3. Verifique antivírus (pode bloquear)
4. Teste em outro computador

### Performance Ruim:
1. Use build de Release (não Development)
2. Desabilite logs desnecessários
3. Otimize texturas e sprites
4. Reduza qualidade gráfica se necessário

## 📋 Checklist Final

- [ ] Build executado com sucesso
- [ ] Executável abre sem erros
- [ ] Todas as funcionalidades testadas
- [ ] Arquivo compactado para entrega
- [ ] README.md incluído
- [ ] Link do repositório Git fornecido

## 🎯 Informações para Entrega

### Arquivos Necessários:
1. **Executável**: `Jogo2D_MarioStyle.exe` + dependências
2. **Código Fonte**: Repositório Git completo
3. **Documentação**: README.md atualizado
4. **Instruções**: Este arquivo (INSTRUCOES_BUILD.md)

### Links Importantes:
- **Repositório Git**: https://github.com/caiomaitan/Projeto_Desenvolvimento_Games.git
- **Build Download**: [Link para download do build]

---

**Boa sorte com a entrega! 🎮**






