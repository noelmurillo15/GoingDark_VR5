using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerViewCheck : MonoBehaviour
{

    private float fov;
    private Vector3 rayDirection;
    private GameObject[] stars;
    private GameObject LevelMenu, shop, curStar;
    private Text[] texts;
    private Text acceptButton;
    public float delayTimer;
    public bool isSwitching;
    private ShopMenu shopMenu;

    // Use this for initialization
    void Start()
    {
        delayTimer = 2.0f;
        fov = transform.GetComponent<Camera>().fieldOfView / 5;
        rayDirection = Vector3.zero;
        LevelMenu = GameObject.Find("LevelMenu");
        stars = GameObject.FindGameObjectsWithTag("Star");
        curStar = null;
        shop = GameObject.Find("Shop");
        shopMenu = shop.GetComponent<ShopMenu>();
        texts = LevelMenu.GetComponentsInChildren<Text>();
        acceptButton = GameObject.Find("Accept").GetComponentInChildren<Text>();
        isSwitching = false;

        LevelMenu.SetActive(false);
        shop.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        IsInSight();
        if (isSwitching)
        {
            LevelMenu.SetActive(false);
            shop.SetActive(false);
            SwitchScene();
        }
    }

    void SwitchScene()
    {
        switch (curStar.name)
        {
            case "Level1":
                SceneManager.LoadScene("Level1");
                break;
            case "Level2":
                SceneManager.LoadScene("Level2");
                break;
            case "Level3":
                SceneManager.LoadScene("Level3");
                break;
            case "Level4":
                SceneManager.LoadScene("Level4");
                break;
        }
    }

    public void IsInSight()
    {
        RaycastHit hit;
        for (int i = 0; i < stars.Length; i++)
        {
            rayDirection = stars[i].transform.position - transform.position;
            if (Vector3.Angle(rayDirection, transform.forward) <= fov)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {
                    if (hit.collider.name == "Earth")
                    {
                        shop.SetActive(true);
                        LevelMenu.SetActive(false);
                        return;
                    }
                    else
                    {
                        LevelMenu.SetActive(true);
                        if (shop.activeSelf)
                        {
                            shopMenu.SendMessage("Back");
                            shop.SetActive(false);
                        }
                        MissionText(stars[i].name);
                        if (stars[i].GetComponent<MapConnection>().isUnlocked == 1)
                        {
                            curStar = stars[i];
                            acceptButton.text = "Accept";
                            if (Input.GetKeyDown(KeyCode.X))
                                isSwitching = true;
                            return;
                        }
                        else
                        {
                            acceptButton.text = "Locked";
                            isSwitching = false;
                            return;
                        }
                    }
                }
            }

        }
        LevelMenu.SetActive(false);
        if (shop.activeSelf)
        {
            shopMenu.SendMessage("Back");
            shop.SetActive(false);
        }
        isSwitching = false;
    }

    private void MissionText(string missionName)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].name == missionName || texts[i].name == "Text")
            {
                texts[i].enabled = true;
            }
            else
                texts[i].enabled = false;
        }

    }
}
