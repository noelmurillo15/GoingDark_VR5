using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerViewCheck : MonoBehaviour
{

    private float fov;
    private Vector3 rayDirection;
    private GameObject[] stars;
    private GameObject LevelMenu, shop, curStar;
    private MenuTraverse LevelTraverse, shopTraverse;
    private Text[] texts;
    private Text acceptButton;
    public float delayTimer;
    public bool isSwitching;
    private ShopMenu shopMenu;

    [SerializeField]
    private GameObject[] particles;

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
        LevelTraverse = LevelMenu.GetComponent<MenuTraverse>();
        shopTraverse = shop.transform.FindChild("MainList").GetComponent<MenuTraverse>();

        isSwitching = false;

        LevelMenu.SetActive(false);
        shop.SetActive(false);
        LevelTraverse.enabled = false;
        shopTraverse.enabled = false;
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
                LoadingScreenManager.LoadScene("Level1");
                break;
            case "Level2":
                LoadingScreenManager.LoadScene("Level2");
                break;
            case "Level3":
                LoadingScreenManager.LoadScene("Level3");
                break;
            case "Level4":
                LoadingScreenManager.LoadScene("Level4");
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
                if (Physics.Raycast(transform.position, rayDirection, out hit, 1000f))
                {
                    if (hit.collider.name == "Earth")
                    {
                        particles[0].SetActive(true);
                        particles[1].SetActive(false);
                        particles[2].SetActive(false);
                        particles[3].SetActive(false);
                        particles[4].SetActive(false);
                        shop.SetActive(true);
                        shopTraverse.enabled = true;
                        LevelMenu.SetActive(false);
                        LevelTraverse.enabled = false;
                        return;
                    }
                    else
                    {
                        LevelMenu.SetActive(true);
                        LevelTraverse.enabled = true;
                        if (shop.activeSelf)
                        {
                            shopMenu.SendMessage("Back");
                            shop.SetActive(false);
                            shopTraverse.enabled = false;
                        }
                        MissionText(stars[i].name);

                        switch (hit.collider.name)
                        {
                            case "Level1":
                                particles[1].SetActive(true);
                                particles[0].SetActive(false);
                                particles[2].SetActive(false);
                                particles[3].SetActive(false);
                                particles[4].SetActive(false);
                                break;
                            case "Level2":
                                particles[2].SetActive(true);
                                particles[0].SetActive(false);
                                particles[1].SetActive(false);
                                particles[3].SetActive(false);
                                particles[4].SetActive(false);
                                break;
                            case "Level3":
                                particles[3].SetActive(true);
                                particles[0].SetActive(false);
                                particles[2].SetActive(false);
                                particles[1].SetActive(false);
                                particles[4].SetActive(false);
                                break;
                            case "Level4":
                                particles[4].SetActive(true);
                                particles[0].SetActive(false);
                                particles[2].SetActive(false);
                                particles[3].SetActive(false);
                                particles[1].SetActive(false);
                                break;
                            default:
                                break;
                        }
                      
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
                else
                {
                    for (int x = 0; x < particles.Length; x++)
                    {
                        particles[x].SetActive(false);
                    }
                }

            }
           
        }
        if (LevelMenu.activeSelf)
        {
            LevelMenu.SetActive(false);
            LevelTraverse.enabled = false;
        }

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
            {
                texts[i].enabled = false;
            }
        }

    }
}