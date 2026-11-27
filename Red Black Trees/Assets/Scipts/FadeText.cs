using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float timer;
    [SerializeField] float duration;
    private void Start()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
    void Update()
    {
        if (timer > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, 1, timer / duration));
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
        }
    }
    public void SetTimer(float duration)
    {
        this.duration = duration;
        timer = duration;
    }
}
