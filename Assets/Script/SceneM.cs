using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setting1()
    {
        SceneManager.LoadScene("Setting1");
    }

    public void tuto() 
    {
        SceneManager.LoadScene("Tutorial");

    }

    public void Main()
    {
        SceneManager.LoadScene("MainScene");

    }

    public void Log()
    {
        SceneManager.LoadScene("Login");

    }

    public void PlayLog()
    {
        SceneManager.LoadScene("PlayLogin");

    }


}
