using UnityEngine;

/// <summary>
/// Adiciona animação sutil aos detalhes de grama
/// Cria movimento suave e natural
/// </summary>
public class GrassAnimation : MonoBehaviour
{
    [Header("Configurações de Animação")]
    [SerializeField] private float swaySpeed = 1f;
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float rotationAmount = 5f;
    
    private Vector3 startPosition;
    private Vector3 startRotation;
    private float randomOffset;
    
    /// <summary>
    /// Inicializa a animação
    /// </summary>
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }
    
    /// <summary>
    /// Atualiza a animação a cada frame
    /// </summary>
    void Update()
    {
        AnimateGrass();
    }
    
    /// <summary>
    /// Aplica animação de balanço e rotação
    /// </summary>
    private void AnimateGrass()
    {
        float time = Time.time + randomOffset;
        
        // Animação de balanço horizontal
        float swayX = Mathf.Sin(time * swaySpeed) * swayAmount;
        Vector3 newPosition = startPosition + new Vector3(swayX, 0, 0);
        transform.position = newPosition;
        
        // Animação de rotação sutil
        float rotationZ = Mathf.Sin(time * rotationSpeed) * rotationAmount;
        Vector3 newRotation = startRotation + new Vector3(0, 0, rotationZ);
        transform.eulerAngles = newRotation;
    }
}