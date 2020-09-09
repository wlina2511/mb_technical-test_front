using UnityEngine;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public GameObject mainMenuCanvas, rankDisplayCanvas;
    public TextMeshProUGUI titleText;

    private Color titleTextColor;


    // Start is called before the first frame update
    void Start()
    {
        titleTextColor = titleText.color;
    }

    #region Button Function
    // Functions to switch between canvases 

    public void BackToMenu()
    {
        rankDisplayCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        titleText.text = "Send your score and click on 'Fetch Ranks' to get started !";
        titleText.color = titleTextColor;
    }

    public void ToRanks()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        rankDisplayCanvas.gameObject.SetActive(true);
    }

    #endregion

}
