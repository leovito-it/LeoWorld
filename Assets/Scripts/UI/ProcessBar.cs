using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    public string sceneName;
    string currentSceneName;
    public Image background, loading;
    public Text textProgress;
    AsyncOperation ao;
    Vector2 backgroundSize;

    // Update is called once per frame
    public void GameStart()
    {
        gameObject.SetActive(true);
        backgroundSize = background.GetComponent<RectTransform>().sizeDelta;
        currentSceneName = SceneManager.GetActiveScene().name;
        Invoke("PrepareScene", 2f);
    }

    void PrepareScene()
    {
        ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        ao.allowSceneActivation = true;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        while (!ao.isDone)
        {
            textProgress.text = ao.progress * 100 + " %";
            loading.GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundSize.x * ao.progress, 0);
            yield return new WaitForFixedUpdate();
        }
        if (ao.isDone)
            SceneManager.UnloadSceneAsync(currentSceneName);
    }
}
