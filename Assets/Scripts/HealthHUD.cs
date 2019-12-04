using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Image healthBar;
    
    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = PlayerStats.health;
    }
}
