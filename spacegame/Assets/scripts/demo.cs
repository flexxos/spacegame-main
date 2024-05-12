
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

    [SerializeField] private PickerWheel pickerWheel;

    private int totalCoins = 0; // Total coins collected
    private int totalTickets = 0; // Total tickets owned
    private int ticketCost = 1; // Ticket cost for spinning the wheel

    private void Start()
    {
        LoadData(); // Load coins and tickets when the script starts

        // Add button listeners
        uiSpinButton.onClick.AddListener(SpinWheel);
        generateTicketButton.onClick.AddListener(GenerateTicket);

    }

    private void LoadData()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Load coins from PlayerPrefs
        totalTickets = PlayerPrefs.GetInt("TotalTickets", 0); // Load tickets from PlayerPrefs
        UpdateCoinText(); // Update UI text for coins
        UpdateTicketText(); // Update UI text for tickets
    }


    private void SaveData()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins); // Save coins to PlayerPrefs
        PlayerPrefs.SetInt("TotalTickets", totalTickets); // Save tickets to PlayerPrefs
        PlayerPrefs.Save(); // Ensure PlayerPrefs data is saved immediately
    }

    private void SpinWheel()
    {
        if (totalTickets >= ticketCost) // Check if enough tickets are available
        {
            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "Spinning..";

            pickerWheel.OnSpinStart(() =>
            {
                Debug.Log("Spin Started..");
            });

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log("Spin end: Label:" + wheelPiece.Label + " , Amount:" + wheelPiece.Amount);
                uiSpinButton.interactable = true;
                uiSpinButtonText.text = "Spin!";

                totalCoins += wheelPiece.Amount; // Increment total coins
                UpdateCoinText(); // Update UI text

                totalTickets -= ticketCost; // Deduct ticket cost
                UpdateTicketText(); // Update UI text for tickets

                // Save data to PlayerPrefs
                SaveData();
            });

            pickerWheel.Spin();
        }
        else
        {
            uiSpinButton.interactable = false;
            Debug.Log("Not enough tickets!");
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
    }


    // Update the UI text displaying the coins
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "" + totalCoins.ToString(); // Update UI text
        }
    }

    // Update the UI text displaying the tickets
    private void UpdateTicketText()
    {
        if (ticketText != null)
        {
            ticketText.text = "" + totalTickets.ToString(); // Update UI text
        }
    }

    // Method to generate tickets
    private void GenerateTicket()
    {
        totalTickets++; // Increment total tickets
        UpdateTicketText(); // Update UI text
        SaveData(); // Save data after generating tickets
    }
}
