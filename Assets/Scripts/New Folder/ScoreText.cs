using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _ui;

    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    public void UpdateText(int score)
    {
        _ui.text = $"Score : {score}";
    }
}
