using TMPro;
using UnityEngine;

public class XpText : MonoBehaviour
{
    TextMeshProUGUI _xpText;

    // Start is called before the first frame update
    void Start()
    {
        _xpText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _xpText.text = "XP - " + PlayerStats.xp.ToString() + " /" + PlayerStats.nextLevelXP.ToString() + " Level - " + PlayerStats.level.ToString();
    }
}
