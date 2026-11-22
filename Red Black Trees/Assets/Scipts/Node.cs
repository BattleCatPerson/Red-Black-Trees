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
    [SerializeField] TextMeshProUGUI text;
    public void Initialize(Node parent, Node left, Node right, TreeColor color, int key)
    {
        this.parent = parent;
        this.left = left;
        this.right = right;
        this.color = color;
        this.key = key;
        text.text = key.ToString();
        this.SetColor(color);
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
    //public void SetLevel()
    //{
    //    if (parent != null)
    //    {
    //        Debug.Log("Current node: " + name + " Parent node: " + parent.name + " Parent Level: " + parent.level);
    //        level = parent.level + 1;
    //    }
    //    else
    //    {
    //        level = 0;
    //    }
    //}

}
