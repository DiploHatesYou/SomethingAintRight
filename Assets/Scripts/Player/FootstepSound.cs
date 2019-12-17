using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    private AudioSource footstepSound;

    // Start is called before the first frame update
    void Start()
    {
        footstepSound = GetComponent<AudioSource>();
        footstepSound.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            footstepSound.volume = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            footstepSound.volume = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            footstepSound.volume = 1;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            footstepSound.volume = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            footstepSound.volume = 1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            footstepSound.volume = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            footstepSound.volume = 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            footstepSound.volume = 0;
        }
    }
}
