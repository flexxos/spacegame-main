using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Demo : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private TMP_Text uiSpinButtonText;
    [SerializeField] private TMP_Text coinText; // TextMeshPro text for displaying coins
    [SerializeField] private TMP_Text ticketText; // TextMeshPro text for displaying tickets
    [SerializeField] private Button generateTicketButton; // Button to generate tickets
    [SerializeField] private Button homeButton; // Home button

    [SerializeField] private PickerWheel pickerWheel;

    private int totalCoins = 0; // Total coins collected
    private int totalTickets = 0; // Total tickets owned
    private int ticketCost = 1; // Ticket cost for spinning the wheel

    private void Start()
    {
        // Load coins from PlayerPrefs
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // Initialize text elements
        UpdateCoinText();
        UpdateTicketText();

        // Add button listeners
        uiSpinButton.onClick.AddListener(SpinWheel);
        generateTicketButton.onClick.AddListener(GenerateTicket);

        // Start the coin generation coroutine
        StartCoroutine(GenerateCoins());
    }

    private bool isSpinning = false; // Flag to indicate if the wheel is spinning

    private void SpinWheel()
    {
        if (totalTickets >= ticketCost && !isSpinning) // Check if enough tickets are available and not spinning
        {
            isSpinning = true; // Set spinning flag to true
            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "Spinning..";

            // Disable the home button
            homeButton.interactable = false;
            Color homeButtonColor = homeButton.image.color;
            homeButtonColor.a = 0.5f; // Set button transparency
            homeButton.image.color = homeButtonColor;

            // Persist the home button between scenes
            DontDestroyOnLoad(homeButton.gameObject);

            pickerWheel.OnSpinStart(() =>
            {
                Debug.Log("Spin Started..");
            });

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log("Spin end: Label:" + wheelPiece.Label + " , Amount:" + wheelPiece.Amount);
                uiSpinButton.interactable = true;
                uiSpinButtonText.text = "Spin!";
                isSpinning = false; // Reset spinning flag

                totalCoins += wheelPiece.Amount; // Increment total coins
                UpdateCoinText(); // Update UI text

                totalTickets -= ticketCost; // Deduct ticket cost
                UpdateTicketText(); // Update UI text for tickets

                // Save coins to PlayerPrefs
                PlayerPrefs.SetInt("TotalCoins", totalCoins);

                // Enable the home button
                homeButton.interactable = true;
                homeButtonColor.a = 1f; // Reset button transparency
                homeButton.image.color = homeButtonColor;
            });

            pickerWheel.Spin();
        }
        else
        {
            uiSpinButton.interactable = false;
            Debug.Log("Not enough tickets or already spinning!");
            uiSpinButtonText.text = "You don't have tickets!"; // Update spin button text

            // Start coroutine to revert text after 2 seconds
            StartCoroutine(ResetSpinButtonText());
        }
    }

    // Coroutine to reset spin button text after 2 seconds
    private IEnumerator ResetSpinButtonText()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        uiSpinButton.interactable = true;
        uiSpinButtonText.text = "Spin!"; // Reset spin button text

        // Enable home button
        homeButton.interactable = true;
        Color homeButtonColor = homeButton.image.color;
        homeButtonColor.a = 1f; // Reset button transparency
        homeButton.image.color = homeButtonColor;
    }

    // Coin generation coroutine
    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            totalCoins++; // Increment total coins
            UpdateCoinText(); // Update UI text
        }
    }

    // Update the UI text displaying the coins
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + totalCoins.ToString(); // Update UI text
        }
    }

    // Update the UI text displaying the tickets
    private void UpdateTicketText()
    {
        if (ticketText != null)
        {
            ticketText.text = "Tickets: " + totalTickets.ToString(); // Update UI text
        }
    }

    // Method to generate tickets
    private void GenerateTicket()
    {
        totalTickets++; // Increment total tickets
        UpdateTicketText(); // Update UI text
    }
}