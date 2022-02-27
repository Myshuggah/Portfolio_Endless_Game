using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver; //static - global variable so it can be accessed globaly
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public Text coinsText;
    
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1; //Game goes to normal speed at start
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Game Over function
        if (gameOver)
        {
            Time.timeScale = 0; //basically to stop in game time
            gameOverPanel.SetActive(true);
        }

        // Score Counter
        coinsText.text = "Coins: " + numberOfCoins;

        // Start Game Function
        if(SwipeManager.tap)
        {
            isGameStarted = true;
            PlayerController.gameHasStarted = true;
            Destroy(startingText);
        }
    }
}
