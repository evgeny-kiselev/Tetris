using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour {
    public Button classicStartButton, advancedStartButton, exitButton;
    public GameObject classicGame, advancedGame;

    // Use this for initialization
    void Start () {
        classicStartButton.onClick.AddListener(onClassicStartClick);
        advancedStartButton.onClick.AddListener(onAdvancedStartClick);
        exitButton.onClick.AddListener(onExitStartClick);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void onClassicStartClick()
    {
        gameObject.active = false;
        Instantiate(classicGame);
    }

    public void onAdvancedStartClick()
    {
        gameObject.active = false;
        Instantiate(advancedGame);
    }

    public void onExitStartClick()
    {
        Application.Quit();
    }
}
