using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class BlockPool : MonoBehaviour
{
    [SerializeField] private Block pBlock;
    private List<Block> blockPool = new List<Block>();
    public List<Block> activeBlockPool = new List<Block>();
    private Queue<Block> obsoleteBlock = new Queue<Block>();
    public ReactiveProperty<int> height { get; private set; }
    private bool isInit = false;

    public void GoUp(int upHeight)
    {
        height.Value += upHeight;
    }

    public bool CheckAnswer(int curPos)
    {
        Debug.Log(activeBlockPool[0].answerNum);
        Debug.Log(curPos);
        return (curPos == activeBlockPool[0].answerNum);
    }

    private void Start()
    {
        Init();

    }

    public void Init()
    {
        height = new ReactiveProperty<int>(1);
        for (int i = 0; i < 10; i++)
        {
            var block = Instantiate(pBlock);
            block.gameObject.SetActive(false);
            blockPool.Add(block);
        }
        for(int i=0;i<5;i++)
        {
            Dequeue();
            if (i != 4)
                GoUp(1);
        }
        height.Subscribe(_ => { if (isInit) { Dequeue(); Enqueue(activeBlockPool[0]); } }).AddTo(this);
        isInit = true;
    }

    public void Enqueue(Block block)
    {
        obsoleteBlock.Enqueue(block);
        if (obsoleteBlock.Count > 4)
            obsoleteBlock.Peek().gameObject.SetActive(false);
        obsoleteBlock.Dequeue();
        blockPool.Add(block);
        activeBlockPool.Remove(block);
    }

    public void Dequeue()
    {
        var block = blockPool[0];
        blockPool.Remove(blockPool[0]);
        block.transform.position = new Vector2(0, height.Value*2);
        block.gameObject.SetActive(true);
        if (activeBlockPool.Count == 0)
            block.MakeRandomPlatforms(2);
        else
        {
            var frontBlock = activeBlockPool[activeBlockPool.Count-1];
            block.MakeRandomPlatforms(frontBlock.answerNum);
        }
        activeBlockPool.Add(block);
        
        if(activeBlockPool.Count>=7)
        {
            Enqueue(activeBlockPool[0]);
        }
    }
}
