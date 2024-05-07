using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton instance

    public int coinsPerSecond = 1; // Amount of coins gained per second

    private int totalCoins = 0; // Total coins collected

    public TMP_Text coinText; // Reference to the TextMeshPro text displaying the coins

    private bool isGeneratingCoins = true; // Flag to control coin generation coroutine

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scene changes
            LoadCoins(); // Load coins when the script starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SaveCoins(); // Save coins when the script is destroyed
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins); // Save coins to PlayerPrefs
        PlayerPrefs.Save(); // Ensure PlayerPrefs data is saved immediately
    }

    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Load coins from PlayerPrefs
        UpdateCoinText(); // Update UI text when loading coins
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Register scene load event handler
        StartCoroutine(GenerateCoins()); // Start the coin generation coroutine
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene") // Check if the main scene is loaded
        {
            isGeneratingCoins = false; // Stop generating coins when in the main scene
        }
        else
        {
            isGeneratingCoins = true; // Resume generating coins in other scenes
        }
    }

    private System.Collections.IEnumerator GenerateCoins()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (isGeneratingCoins)
            {
                totalCoins += coinsPerSecond; // Increment total coins if coin generation is enabled
                UpdateCoinText(); // Update UI text
                SaveCoins(); // Save coins after each increment
            }
        }
    }

    // Update the UI text displaying the coins
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString(); // Update UI text without prefix
        }
    }
}