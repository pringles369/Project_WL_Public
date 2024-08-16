using UnityEngine;

public class TreeNode
{
    public TreeNode leftTree;
    public TreeNode rightTree;
    public TreeNode parentTree;
    public RectInt treeSize;
    public RectInt dungeonSize;

    public TreeNode(int x, int y, int width, int height)
    {
        treeSize = new RectInt(x, y, width, height);
    }
}