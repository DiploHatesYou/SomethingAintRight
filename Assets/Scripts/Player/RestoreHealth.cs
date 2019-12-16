using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    private Canvas Canvas;
    private bool _canHeal = false;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GetComponentInChildren<Canvas>();
        Canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canHeal == true && PlayerStats.money >= 100)
        {
            PlayerStats.health = 1f;
            PlayerStats.money -= 100;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Canvas.gameObject.SetActive(true);
            _canHeal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Canvas.gameObject.SetActive(false);
            _canHeal = false;
        }
    }
}
