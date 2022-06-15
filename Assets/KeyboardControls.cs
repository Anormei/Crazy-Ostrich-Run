using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class KeyboardControls : MonoBehaviour
{
    [SerializeField]
    private UnityEvent holdSpace;
    [SerializeField]
    private UnityEvent releaseSpace;
    [SerializeField]
    private UnityEvent holdDownArrow;
    [SerializeField]
    private UnityEvent releaseDownArrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdSpace.Invoke();   
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            releaseSpace.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            holdDownArrow.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            releaseDownArrow.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("GameplayScene");
        }
    }
}
