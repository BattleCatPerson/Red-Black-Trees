using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RedBlackTree : MonoBehaviour
{
    [SerializeField] Node root;
    [SerializeField] Node nodeInstance;
    [SerializeField] Node leafInstance;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] float xOffset;
    [SerializeField] float levelMultiplier;
    [SerializeField] float yOffset;
    [SerializeField] int maxLevel = -1;
    [SerializeField] List<Node> activeNodes;
    [SerializeField] List<Node> nodes;
    [SerializeField] int n;
    void Start()
    {
        root = null;
        GenerateTree();
    }

    void Update()
    {

    }
    public void GenerateTree()
    {
        int h = (int)Mathf.Ceil(Mathf.Log(n, 2));
        for (int i = 0; i <= h; i++)
        {
            for (int j = 0; j < Mathf.Pow(2, i); j++)
            {
                var inst = Instantiate(nodeInstance, transform);
                inst.name = "Height: " + i.ToString() + " Number: " + (j + 1).ToString();
            }
        }
        Debug.Log(h);
    }
    public void CreateNode()
    {
        if (inputField.text != "")
        {
            Debug.Log(inputField.text);
            int key = int.Parse(inputField.text);
            Node instance = Instantiate(nodeInstance, transform);
            instance.Initialize(null, null, null, TreeColor.Black, key);
            InsertNode(instance);
        }
    }
    public void Offset(Transform node, bool isLeft, bool isLeaf)
    {
        float adjustedXOffset = xOffset;
        if (isLeaf)
        {
            adjustedXOffset /= 4;
        }
        if (isLeft)
        {
            node.localPosition = new Vector2(-adjustedXOffset, -yOffset);
        }
        else
        {
            node.localPosition = new Vector2(adjustedXOffset, -yOffset);
        }
    }
    public void ReadjustOffset()
    {
        foreach (Node node in activeNodes)
        {
            Transform t = node.transform;
            if (node.parent != null)
            {
                float offset = xOffset;
                if (node.level == maxLevel - 1)
                {
                    offset /= 2;
                }
                //if (node.level < 2)
                //{
                //    offset = xOffset * Mathf.Max(levelMultiplier * (maxLevel - node.level), 1);
                //}
                if (node.parent.right == node)
                {
                    t.localPosition = new Vector2(offset, -yOffset);
                }
                else
                {
                    t.localPosition = new Vector2(-offset, -yOffset);
                }
            }
            else
            {
                t.localPosition = Vector2.zero;
            }
        }
    }
    public void CreateLeaves(Node parent)
    {
        Node leftLeaf = Instantiate(leafInstance, transform);
        Node rightLeaf = Instantiate(leafInstance, transform);
        int newLevel = parent.level + 1;
        if (newLevel > maxLevel)
        {
            maxLevel = newLevel;
        }
        leftLeaf.level = newLevel;
        rightLeaf.level = newLevel;
        leftLeaf.transform.parent = parent.transform;
        rightLeaf.transform.parent = parent.transform;
        leftLeaf.parent = parent;
        rightLeaf.parent = parent;
        parent.left = leftLeaf;
        parent.right = rightLeaf;
        activeNodes.Add(leftLeaf);
        activeNodes.Add(rightLeaf);
        //Offset(leftLeaf.transform, true, true);
        //Offset(rightLeaf.transform, false, true);
    }
    public void InsertNode(Node z)
    {
        Node y = RBLocateParent(z);
        z.parent = y;
        if (y != null)
        {
            z.transform.parent = y.transform;
        }
        if (y == null)
        {
            root = z;
            z.transform.localPosition = Vector3.zero;
        }
        else if (z.key < y.key)
        {
            activeNodes.Remove(y.left);
            Destroy(y.left.gameObject);
            y.left = z;
            //Offset(z.transform, true, false);
        }
        else
        {
            activeNodes.Remove(y.right);
            Destroy(y.right.gameObject);
            y.right = z;
            //Offset(z.transform, false, false);
        }
        CreateLeaves(z);
        z.SetColor(TreeColor.Red);
        activeNodes.Add(z);
        ReadjustOffset();
    }
    public Node RBLocateParent(Node z)
    {
        Node y = null;
        Node x = root;
        int level = 0;
        while (!IsLeaf(x))
        {
            y = x;
            if (z.key < x.key)
            {
                x = x.left;
            }
            else
            {
                x = x.right;
            }
            level++;
        }
        z.level = level;
        return y;
    }
    public bool IsLeaf(Node node) => node == null || node.isLeaf;
}
