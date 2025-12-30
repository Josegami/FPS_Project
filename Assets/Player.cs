using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player dead");

            //Game over
            //Re spawn Player
            //Diying anim
        }
        else
        {
            print("Player hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterHand"))
        {
            TakeDamage(other.gameObject.GetComponent<MonsterHand>().damage);
        }
    }
}
