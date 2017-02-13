using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public static CharacterManager instance;

    public List<Character> characters;
    public List<Bullet> bullets;
    public float minSpeedForKill;
    public float dotForFootHit;
    public float maxDimensionalShiftTime;
    public float timeToShowMax;
    public float jumpForce;
    public bool useMaxTime;
    public CharacterSprites[] characterSprites;

    private void Awake()
    {
        instance = this;
    }

    public void RegisterCharacter(Character character)
    {
        characters.Add(character);
    }

    public void UnregisterCharacter(Character character)
    {
        characters.Remove(character);
    }

    public void RegisterBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }

    public void UnregisterBullet(Bullet bullet)
    {
        bullets.Remove(bullet);
    }

    public void Cleanup()
    {
        List<Character> toDestroy = new List<Character>(characters);

        for (int i = 0; i < toDestroy.Count; i++)
            Destroy(toDestroy[i].gameObject);

        characters = new List<Character>();

        List<Bullet> toDestroy2 = new List<Bullet>(bullets);

        for (int i = 0; i < toDestroy2.Count; i++)
            Destroy(toDestroy2[i].gameObject);

        bullets = new List<Bullet>();

    }
}

[System.Serializable]
public class CharacterSprites
{
    public Sprite avatar;
    public Sprite[] dimensionSprites;
    public Sprite[] gunSprites;
}
