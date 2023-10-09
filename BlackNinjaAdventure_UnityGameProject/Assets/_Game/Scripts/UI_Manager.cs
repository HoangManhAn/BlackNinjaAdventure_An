using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private static UI_Manager instance;
    public static UI_Manager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UI_Manager>();
            }

            return instance;
        }
    }

    //private void Awake()
    //{
    //    instance = this;
    //}

    [SerializeField] Text coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

}



