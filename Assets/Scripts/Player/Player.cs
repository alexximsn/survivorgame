using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerHealth),typeof(Playerlever))]
public class Player : MonoBehaviour
{
    public static Player instance;
    private PlayerHealth playerHealth;
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private Animator playerRendererAnimator;
    private Playerlever playerLevel;
   private void Awake()
   {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerHealth=GetComponent<PlayerHealth>();
        playerLevel = GetComponent<Playerlever>();

        CharacterSelectionManager.onCharacterSelected += CharacterSelectedCallback;
    }

    private void CharacterSelectedCallback(CharacterDataSO characterData)
    {
        playerRenderer.sprite = characterData.Sprite;
        if (characterData.AnimOverrideController != null && playerRendererAnimator != null)
        {
            playerRendererAnimator.runtimeAnimatorController = characterData.AnimOverrideController;
        }
    }

    private void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallback;
    }
    public void TakeDamage(int damage)
    {
      playerHealth.TakeDamage(damage);
    }
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;//返回玩家碰撞的中心，以确定射击位置
    }
    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
