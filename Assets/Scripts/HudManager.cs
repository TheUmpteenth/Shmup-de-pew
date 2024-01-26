using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    private static HudManager Instance { get; set; } // don't give access to the singleton - it's just that we don't want two of this.
    
    [SerializeField] private TextMeshProUGUI m_livesDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    
    private void Awake() 
    { 
        // If there is an instance, and it's not this, delete this.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        LivesManager.onLivesChanged += UpdateLivesDisplay;
        ScoreManager.onScoreChanged += UpdateScoreDisplay;
        UpdateLivesDisplay();
        UpdateScoreDisplay();
    }

    private void UpdateLivesDisplay()
    {
        m_livesDisplay.SetText($"Lives: {LivesManager.Lives}");
    }

    private void UpdateScoreDisplay()
    {
        m_scoreDisplay.SetText($"Score: {ScoreManager.Score}");
    }
}
