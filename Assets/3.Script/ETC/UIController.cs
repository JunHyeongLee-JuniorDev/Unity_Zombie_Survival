using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Text ammoText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    [SerializeField] private GameObject GameOver_ob;

    private void Start()
    {
        GameOver_ob.SetActive(false);
    }

    public void Update_AmmoText(int magAmmo, int Remain)
    {
        ammoText.text = $"{magAmmo} / {Remain}";
    }

    public void update_Score(int magAmmo, int Remain)
    {
        scoreText.text = $"Score : {new}"
    }

    public void update_Wave(int magAmmo, int Remain)
    {

    }
}
