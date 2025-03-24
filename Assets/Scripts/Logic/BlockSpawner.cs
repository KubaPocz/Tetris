using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject[] spawnPoints;

    private GameObject spawnedBlock;
    private TetrisController spawnedBlockTetrisController;
    public GridManager[] gridManager;
    private void Awake()
    {
        
    }
    void Start()
    {
        gridManager[0].nextBlock = blocks[Random.Range(0, blocks.Length)];
        gridManager[1].nextBlock = blocks[Random.Range(0, blocks.Length)];

        SpawnBlock(1);
        SpawnBlock(2);
        Invoke(nameof(SendNextBlockUpdate), 0.5f);
    }

    private void SendNextBlockUpdate()
    {
        Events.UpdateNextBlock(1, gridManager[0].nextBlock);
        Events.UpdateNextBlock(2, gridManager[1].nextBlock);
    }
    public void SpawnBlock(int playerId)
    {
        GameObject block;
        switch (playerId)
        {
            case 1:
                block = gridManager[playerId-1].nextBlock;
                spawnedBlock = Instantiate(block, spawnPoints[playerId-1].transform.position,Quaternion.identity);
                spawnedBlock.GetComponent<TetrisController>().Initialize(playerId);
                gridManager[0].nextBlock = blocks[Random.Range(0, blocks.Length)];
                Events.UpdateNextBlock(playerId, gridManager[playerId-1].nextBlock);
                break;
            case 2:
                block = gridManager[playerId-1].nextBlock;
                spawnedBlock = Instantiate(block, spawnPoints[playerId-1].transform.position, Quaternion.identity);
                spawnedBlock.GetComponent<TetrisController>().Initialize(playerId);
                gridManager[1].nextBlock = blocks[Random.Range(0, blocks.Length)];
                Events.UpdateNextBlock(playerId, gridManager[playerId-1].nextBlock);
                break;
            default:
                break;
        }
    }
}
