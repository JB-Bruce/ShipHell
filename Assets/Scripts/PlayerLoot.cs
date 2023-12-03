using TMPro;
using UnityEngine;

public class PlayerLoot : MonoBehaviour
{
    public LootTextStruct[] lootText;

    private void Start()
    {
        for (int i = 0; i < lootText.Length; i++)
        {
            lootText[i].lootAmount = PlayerPrefs.GetInt(lootText[i].lootEnum.ToString() + "Amount", (int)lootText[i].lootEnum);
            lootText[i].text.text = lootText[i].lootAmount.ToString();

            PlayerPrefs.SetInt(lootText[i].lootEnum.ToString() + "Amount", lootText[i].lootAmount);
        }
    }
}

[System.Serializable]
public struct LootTextStruct
{
    public LootEnum lootEnum;
    public int lootAmount;
    public TextMeshProUGUI text;
}
