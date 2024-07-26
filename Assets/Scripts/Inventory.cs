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

        // �÷��̾ I Ű�� ���� ��� �κ��丮�� Ȱ�� ���¸� ������.

        InventoryPanel.SetActive(invOn);
        if (Input.GetKeyDown(KeyCode.I)) {
            invOn = !invOn;
        }
    }

    void UIUpdate()
    {

        // �κ��丮�� Ȱ�� ���ο� ������� ���ҽ��� ������ ��� ������.

        for (int i = 0; i < 6; i++)
        {

            ItemCounts[i][0].SetText(GameManager.Instance.ItemClass[i].normalCount.ToString());
            ItemCounts[i][1].SetText(GameManager.Instance.ItemClass[i].rareCount.ToString());
            ItemCounts[i][2].SetText(GameManager.Instance.ItemClass[i].epicCount.ToString());

        }
    }
}
