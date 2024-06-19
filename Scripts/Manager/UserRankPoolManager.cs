using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRankPoolManager : MonoBehaviour
{
    [SerializeField] private ItemUserController UserRankPrefab;
    [SerializeField] private Transform PoolStorageTransform;
    [SerializeField] int PreparingCount = 30;

    private List<ItemUserController> pooledUserRanks = new List<ItemUserController>();

    private void Awake()
    {
        InitUserRankPool();
    }

    private void InitUserRankPool()
    {
        pooledUserRanks = new List<ItemUserController>();
        for (int i = 0; i < PreparingCount; i++)
        {
            ItemUserController userRank = Instantiate<ItemUserController>(UserRankPrefab, PoolStorageTransform);
            userRank.gameObject.SetActive(false);
            pooledUserRanks.Add(userRank);
        }
    }

    public ItemUserController GetUserRankUI()
    {
        for (int i = 0; i < pooledUserRanks.Count; i++)
        {
            if (pooledUserRanks[i].gameObject == null)
                pooledUserRanks.RemoveAt(i);

            if (!pooledUserRanks[i].gameObject.activeInHierarchy)
            {
                return pooledUserRanks[i];
            }
        }

        ItemUserController userRankUI = Instantiate<ItemUserController>(UserRankPrefab, PoolStorageTransform);
        userRankUI.gameObject.SetActive(false);
        pooledUserRanks.Add(userRankUI);
        return userRankUI;
    }

    public void DisableToPool(ItemUserController userRankUI)
    {
        userRankUI.gameObject.SetActive(false);
        userRankUI.transform.SetParent(PoolStorageTransform);
    }
}
