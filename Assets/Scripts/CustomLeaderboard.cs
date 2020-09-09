using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CustomLeaderboard : MonoBehaviour
{
    [ Header ("LeaderBoard Customisation")]
    [Space (10)]
    [Tooltip("Sorting in ascending order or descending order")]
    public bool isAscending = true;
    [Range(-1, 100)]
    [Tooltip("Maximum number of players to display, -1 is for unlimited")]
    public int maxPlayerToDisplay = -1;

    [Range(1, 10)]
    [Tooltip("Number of decimals to display")]
    public int numberOfDecimals;


    [Header ("Project Variables")]
    [Space (20)]
    public Button sortingButton, maxPlayerButton, fetchButton;
    public ClientAPI client;
    public GameObject playerListPanel;
    public GameObject playerViewPrefab;
    public PlayerFormView playerForm;
    public PlayerDatabase playerDatabase;
    public string lastPlayerSent;
    public Transform canvasParent;
    public TextMeshProUGUI titleText;

    public GameObject playerViewTest;

    private Transform contentParent;
    private List<PlayerView> playerViews = new List<PlayerView>();
    private List<Player> currentPlayers, temporaryList;

    // Start is called before the first frame update
    void Start()
    {
        // Initializes the input fields and tells the views where to instantiate
        InitializeFormView(); 
        CreateListView();

        // I'm fetching players on startup in case the user wants to only see the leaderboard without him
        RequestPlayers();

        // Assigning each function to its button
        sortingButton.onClick.AddListener(() => ChangeSortingOrder());
        maxPlayerButton.onClick.AddListener(() => ChangeMaxPlayerDisplayed());
        fetchButton.onClick.AddListener(() => RequestPlayers());
        fetchButton.onClick.AddListener(() => TitleTextAction());
    }

    // Is called if the player fetches the rankings without having submitted one himself
    private void TitleTextAction()
    {
        titleText.text = "Ranks fetched !";
        titleText.color = Color.green;
    }

    #region Startup functions
    private void InitializeFormView()
    {        
        playerForm.InitFormView(SendScoreRequest, SendDeleteRequest);
    }

    private void CreateListView()
    {        
        contentParent = playerListPanel.GetComponentInChildren<ScrollRect>().content;
    }
    #endregion

    #region Client requests 
    // Ask the client if the user can send his score
    private void SendScoreRequest(PlayerRequestData data)
    {
        // If the client agrees, the callback OnDataReceived is called
        client.PostRequest(client.postUrl, data, result => {
            OnDataReceived(result);
        });
    }

    // Ask the client if the user can delete his score
    private void SendDeleteRequest(PlayerRequestData data)
    {
        // If the client agrees, the callback OnDataReceived is called
        client.PostRequest(client.deleteUrl, data, result => {
            OnDataReceived(result);
        });
    }

    // Ask the client if we can retrieve players' rankings and scores
    private void RequestPlayers()
    {
        client.GetRequest(client.getUrl, result => {
            Debug.Log(result);
            OnDataReceived(result);
        });
    }

    // In case everything went well, this function fills the local database with the Json responses that have been converted to Player type, and then calls CreatePlayerViews
    private void OnDataReceived(string json)
    {
        var receivedPlayers = JsonHelper.FromJson<Player>(json);
        playerDatabase.ClearDatabase();

        foreach (var player in receivedPlayers)
        {
            playerDatabase.Add(player);
        }

        CreatePlayerViews(maxPlayerToDisplay);
    }

    #endregion

    // Create the players views which will fill the Scroll View content
    private void CreatePlayerViews(int maxPlayer)
    {
        // Get players from the database into a List<Player>
        currentPlayers = playerDatabase.GetPlayers();        

        // Using the Linq system, the list is ordered either from lowest score to highest or vice versa, depending of the value of "isAscending"
        currentPlayers = currentPlayers.OrderBy(w => w.userScore).ToList();
        if (isAscending)
        {
            currentPlayers.Reverse();
        }

        // Depending of the value of "maxPlayer", the list of players is partially copied into another smaller temporary list
        if (maxPlayer != -1)
        {
            if (maxPlayer > currentPlayers.Count) maxPlayer = currentPlayers.Count;

            temporaryList = new List<Player>();

            for (int i = 0; i < maxPlayer; i++)
            {
                temporaryList.Add(currentPlayers[i]);
            }
            currentPlayers = new List<Player>(temporaryList);
            temporaryList.Clear();

        }

        // Old views are destroyed to make room for the fresh new ones
        if (playerViews.Count > 0)
        {
            foreach (var player in playerViews)
            {
                Destroy(player.gameObject);
            }
            playerViews.Clear();
        }

        // The rank is initialized
        int rank = 1;

        // Create player views from the list that has been copied from the database
        foreach (var player in currentPlayers)
        {
            var playerViewInstanciation = Instantiate(playerViewPrefab, contentParent) as GameObject;

            // Change the color of the view corresponding to the last player input, allowing him to see himself more clearly, then call DisplayCheeringMessage
            if (lastPlayerSent.Equals(player.userId))
            {
                playerViewInstanciation.gameObject.GetComponent<Image>().color = new Color32(255, 144, 146,255);

                // I though that would be more logical that DisplayCheeringMessage only works in ascending order, as it seems weird to me to say something like "You need to be down X points to beat the player ahead of you"
                if (isAscending == true)
                {
                    DisplayCheeringMessage(lastPlayerSent, currentPlayers, rank);
                }
            }           

            var playerView = playerViewInstanciation.GetComponent<PlayerView>();
            playerView.InitView(player, rank, numberOfDecimals);
            playerViews.Add(playerView);       
            
            rank += 1; 
        }
    }

    // This functions displays the score gap between the last user and the user just above him as well as the first user.
    private void DisplayCheeringMessage(string lastPlayer, List<Player> players, int rank)
    {        
        float firstPlayerScore = players[0].userScore;
        titleText.fontSize = 40;

        if (players.Count > 1)
        {
            switch (rank)
            {
                case 1:
                    titleText.text = " Congratulations ! \n You are ranked first and have absolutely dominated everyone, the player in the second place needs to score "
                    + (players[0].userScore + 1 - players[1].userScore).ToString()
                    + " points to catch up to you !";
                    break;

                case 2:
                    titleText.text = " Congratulations ! \n You are ranked second and you need to score "
                    + (players[0].userScore + 1 - players[1].userScore).ToString() 
                    + " points to catch up to the first player!";
                    break;

                default :
                    float scoreToBeat = players[rank - 2].userScore;
                    float scoreRequiredToBeat = scoreToBeat + 1 - players[rank - 1].userScore;                
                    titleText.text = " Congratulations ! \n You are ranked " 
                    + rank.ToString() + " and you need " 
                    + scoreRequiredToBeat.ToString() 
                    + " more points to beat the player just ahead of you, and "
                    + (firstPlayerScore + 1 - players[rank - 1].userScore).ToString() 
                    + " to beat the first player !";
                    break;
            }
        }
        else
        {
            titleText.text = " Congratulations ! \n You are ranked first because no one else has submitted a score";            
        }
    }

    #region Button functions
    // Function of the sortingButton button 
    private void ChangeSortingOrder()
    {
        if (isAscending)
        {
            isAscending = false;
            sortingButton.GetComponentInChildren<Text>().text = "Switch to descending Order";
            CreatePlayerViews(maxPlayerToDisplay);
        }
        else
        {
            isAscending = true;
            sortingButton.GetComponentInChildren<Text>().text = "Switch to ascending Order";
            CreatePlayerViews(maxPlayerToDisplay);
        }
    }

    // Function of the maxPlayerButton button
    private void ChangeMaxPlayerDisplayed()
    {
        switch (maxPlayerToDisplay)
        {
            case -1:
                maxPlayerToDisplay = 10;
                maxPlayerButton.GetComponentInChildren<Text>().text = "Switch to Top 50";
                break;
            case 10:
                maxPlayerToDisplay = 50;
                maxPlayerButton.GetComponentInChildren<Text>().text = "Switch to Top 100";
                break;
            case 50:
                maxPlayerToDisplay = 100;
                maxPlayerButton.GetComponentInChildren<Text>().text = "Switch to unlimited";
                break;
            case 100:
                maxPlayerToDisplay = -1;
                maxPlayerButton.GetComponentInChildren<Text>().text = "Switch to Top 10";
                break;
        }
        // After changing the maximu number of players, new view are created accordingly
        CreatePlayerViews(maxPlayerToDisplay);
    }

    #endregion
}
