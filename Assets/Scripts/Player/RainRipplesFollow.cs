using UnityEngine;

public class RainRipplesFollow : MonoBehaviour
{
    public Transform player;
    public GameObject rainRipples;

    private float _insTimer = 2f;
    private bool _newPS = false;


    private void Update()
    {
        RainRipples();        
    }


    void RainRipples()
    {
        _insTimer -= Time.deltaTime;
        if (_insTimer <= 0f)
        {
            Instantiate(rainRipples, new Vector3(player.position.x, transform.position.y, player.position.z), Quaternion.identity);
            _insTimer = 5f;
            _newPS = true;
        }

        if (_newPS == true)
        {
            Destroy(rainRipples, 3f);
        }
    }
}
