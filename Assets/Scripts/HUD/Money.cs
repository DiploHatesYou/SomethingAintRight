using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + PlayerStats.money.ToString();
    }
}
