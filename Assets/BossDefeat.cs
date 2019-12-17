using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    Enemy Enemy;

    public GameObject winUI;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.health <= 0)
        {
            winUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            cam.GetComponent<MouseOrbit>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Pause.gameIsPaused = true;
        }
    }
}
