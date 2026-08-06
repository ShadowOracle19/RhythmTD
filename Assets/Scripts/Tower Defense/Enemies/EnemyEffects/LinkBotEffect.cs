using UnityEngine;

public class LinkBotEffect : MonoBehaviour
{
    public GameObject linkBotSummon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SummonLinkBots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonLinkBots()
    {
        for (int i = 0; i <= 4; i++)
        {
            Spawner.Instance.ForceEnemySpawnOnTile(new Vector3(transform.position.x + i, transform.position.y, transform.position.z), linkBotSummon);
        }
        
    }
}
