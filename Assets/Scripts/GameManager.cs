using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform shipParent;

    public bool gameStarted { get; private set; } = false;

    [SerializeField] List<GameObject> objectsToDisable = new();
    [SerializeField] List<GameObject> objectsToEnable = new();

    [SerializeField] PartsManager partsManager;

    [SerializeField] GameObject endCanvas;

    Dictionary<string, int> harvestedRessources = new();

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        endCanvas.SetActive(false);

        var items = LootEnum.GetValues(typeof(LootEnum));

        foreach (var item in items)
        {
            harvestedRessources[item.ToString()] = 0;
        }
    }

    public void StartGame(List<ShipPart> ship)
    {
        gameStarted = true;

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }

        foreach (var item in ship)
        {
            item.transform.parent = shipParent;
            item.enabled = true;
        }

        

        partsManager.partList = ship;
    }

    public static T GetClosestFromList<T>(Vector2 pos, List<T> list) where T : Behaviour
    {
        T closest = null;

        foreach (T thing in list)
        {
            if (closest == null || Vector2.Distance(pos, thing.transform.position) < Vector2.Distance(pos, closest.transform.position))
                closest = thing;
        }

        return closest;
    }

    public void EndGame()
    {
        ClaimRessources();
        endCanvas.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddRessources(LootEnum ressource)
    {
        if (harvestedRessources.ContainsKey(ressource.ToString()))
            harvestedRessources[ressource.ToString()] += 1;
        else
            harvestedRessources[ressource.ToString()] = 1;
    }

    public void ClaimRessources()
    {
        foreach (var item in harvestedRessources)
        {
            int previousAmount = PlayerPrefs.GetInt(item.Key + "Amount", 0);
            PlayerPrefs.SetInt(item.Key + "Amount", previousAmount + item.Value);            
        }
    }
}
