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
        //GenerateTree();
    }

    void Update()
    {

    }
    public void RotateRightRoot()
    {
        RightRotate(root);
    }
    public void RotateLeftRoot()
    {
        LeftRotate(root);
    }
    //public void GenerateTree()
    //{
    //    int h = (int)Mathf.Ceil(Mathf.Log(n, 2));
    //    for (int i = 0; i <= h; i++)
    //    {
    //        for (int j = 0; j < Mathf.Pow(2, i); j++)
    //        {
    //            var inst = Instantiate(nodeInstance, transform);
    //            inst.name = "Height: " + i.ToString() + " Number: " + (j + 1).ToString();
    //        }
    //    }
    //    Debug.Log(h);
    //}
    public void CreateNode()
    {
        if (inputField.text != "")
        {
            Debug.Log(inputField.text);
            int key = int.Parse(inputField.text);
            Node instance = Instantiate(nodeInstance, transform);
            instance.Initialize(null, null, null, TreeColor.Black, key);
            RBTreeInsert(instance);
        }
    }
    //public void Offset(Node node, bool isLeft, bool isLeaf)
    //{
    //    float adjustedXOffset = xOffset;
    //    if (node.parent != null)
    //    {
    //        //adjustedXOffset /= 4;
    //        adjustedXOffset = Mathf.Pow(levelMultiplier, maxLevel - node.level) * xOffset;
    //    }
    //    if (isLeft)
    //    {
    //        node.transform.localPosition = new Vector2(-adjustedXOffset, -yOffset);
    //    }
    //    else
    //    {
    //        node.transform.localPosition = new Vector2(adjustedXOffset, -yOffset);
    //    }
    //}
    public void ReadjustOffset()
    {
        foreach (Node node in activeNodes)
        {
            Transform t = node.transform;
            if (node.parent != null)
            {
                float offset = xOffset;
                //if (node.level == maxLevel - 1)
                //{
                //    offset /= 2;
                //}
                offset = xOffset * Mathf.Pow(levelMultiplier, maxLevel - node.level);
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
        leftLeaf.SetParent(parent);
        rightLeaf.SetParent(parent);
        parent.left = leftLeaf;
        parent.right = rightLeaf;
        activeNodes.Add(leftLeaf);
        activeNodes.Add(rightLeaf);
        //Offset(leftLeaf.transform, true, true);
        //Offset(rightLeaf.transform, false, true);
    }
    public void RBTreeInsert(Node z)
    {
        Node y = RBLocateParent(z);
        z.SetParent(y);

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
    public Node Sibling(Node v)
    {
        Node p = v.parent;
        if (p.left == v)
        {
            return p.right;
        }
        else
        {
            return p.left;
        }
    }
    public Node RBInsertFixupA(Node z)
    {
        while (z != root && z.parent.color == TreeColor.Red)
        {
            Node y = Sibling(z.parent);
            if (y.color == TreeColor.Black)
            {
                return z;
            }
            z.parent.SetColor(TreeColor.Black);
            y.SetColor(TreeColor.Black);
            z = z.parent.parent;
            z.SetColor(TreeColor.Red);
        }
        return z;
    }
    public Node RBInsertFixupB(Node z)
    {
        Node x = z.parent;
        Node w = x.parent;
        if (z == x.right && x == w.left)
        {
            z = x;
            //left rotate (x)
            LeftRotate(x);
        }
        else if (z == x.left && x == w.right)
        {
            z = x;
            //right rotate (x)
            RightRotate(x);
        }
        return z;
    }
    public Node RBInsertFixupC(Node z)
    {
        Node x = z.parent;
        Node w = x.parent;
        if (z == x.left && x == w.left)
        {
            //right rotate (w)
        }
        else
        {
            //left rotate (w)
        }
        x.SetColor(TreeColor.Black);
        w.SetColor(TreeColor.Red);
        return z;
    }
    public void RBInsertFixup(Node z)
    {
        z = RBInsertFixupA(z);
        if (z != root && z.parent.color == TreeColor.Red)
        {
            z = RBInsertFixupB(z);
            z = RBInsertFixupC(z);
        }
        root.SetColor(TreeColor.Black);
    }
    public void RightRotate(Node x)
    {
        Node y = x.left;
        x.left = y.right;
        if (!IsLeaf(y.right))
        {
            y.right.SetParent(x);
        }
        y.SetParent(x.parent);
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.right)
        {
            x.parent.right = y;
        }
        else
        {
            x.parent.left = y;
        }
        y.right = x;
        x.SetParent(y);
        ReadjustOffset();
    }
    public void LeftRotate(Node x)
    {
        Node y = x.right;
        x.right = y.left;
        if (!IsLeaf(y.left))
        {
            y.left.SetParent(x);
        }
        y.parent = x.parent;
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.left)
        {
            x.parent.left = y;
        }
        else
        {
            x.parent.right = y;
        }
        y.left = x;
        x.SetParent(y);
        ReadjustOffset();
    }
}
