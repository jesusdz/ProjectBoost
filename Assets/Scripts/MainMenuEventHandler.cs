using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenuEventHandler : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] Image blackPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnStartGamePressed()
    {
        StartCoroutine(LoadFirstScene());
    }

    IEnumerator LoadFirstScene()
    {
        blackPanel.gameObject.SetActive(true);

        // Wait while there is sound or the image is not fade out
        while (music.volume > 0f || blackPanel.color.a < 1f)
        {
            music.volume -= Time.deltaTime / 2f;

            Color new_color = blackPanel.color;
            new_color.a += Time.deltaTime / 2f;
            blackPanel.color = new_color;

            yield return null;
        }

        // Wait two more seconds
        float delay = 2f;
        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(1);
    }

    public void OnExitGamePressed()
    {
        Application.Quit(0);
    }
}
