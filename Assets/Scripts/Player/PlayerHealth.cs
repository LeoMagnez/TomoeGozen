using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance { get; set; }

    [HideInInspector]
    public int playerHP = 100;

    public Slider hpSlider;

    public TextMeshProUGUI playerHP_text;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        playerHP_text.SetText(playerHP.ToString() + "/ 100");
    }

    public void SetMaxHealth(int maxHealth)
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        hpSlider.value = health;
    }
}
