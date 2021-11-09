using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BlockPool : MonoBehaviour
{
    [SerializeField] private Block pBlock;
    [SerializeField] private Player playerMove;
    private List<Block> blockPool = new List<Block>();
    private List<Block> activeBlockPool = new List<Block>();
    private int height = 1;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            var block = Instantiate(pBlock);
            block.gameObject.SetActive(false);
            blockPool.Add(block);
        }
        for(int i=0;i<5;i++)
        {
            Dequeue();
        }
    }

    public void Enqueue(Block block)
    {
        block.gameObject.SetActive(false);
        blockPool.Add(block);
        activeBlockPool.Remove(block);
    }

    public void Dequeue()
    {
        var block = blockPool[0];
        blockPool.Remove(blockPool[0]);
        block.transform.position = new Vector2(0, height);
        block.gameObject.SetActive(true);
        if (activeBlockPool.Count == 0)
            block.MakeRandomPlatforms(0);
        else
        {
            var frontBlock = activeBlockPool[activeBlockPool.Count-1];
            block.MakeRandomPlatforms(frontBlock.answerNum);
        }
        activeBlockPool.Add(block);
        ++height;
        if(activeBlockPool.Count>=7)
        {
            Enqueue(activeBlockPool[0]);
        }
    }
}
