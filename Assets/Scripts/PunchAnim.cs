using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAnim : MonoBehaviour
{
    private Animator _anim;
    bool _punch;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _punch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _punch = true;
        }
        else
        {
            _punch = false;
        }

        if (_punch == true)
        {
            _anim.SetBool("Punch", true);
        }
        if (_punch == false)
        {
            _anim.SetBool("Punch", false);
        }
    }
}
