
class Node
{
    public Node PreNode;
    public Node NextNode;
    public NodeList Parent;
    public object Data;
}
class NodeList
{
    private Node m_HeadNode;
    private Node m_EndNode;
    private int m_Length;
    public int Length { get => m_Length; private set => m_Length = value; }
    public Node EndNode { get => m_EndNode; private set => m_EndNode = value; }
    public Node HeadNode { get => m_HeadNode; private set => m_HeadNode = value; }

    public bool Push_Front(object inData)
    {
        if (inData == null)
            return false;

        Node tNode = new Node { Data = inData, Parent = this };

        if (HeadNode == null || EndNode == null)
        {
            HeadNode = tNode;
            EndNode = tNode;
        }
        else
        {
            HeadNode.PreNode = tNode;
            tNode.NextNode = HeadNode;
            HeadNode = tNode;
        }

        m_Length++;
        return true;
    }

    public bool Push_Back(object inData)
    {
        if (inData == null)
            return false;

        Node tNode = new Node { Data = inData, Parent = this };

        if (HeadNode == null || EndNode == null)
        {
            HeadNode = tNode;
            EndNode = tNode;
        }
        else
        {
            EndNode.NextNode = tNode;
            tNode.PreNode = EndNode;
            EndNode = tNode;
        }
        m_Length++;

        return true;
    }
}
