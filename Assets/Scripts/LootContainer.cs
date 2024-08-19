using UnityEngine;

public class LootContainer : MonoBehaviour
{
    [SerializeField] GameObject Loot;

    public void DumpLoot()
    {
        if (Loot != null) 
        {
            GameObject bullet = Instantiate(Loot, this.transform.position, Quaternion.identity);
        }
    }
}
