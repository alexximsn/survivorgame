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
    IEnumerator Start()
    {
        yield return null;
        ConfigurePlayerStatsPanel();
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
        Debug.Log($"Opened Position: {playerStatsOpenedPos}, Closed Position: {playerStatsClosedPos}");
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
}
