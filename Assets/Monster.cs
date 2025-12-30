using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterHand monsterHand;

    public int monsterDamage;

    private void Start()
    {
        monsterHand.damage = monsterDamage;
    }
}
