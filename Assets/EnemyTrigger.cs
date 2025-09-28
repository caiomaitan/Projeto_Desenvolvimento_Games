using UnityEngine;

/// <summary>
/// Script auxiliar para detectar colisões do inimigo com o jogador
/// </summary>
public class EnemyTrigger : MonoBehaviour
{
    private EnemyController enemyController;
    
    /// <summary>
    /// Define qual inimigo controla este trigger
    /// </summary>
    public void SetEnemy(EnemyController enemy)
    {
        enemyController = enemy;
    }
    
    /// <summary>
    /// Detecta colisões com o jogador
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyController != null)
        {
            // Verifica se o jogador está vindo de cima (pulo na cabeça)
            // Jogador deve estar pelo menos 0.3 unidades acima do inimigo
            if (other.transform.position.y > enemyController.transform.position.y + 0.3f)
            {
                // Jogador pulou na cabeça - inimigo morre imediatamente
                Debug.Log("🦘 PULO NA CABEÇA! Inimigo morrendo...");
                enemyController.Die();
            }
            else
            {
                // Jogador tocou o inimigo - jogador morre
                Debug.Log("💥 Jogador tocou o inimigo! Morrendo...");
                PlayerController2D playerController = other.GetComponent<PlayerController2D>();
                if (playerController != null)
                {
                    playerController.Die();
                }
            }
        }
    }
}

