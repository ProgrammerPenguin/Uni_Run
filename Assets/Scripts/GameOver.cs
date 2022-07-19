using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private GameObject[] _childs;
    private int _childCount;
    // 식 본문 : 한 줄일때만 가능하다. 
    public void Activate()
    {
        for (int i = 0; i < _childCount; ++i)
        {
            _childs[i].SetActive(true);
        }
    } 
    
    void Awake()
    {
        _childCount = transform.childCount;
        _childs = new GameObject[_childCount];

        for (int i = 0; i < _childCount; ++i)
        {
            _childs[i] = transform.GetChild(i).gameObject;
        }
    }
    void OnEnable()
    {
        GameManager.Instance.OnGameEnd.AddListener(Activate);    
    }

    void OnDisble()
    {
        GameManager.Instance.OnGameEnd.RemoveListener(Activate);
    }
}
