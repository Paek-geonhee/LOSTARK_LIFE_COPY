using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryPanel;

    public TMP_Text[][] ItemCounts;



    private bool invOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invOn = false;
        ItemCounts = new TMP_Text[][] { 
            GameObject.Find("MeatGroup").GetComponentsInChildren<TMP_Text>(),
            GameObject.Find("PlantGroup").GetComponentsInChildren<TMP_Text>(),
            GameObject.Find("TreeGroup").GetComponentsInChildren<TMP_Text>(),
            GameObject.Find("StoneGroup").GetComponentsInChildren < TMP_Text >(),
            GameObject.Find("TreasureGroup").GetComponentsInChildren < TMP_Text >(),
            GameObject.Find("FishGroup").GetComponentsInChildren < TMP_Text >()};
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        UIHandler();
    }

    void UIHandler() {

        // 플레이어가 I 키를 누를 경우 인벤토리의 활성 상태를 변경함.

        InventoryPanel.SetActive(invOn);
        if (Input.GetKeyDown(KeyCode.I)) {
            invOn = !invOn;
        }
    }

    void UIUpdate()
    {

        // 인벤토리의 활성 여부와 관계없이 리소스의 개수는 상시 갱신함.

        for (int i = 0; i < 6; i++)
        {

            ItemCounts[i][0].SetText(GameManager.Instance.ItemClass[i].normalCount.ToString());
            ItemCounts[i][1].SetText(GameManager.Instance.ItemClass[i].rareCount.ToString());
            ItemCounts[i][2].SetText(GameManager.Instance.ItemClass[i].epicCount.ToString());

        }
    }
}
