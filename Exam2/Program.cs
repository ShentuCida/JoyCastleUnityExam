Console.WriteLine("Hello, World!");
BTreeExam.Create();
BTreeExam.Print();

/// <summary>
/// 数据结构.
/// </summary>
public class BTreeExam
{
    //首先定义一下结构.
    public class BTree
    {
        public int value;
        public BTree? left = null;
        public BTree? right = null;

        public BTree(int value)
        {
            left = null;
            right = null;
            this.value = value;
        }
    }
    //表头.
    private static BTree head;
    //创建二叉树.
    public static void Create()
    {
        head = new BTree(2);
        head.value = 2;

        head.left = new BTree(11);
        head.right = new BTree(23);

        head.left.left = new BTree(10);
        head.left.right = new BTree(15);

        head.right.left = new BTree(7);
        head.right.right = new BTree(14);

        head.right.left.right = new BTree(12);
        head.right.left.right.left = new BTree(13);
    }
    //递归寻找最左侧的子叶,并将找到的值保存.
    public static void FindLeft(BTree tree, int depth, Dictionary<int, int> dic)
    {        
        if (tree == null)
        {
            return;
        }
        //如果还没存过这层的数据,那这个数据就是要取出来的左侧值.
        if (!dic.ContainsKey(depth))
        {
            dic.Add(depth, tree.value);
        }
        //这里的递归逻辑可总结为:先查左再查右,有值就以层为索引存下,存过的层不再存新数据.
        //如果要查询右侧,将这两个if颠倒即可.
        if (tree.left != null)
        {
            FindLeft(tree.left, depth + 1, dic);
        }
        if (tree.right != null)
        {
            FindLeft(tree.right, depth + 1, dic);
        }
    }
    public static void Print()
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        FindLeft(head, 0, dic);
        //打印这个值.
        string result = "[";
        for (int i = 0; i < dic.Count; i++)
        {
            result += (dic[i]);
            //最后的那个加个反括号.
            result += i == dic.Count - 1 ? "]" : ",";
        }
        Console.WriteLine(result);
    }
}
