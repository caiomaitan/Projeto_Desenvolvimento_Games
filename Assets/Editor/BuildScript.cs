using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

/// <summary>
/// Script para automatizar o build do jogo para Windows
/// </summary>
public class BuildScript
{
    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        // Configurações do build
        string buildPath = "Builds/Windows/";
        string executableName = "Jogo2D_MarioStyle";
        
        // Cria o diretório se não existir
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        
        // Configura as cenas para build
        string[] scenes = {
            "Assets/_Project/Scenes/MainMenu.unity",
            "Assets/_Project/Scenes/Game.unity",
            "Assets/_Project/Scenes/GamePhase2.unity",
            "Assets/_Project/Scenes/GameOver.unity"
        };
        
        // Configurações do BuildPlayerOptions
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath + executableName + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        
        Debug.Log("Iniciando build para Windows...");
        Debug.Log($"Cenas incluídas: {string.Join(", ", scenes)}");
        Debug.Log($"Caminho do build: {buildPlayerOptions.locationPathName}");
        
        // Executa o build
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build concluído com sucesso! Tamanho: {summary.totalSize} bytes");
            Debug.Log($"Arquivo gerado: {buildPlayerOptions.locationPathName}");
            
            // Abre a pasta do build
            EditorUtility.RevealInFinder(buildPath);
        }
        else
        {
            Debug.LogError($"Build falhou: {summary.result}");
        }
    }
}
