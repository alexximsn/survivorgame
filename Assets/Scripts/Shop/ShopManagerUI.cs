using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform playerStatsClosePanel;
    private Vector2 playerStatsOpenedPos;
    private Vector2 playerStatsClosedPos;

    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryClosePanel;
    private Vector2 inventoryOpenedPos;
    private Vector2 inventoryClosedPos;


    [SerializeField] private RectTransform itemInfoSliderPanel;
    private Vector2 itemInfoSliderOpenedPos;
    private Vector2 itemInfoSliderClosedPos;


    IEnumerator Start()
    {
        yield return null;
        ConfigurePlayerStatsPanel();
        ConfigureInventoryPanel();
        ConfigureitemInfoSlider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4*playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);
        playerStatsOpenedPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos+Vector2.left*width;
        playerStatsPanel.anchoredPosition = playerStatsClosedPos;
        HidePlayerStats();
    }
   
    public void ShowPlayerStats()
    {

        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.gameObject.SetActive(true);
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = true;


        LeanTween.cancel(playerStatsPanel);
       LeanTween.move(playerStatsPanel, playerStatsOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);
        LeanTween.cancel(playerStatsClosePanel);
  
        LeanTween.alpha(playerStatsClosePanel, 0.8f, .5f).setRecursive(false);
    } 
    public void HidePlayerStats()
    {

        playerStatsClosePanel.GetComponent<Image>().raycastTarget = false;
        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsClosedPos, .5f).
            setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(()=>playerStatsPanel.gameObject.SetActive(false));
        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0, .5f).setRecursive(false)
            .setOnComplete(()=>playerStatsClosePanel.gameObject.SetActive(false)); 
    }
    private void ConfigureInventoryPanel()
    {
        float width = Screen.width / (4 * inventoryPanel.lossyScale.x);
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);
        inventoryOpenedPos = inventoryPanel.anchoredPosition;
        inventoryClosedPos = inventoryOpenedPos - Vector2.left * width;

        inventoryPanel.anchoredPosition = inventoryClosedPos;
        HideInventory(false);
    }
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryClosePanel.gameObject.SetActive(true);
        inventoryClosePanel.GetComponent<Image>().raycastTarget = true;


        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);
        LeanTween.cancel(inventoryClosePanel);

        LeanTween.alpha(inventoryClosePanel, 0.8f, .5f).setRecursive(false);
    }
    public void HideInventory(bool hideItemInfo=true)
    {
        inventoryClosePanel.GetComponent<Image>().raycastTarget = false;
        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryClosedPos, .5f).
            setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => inventoryPanel.gameObject.SetActive(false));
        LeanTween.cancel(inventoryClosePanel);
        LeanTween.alpha(inventoryClosePanel, 0, .5f).setRecursive(false)
            .setOnComplete(() => inventoryClosePanel.gameObject.SetActive(false));
        if(hideItemInfo)
        HideItemInfo();
    }

    private void ConfigureitemInfoSlider()
    {
        float height = Screen.height / (2 * itemInfoSliderPanel.lossyScale.x);
        itemInfoSliderPanel.offsetMax = itemInfoSliderPanel.offsetMax.With(y: height);
        itemInfoSliderOpenedPos = itemInfoSliderPanel.anchoredPosition;
        itemInfoSliderClosedPos = itemInfoSliderClosedPos + Vector2.down * height;
        itemInfoSliderPanel.anchoredPosition = itemInfoSliderClosedPos;
        itemInfoSliderPanel.gameObject.SetActive(false);
    }
    public void ShowItemInfo()
    {
        itemInfoSliderPanel.gameObject.SetActive(true);
        itemInfoSliderPanel.LeanCancel();
        itemInfoSliderPanel.LeanMove((Vector3)itemInfoSliderOpenedPos, .3f).setEase(LeanTweenType.easeOutCubic);
    }
    public void HideItemInfo()
    {
        itemInfoSliderPanel.LeanCancel();
        itemInfoSliderPanel.LeanMove((Vector3)itemInfoSliderClosedPos, .3f).
            setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => itemInfoSliderPanel.gameObject.SetActive(false));
    }
}
