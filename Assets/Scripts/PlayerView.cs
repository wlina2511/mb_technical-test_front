using UnityEngine;
using TMPro;
using System;


// Fill the player view with the corresponding attributes
public class PlayerView : MonoBehaviour
{
    public TextMeshProUGUI userIdText, userScoreText, userRankText;
    public void InitView(Player player, int rank, int numberOfDecimals)
    {
        userIdText.text = player.userId.ToString();        
        userScoreText.text = Math.Round(player.userScore,numberOfDecimals).ToString();
        userRankText.text = rank.ToString();
    }
}
