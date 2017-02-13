using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnstableDisplay : MonoBehaviour
{
    private Character character;
    private Image image;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        image = GetComponentInChildren<Image>();

        if (!GameStateManager.instance.currentMode.killOnNotSwitch)
            gameObject.SetActive(false);
    }
    
	void Update ()
    {
        image.fillAmount = 1f - (character.timePassedInDimension / CharacterManager.instance.maxDimensionalShiftTime);
            //image.color = DimensionConfig.instance.dimensions[character.currentDimension].color;
        
	}
}
