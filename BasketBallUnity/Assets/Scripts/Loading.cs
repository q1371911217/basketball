using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    GameObject barGo, btnGo;

    [SerializeField]
    Image i;
    [SerializeField]
    Text t;
    [SerializeField]
    Transform bbb;

    int progress = 0;

    void Awake()
    {
        audioSource.volume = Game.volumeOpen ? 1 : 0;

        if (Game.isLoad)
        {
            barGo.SetActive(false);
            btnGo.SetActive(true);
        }
        else
            StartCoroutine(load());
    }

    IEnumerator load()
    {
        while(progress < 100)
        {
            progress += Random.Range(0, 7);
            if (progress > 100)
                progress = 100;
            i.fillAmount = (float)progress / 100;
            
            bbb.transform.localPosition = new Vector3(-263.5f + 463.6f * i.fillAmount, 0, 0);
            t.text = string.Format("{0}%", progress);
            yield return new WaitForSeconds(0.1f);
        }

        barGo.SetActive(false);
        btnGo.SetActive(true);

        Game.isLoad = true;
    }

    public void onStart()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
