using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ClientAPI : MonoBehaviour
{
    [Header ("Custom urls pointing towards different routes")]
    public string getUrl = "localhost:3000/player";
    public string postUrl = "localhost:3000/player/create";
    public string deleteUrl = "localhost:3000/player/delete";

    [Header("Project Variables")]
    public CustomLeaderboard customLeaderboard;



    
    #region GET functions

    // GetRequest is called from the CustomLeaderboard script, and in turn calls the Get Coroutine
    // This method is used to retrieve players
    public void GetRequest(string url, System.Action<string> callback)
    {
        StartCoroutine(Get(url, callback));
    }

    public IEnumerator Get(string url,System.Action<string> callback)
    {
        // Check if the urls are matching
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Send the web request
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // If everything went well :
                if (www.isDone)
                {
                    // Handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    //Format into json to be able to work with JsonUtil 
                    result = "{\"result\":" + result + "}";

                    // Execute the callback function after everything is done, with the Json result as parameter
                    callback(result);
                }
                else
                {
                    //Handle the error (Even though I'm only "printing" error, this prevents the function from going sideways if the www request encountered a problem)
                    Debug.Log("Error! Couldn't get data.");
                }
            }
        }        
    }

    #endregion

    #region POST functions

    // PostRequest is called from the CustomLeaderboard script, and in turn calls the Post Coroutine
    // This method is used to either create, delete or update a player
    public void PostRequest(string url, PlayerRequestData data, System.Action<string> callback)
    {
        StartCoroutine(Post(url, data, callback));
    }

    public IEnumerator Post(string url, PlayerRequestData data, System.Action<string> callback)
    {
        var jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);

        // Check if the urls are matching
        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

            // Send the web request
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // If everything went well :
                if (www.isDone)
                {
                    // Handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    //Format into json to be able to work with JsonUtil 
                    result = "{\"result\":" + result + "}";

                    // Execute the callback function after everything is done, with the Json result as parameter
                    callback(result);
                }
                else
                {
                    //Handle the error (Even though I'm only "printing" error, this prevents the function from going sideways if the www request encountered a problem)
                    Debug.Log("Error! Couldn't post data.");
                }
            }
        }
    }
    #endregion
}
