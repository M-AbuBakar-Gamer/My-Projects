using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCar : MonoBehaviour
{
    public static SelectCar Instance;
    [SerializeField] Button Prev;
    [SerializeField] Button Next;
    [SerializeField] Button use;
    [SerializeField] GameObject Pannel;

    int hstar, hdiamond;
    int[] carValues = { 0, 500, 650, 800, 1000, 1200, 1300, 1500 }; // Add prices for each car

    [Header("Buy Pannel")]
    public Text Havestar;
    public Text Havediamond;
    public Text Need;
    public Button Buy;
    public Button Close;
    public Button Buystard;

    int currentcar;
    string owncarindex;
    Color redcolor = new Color(1f, 0.1f, 0.1f, 1f);
    Color greencolor = new Color(.5f, 1f, 0.4f, 1f);

    private void Start()
    {
        hstar = PlayerPrefs.GetInt("totalStar");
        hdiamond = PlayerPrefs.GetInt("totalDiamond");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Changecar(0);
    }

    void Choosecar(int ind)
    {
        Prev.interactable = (ind != 0);
        Next.interactable = (ind != transform.childCount - 1);
        for (int i = 0; i < transform.childCount; i++)
        {
            string carNo = "CarNo" + i;
            if (i == 0)
            {
                PlayerPrefs.SetInt(carNo, 1);
            }
            transform.GetChild(i).gameObject.SetActive(i == ind);
        }
    }

    public void Changecar(int car)
    {
        currentcar += car;
        Choosecar(currentcar);
        owncarindex = "CarNo" + currentcar;
        if (PlayerPrefs.GetInt(owncarindex) == 1)
        {
            use.GetComponent<Image>().color = greencolor;
            use.GetComponentInChildren<Text>().text = "SELECT";
        }
        else
        {
            use.GetComponent<Image>().color = redcolor;
            use.GetComponentInChildren<Text>().text = "BUY";
        }
    }

    public void useclick()
    {
        hstar = PlayerPrefs.GetInt("totalStar");
        hdiamond = PlayerPrefs.GetInt("totalDiamond");
        if (PlayerPrefs.GetInt(owncarindex) == 1)
        {
            PlayerPrefs.SetInt("Scar", currentcar);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            settext();
        }
    }

    public void Closepannel()
    {
        Pannel.SetActive(false);
        Prev.interactable = true;
        Next.interactable = true;
        use.interactable = true;
    }

    public void buystar()
    {
        hstar = hstar + 10;
        hdiamond = hdiamond - 1;
        if (hdiamond <= 0)
        {
            Buystard.interactable = false;
        }

        Prev.interactable = false;
        Next.interactable = false;
        use.interactable = false;
        PlayerPrefs.SetInt("totalStar", hstar);
        PlayerPrefs.SetInt("totalDiamond", hdiamond);
        settext();
    }

    public void settext()
    {
        Pannel.SetActive(true);

        Havestar.text = hstar.ToString();
        Havediamond.text = hdiamond.ToString();

        if (hstar < carValues[currentcar])
        {
            int needstar = carValues[currentcar] - hstar;
            Buy.interactable = false;
           
            Need.text = needstar + " More Star Needed";
            
        }
        else
        {
            Buy.interactable = true;
           
            Need.text = "Value: " + carValues[currentcar] + " Stars";
         
        }

        if (hdiamond <= 0)
        {
            Buystard.interactable = false;
        }
        Prev.interactable = false;
        Next.interactable = false;
        use.interactable = false;
    }

    public void Buycar()
    {
        PlayerPrefs.SetInt(owncarindex, 1);
        hstar -= carValues[currentcar];
        PlayerPrefs.SetInt("totalStar", hstar);
        int currentMinOne = currentcar - 1;
        Changecar(currentMinOne);
        Closepannel();
    }
}
