using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCaunt;
    public TextMeshProUGUI coinText;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = ": " + coinCaunt.ToString();
    }
}
