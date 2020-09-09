using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class PlayerFormView : MonoBehaviour
{
    public TMP_InputField userIdField;
    public TMP_InputField userScoreField;
    public Button sendScoreButton, deleteScoreButton;
    public GameObject leaderBoard;


    // Initialize the form buttons with the corresponding function
    public void InitFormView(System.Action<PlayerRequestData> callback, System.Action<PlayerRequestData> callback2)
    {
        sendScoreButton.onClick.AddListener(() =>
        {
            OnSendClicked(callback);
        });
        deleteScoreButton.onClick.AddListener(() =>
        {
            OnSendClicked(callback2);
        });
    }

    public void OnSendClicked(System.Action<PlayerRequestData> callback)
    {
        // If inputs are valid, a new PlayerRequestData entry is created, and lastPlayerSent is filled in CustomLeaderboard
        if (InputsAreValid())
        {
            var player = new PlayerRequestData(userIdField.text, float.Parse(userScoreField.text, CultureInfo.InvariantCulture));
            leaderBoard.GetComponent<CustomLeaderboard>().lastPlayerSent = userIdField.text;
            callback(player);
        }
        else
        {
            Debug.LogWarning("Invalid Input");
        }
    }

    //  Chek if the input are not empty nor null 
    private bool InputsAreValid()
    {
        return (!string.IsNullOrEmpty(userIdField.text) || !string.IsNullOrEmpty(userScoreField.text));
    }
}
