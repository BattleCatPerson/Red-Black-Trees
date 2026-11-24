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
    [SerializeField] TMP_InputField deleteInputField;
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
    public void CreateNode()
    {
        if (inputField.text != "")
        {
            Debug.Log(inputField.text);
            int key = int.Parse(inputField.text);
            Node instance = Instantiate(nodeInstance, transform);
            instance.Initialize(null, null, null, TreeColor.Black, key);
            instance.name = key.ToString();
            RBTreeInsert(instance);
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
    public void SetLevels(Node n)
    {
        //foreach (Node node in activeNodes)
        //{
        //    node.SetLevel();
        //}
        Debug.Log("Current Node: " + n.name);
        if (n.parent != null)
        {
            n.level = n.parent.level + 1;
        }
        else
        {
            n.level = 0;
        }
        if (n.left != null)
        {
            SetLevels(n.left);
        }
        if (n.right != null)
        {
            SetLevels(n.right);
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
        leftLeaf.SetColor(TreeColor.Black);
        rightLeaf.SetColor(TreeColor.Black);
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
        RBInsertFixup(z);
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
        Debug.Log("FIXUP A");
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
        Debug.Log("FIXUP B");
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
        Debug.Log("FIXUP C");
        Node x = z.parent;
        Node w = x.parent;
        if (z == x.left && x == w.left)
        {
            //right rotate (w)
            RightRotate(w);
        }
        else
        {
            //left rotate (w)
            LeftRotate(w);
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
        if (x.parent == null)
        {
            y.transform.parent = transform;
            x.transform.parent = transform;
        }
        else
        {
            y.transform.parent = x.transform.parent;
        }

        x.left = y.right;
        //if (!IsLeaf(y.right))
        //{
        y.right.SetParent(x);
        //}
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
        //y.SetLevel();
        //x.SetLevel();
        //y.right.SetLevel();
        SetLevels(root);
        ReadjustOffset();
    }
    public void LeftRotate(Node x)
    {
        Node y = x.right;
        x.right = y.left;
        if (x.parent == null)
        {
            y.transform.parent = transform;
            x.transform.parent = transform;
        }
        else
        {
            y.transform.parent = x.transform.parent;
        }
        //if (!IsLeaf(y.left))
        //{
        y.left.SetParent(x);
        //}
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
        //x.SetLevel();
        //y.SetLevel();
        //y.left.SetLevel();
        SetLevels(root);
        ReadjustOffset();
    }
    public void RBTransplant(Node u, Node v)
    {
        Debug.Log(" U PARENT" + u.transform.parent + "V PARENT" + v.transform.parent);
        v.transform.parent = u.transform.parent;
        if (u.parent == null)
        {
            root = v;
        }
        else if (u == u.parent.left)
        {
            u.parent.left = v;
        }
        else
        {
            u.parent.right = v;
        }
        v.SetParent(u.parent);
        activeNodes.Remove(u);
        Destroy(u.gameObject);
        SetLevels(root);
        ReadjustOffset();
    }
    public void TestTransplant()
    {
        RBTransplant(root, root.right.right.left);
    }
    public void RBDelete(Node z)
    {
        Node y = z;
        Node x;
        TreeColor c = y.color;
        if (IsLeaf(z.left))
        {
            x = z.right;
            RBTransplant(z, z.right);
        }
        else if (IsLeaf(z.right))
        {
            x = z.left;
            RBTransplant(z, z.left);
        }
        else
        {
            y = TreeMin(z.right);
            c = y.color;
            x = y.right;
            if (y.parent == z)
            {
                x.parent = y;
            }
            else
            {
                RBTransplant(y, y.right);
                y.right = z.right;
                y.right.parent = y;
            }
            RBTransplant(z, y);
            y.left = z.left;
            y.left.parent = y;
            y.color = z.color;
        }
        if (c == TreeColor.Black)
        {
            RBDeleteFixUp(x);
        }
    }
    public void RBDeleteFixUp(Node x)
    {
        while (x != root && x.color == TreeColor.Black)
        {
            Node p = x.parent;
            if (x == p.left)
            {
                Node s = p.right;
                if (s.color == TreeColor.Red)
                {
                    s.SetColor(TreeColor.Black);
                    p.SetColor(TreeColor.Red);
                    LeftRotate(p);
                    s = p.right;
                }
                if (s.left.color == TreeColor.Black && s.right.color == TreeColor.Black)
                {
                    s.SetColor(TreeColor.Red);
                    x = p;
                }
                else
                {
                    if (s.right.color == TreeColor.Black)
                    {
                        s.left.SetColor(TreeColor.Black);
                        s.SetColor(TreeColor.Red);
                        RightRotate(s);
                        s = p.right;
                    }
                    s.SetColor(p.color);
                    p.SetColor(TreeColor.Black);
                    s.right.SetColor(TreeColor.Black);
                    LeftRotate(p);
                    x = root;
                }
            }
            else
            {
                Node s = p.left;
                if (s.color == TreeColor.Red)
                {
                    s.SetColor(TreeColor.Black);
                    p.SetColor(TreeColor.Red);
                    RightRotate(p);
                    s = p.left;
                }
                if (s.left.color == TreeColor.Black && s.right.color == TreeColor.Black)
                {
                    s.SetColor(TreeColor.Red);
                    x = p;
                }
                else
                {
                    if (s.left.color == TreeColor.Black)
                    {
                        s.right.SetColor(TreeColor.Black);
                        s.SetColor(TreeColor.Red);
                        LeftRotate(s);
                        s = p.left;
                    }
                    s.SetColor(p.color);
                    p.SetColor(TreeColor.Black);
                    s.left.SetColor(TreeColor.Black);
                    RightRotate(p);
                    x = root;
                }
            }
        }
        x.SetColor(TreeColor.Black);
    }
    public Node TreeMin(Node x)
    {
        while (!IsLeaf(x.left))
        {
            x = x.left;
        }
        return x;
    }
    public Node TreeSearch(Node x, int K)
    {
        if (IsLeaf(x) || K == x.key)
        {
            return x;
        }
        else if (K < x.key)
        {
            return TreeSearch(x.left, K);
        }
        else
        {
            return TreeSearch(x.right, K);
        }
    }
    public void Delete()
    {
        if (inputField.text != "")
        {
            int key = int.Parse(deleteInputField.text);
            Node n = TreeSearch(root, key);
            if (!IsLeaf(n))
            {
                RBDelete(n);
            }
            else
            {
                Debug.Log("NOT IN TREE!");
            }
        }

    }
}
