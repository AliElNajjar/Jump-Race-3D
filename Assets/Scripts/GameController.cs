using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController SharedInstance;

    public GameObject player;

    public Slider progressBar;
    private float progress;
    private float progressValue;

    public AudioSource backgroundM;

    
    public Text commentText;
    
    [HideInInspector]
    public bool go = false;
    
    public GameObject startingText;
    public GameObject gameTitle;
    public GameObject levelComplete;
    public GameObject levelLost;
    
    private Animation commentAnimation;

    void Awake()
    {
        SharedInstance = this;
    }


    private void Start()
    {
        gameTitle.SetActive(true);
        startingText.SetActive(true);
        progressBar.gameObject.SetActive(false);

        commentAnimation = commentText.gameObject.GetComponent<Animation>();

        //GameIsOver = false;

        commentText.text = null;

    }


    private void Update()
    {
        /*if (GameIsOver)
            return;

        if (currentLives <=0)
        {
            EndGame();
        }*/


        if (Input.GetMouseButtonDown(0) && go == false)
        {
            startingText.SetActive(false);
            gameTitle.SetActive(false);
            progressBar.gameObject.SetActive(true);
            commentText.gameObject.SetActive(true);
            go = true;
        }

    }
    public void UpdateProgressUI(float newProgress)
    {
        if (progress >= newProgress)
            progressValue = progress;
        else
        {
            progress = newProgress;
            progressValue = newProgress; 
        }

        progressBar.value = progressValue;

        /*if (progressValue >= 1)
        {
            CompleteLevel();
        }*/
    }
    public void JumpComment(string state)
    {
        StartCoroutine(DisplayComment(state));
    }

    public IEnumerator DisplayComment(string comment)
    {


        switch (comment)
        {
            case "longJump":
                commentText.text = "Long Jump!";
                commentText.color = Color.magenta;
                break;
            case "bullsEye":
                commentText.text = "PERFECT";
                commentText.color = Color.green;
                break;
            case "normalJump":
                commentText.text = "GOOD";
                commentText.color = Color.yellow;
                break;
            default:
                commentText.text = null;
                break;
        }

        commentAnimation.Play("commentAppear");

        yield return new WaitForSeconds(1f);

        commentText.text = null;
    }

    public void CompleteLevel()
    {
        go = false;

        levelComplete.gameObject.SetActive(true);
        commentText.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        backgroundM.Stop();

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //fireworks

        player.GetComponent<Animator>().Play("Wave");



    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelLost()
    {
        go = false;

        levelLost.gameObject.SetActive(true);
        commentText.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        
    }

    /*public void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);

    }*/

}
