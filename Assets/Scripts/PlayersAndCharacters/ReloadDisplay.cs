using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadDisplay : MonoBehaviour
{
    private CharacterAttackHandler attackHandler;
    private Character character;
    private Image image;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        attackHandler = GetComponentInParent<CharacterAttackHandler>();
        image = GetComponentInChildren<Image>();
    }
    
	void Update ()
    {
        if (attackHandler.reloading)
        {
            image.enabled = true;
            image.fillAmount = attackHandler.shotsLeft / ((float)attackHandler.clip);
            image.color = DimensionConfig.instance.dimensions[character.currentDimension].color;
        }
        else
            image.enabled = false;
	}
}
