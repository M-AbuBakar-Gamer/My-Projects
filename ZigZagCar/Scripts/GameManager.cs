using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGamestarted;
    public GameObject Platformspawner;

    [Header("GameOver")]
    public GameObject Gameoverpannel;
    public GameObject newhighscoreimage;
    public Text LAS;

    [Header("Score")]
    public Text scoretext;
    public Text Besttext;
    public Text diamondtext;
     public Text startext;
    int score = 0;
    int bestscore, totaldiamond, totalstar;
    int newDiam;
    int newStar;
    bool countscore;
    int selectedcar = 0;

    [Header("ForPlayer")]
    public GameObject[] Player;
    Vector3 Playerpos = new Vector3(0 ,2, 0);

    public AudioSource aS;
    public AudioClip BGM;


    public float[] scoreMultipliers = { 1.0f, 1.2f, 1.5f, 1.8f, 2.0f, 2.2f, 2.5f, 3.0f };


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        selectedcar = PlayerPrefs.GetInt("Scar");
        Instantiate(Player[selectedcar], Playerpos, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        aS.clip = BGM;
        aS.Play();

        totaldiamond = PlayerPrefs.GetInt("totalDiamond");
        diamondtext.text = totaldiamond.ToString();
        totalstar = PlayerPrefs.GetInt("totalStar");
        startext.text = totalstar.ToString();
        bestscore =  PlayerPrefs.GetInt("bestScore");
        Besttext.text = "BestScore " + bestscore.ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamestarted)
        {
            if (Input.GetMouseButton(0))
            {
                Gamestart();
            }
        }
    }
    public void Gamestart()
    {
        isGamestarted = true;
        countscore = true;
        StartCoroutine(UpdateScore());
        Platformspawner.SetActive(true);
    }
    public void Gameover()
    {
        aS.Stop();
        Gameoverpannel.SetActive(true);
        Platformspawner.SetActive(false);
        LAS.text=score.ToString();
        countscore = false;
        if (score > bestscore)
        {
            PlayerPrefs.SetInt("bestScore",score);
            newhighscoreimage.SetActive(true);
        }
        
    }
    IEnumerator UpdateScore()
    {
        while (countscore)
        {
            yield return new WaitForSeconds(.5f);
            // Multiply the score by the current car's multiplier
            score += Mathf.RoundToInt(scoreMultipliers[selectedcar]);

            if (score > bestscore)
            {
                scoretext.text = score.ToString();
                Besttext.text = "BestScore " + score.ToString();
            }
            else
            {
                scoretext.text = score.ToString();
            }
        }
    }
    public void Reply()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Getstar()
    {
        SoundManager.sm.starSound();
        totalstar++;
        newStar = totalstar;
        PlayerPrefs.SetInt("totalStar", newStar);
        startext.text = totalstar.ToString();
    }
    public void GetDiamond()
    {
        SoundManager.sm.DiamondSound();
        totaldiamond++;
        newDiam = totaldiamond;
        PlayerPrefs.SetInt("totalDiamond", newDiam);
        diamondtext.text = totaldiamond.ToString();
    }
    public void Home()
    {
        SceneManager.LoadScene("ChooseCar");
    }
}
