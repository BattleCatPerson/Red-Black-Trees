using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum TreeColor
{
    Red, Black
}
public class Node : MonoBehaviour
{
    public Node parent;
    public Node left;
    public Node right;
    public TreeColor color;
    public int key;
    public bool isLeaf = false;
    public int level;
    [SerializeField] Image image;
    [SerializeField] Image highlightImage;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float highlightDuration;
    [SerializeField] float highlightTimer;

    public void Initialize(Node parent, Node left, Node right, TreeColor color, int key)
    {
        this.parent = parent;
        this.left = left;
        this.right = right;
        this.color = color;
        this.key = key;
        text.text = key.ToString();
        SetColor(color);
        highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0);
    }
    private void Update()
    {
        if (highlightTimer > 0)
        {
            highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, Mathf.Lerp(0, 1, highlightTimer / highlightDuration));
            highlightTimer -= Time.deltaTime;
            if (highlightTimer <= 0)
            {
                highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0);
            }
        }
    }
    public void CreateLeaf()
    {
        isLeaf = true;
    }
    public void SetParent(Node parent)
    {
        if (parent != null)
        {
            transform.parent = parent.transform;
        }
        this.parent = parent;
    }
    public void SetColor(TreeColor color)
    {
        if (color == TreeColor.Black)
        {
            image.color = Color.black;
        }
        else
        {
            image.color = Color.red;
        }
        this.color = color;
    }
    public void Highlight(float duration)
    {
        highlightDuration = duration;
        highlightTimer = duration;
    }

}
