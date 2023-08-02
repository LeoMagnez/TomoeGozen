using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHP = 100;

    public TextMeshProUGUI playerHP_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHP_text.SetText(playerHP.ToString() + "/ 100");
    }
}
