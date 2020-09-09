


// Our Player model
[System.Serializable]
public class Player 
{
    public string userId;
    public float userScore;

    public Player(string id, float score)
    {
        this.userId = id;
        this.userScore = score;
    }
}

// PlayerRequestData is a data holder class to contain info before a post request is sent
public class PlayerRequestData 
{
    public string userId;
    public float userScore;

    public PlayerRequestData(string id, float score)
    {
        this.userId = id;
        this.userScore = score;
    }
}
