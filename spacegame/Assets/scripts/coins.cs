using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton instance

    public int coinsPerSecond = 1; // Amount of coins gained per second
    public float ticketGenerationChance = 0.00001f; // Chance of generating a ticket (0.001%)

    private int totalCoins = 0; // Total coins collected
    private int totalTickets = 0; // Total tickets collected

    public TMP_Text coinText; // Reference to the TextMeshPro text displaying the coins
    public TMP_Text ticketText; // Reference to the TextMeshPro text displaying the tickets

    private bool isGeneratingCoins = true; // Flag to control coin generation coroutine

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SaveData(); // Save data when the script is destroyed
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins); // Save coins to PlayerPrefs
        PlayerPrefs.SetInt("TotalTickets", totalTickets); // Save tickets to PlayerPrefs
        PlayerPrefs.Save(); // Ensure PlayerPrefs data is saved immediately
    }

    private void LoadData()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Load coins from PlayerPrefs
        totalTickets = PlayerPrefs.GetInt("TotalTickets", 0); // Load tickets from PlayerPrefs
        UpdateUIText(); // Update UI text when loading coins and tickets
    }

    private void Start()
    {
        LoadData(); // Load data when the script starts
        SceneManager.sceneLoaded += OnSceneLoaded; // Register scene load event handler
        StartCoroutine(GenerateCoins()); // Start the coin generation coroutine
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (coinText != null && ticketText != null)
        {
            coinText.gameObject.SetActive(true); // Ensure TMP text is active when the scene is loaded
            ticketText.gameObject.SetActive(true); // Ensure TMP text is active when the scene is loaded
        }

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
        System.Random random = new System.Random();

        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (isGeneratingCoins)
            {
                totalCoins += coinsPerSecond; // Increment total coins if coin generation is enabled

                // Check for ticket generation
                if (random.NextDouble() < ticketGenerationChance)
                {
                    totalTickets++; // Increment total tickets if ticket is generated
                }

                UpdateUIText(); // Update UI text for coins and tickets
                SaveData(); // Save data after each increment
            }
        }
    }

    // Update the UI text displaying the coins and tickets
    private void UpdateUIText()
    {
        if (coinText != null)
        {
            coinText.text = "" + totalCoins.ToString(); // Update UI text with coins prefix
        }

        if (ticketText != null)
        {
            ticketText.text = "" + totalTickets.ToString(); // Update UI text with tickets prefix
        }
    }
}
