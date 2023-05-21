namespace dotgraph;
public class Graph
{
    public Graph(int verticeCount, int edgeCount)
    {
        Edges = new Edge[edgeCount];
        Vertices = new Vertice[verticeCount];
    }
    public Edge[] Edges;
    public Vertice[] Vertices;
    private ILookup<int, Edge>? byCategory;
    private ILookup<int, Edge>? bySourceNode;
    private ILookup<int, Edge>? byTargetNode;
    public int VerticeCount
    {
        get => Vertices.Length;
    }
    public int EdgeCount
    {
        get => Edges.Length;
    }
    public (Vertice, Vertice) GetVertices(Edge edge)
    {
        return (Vertices[edge.Source], Vertices[edge.Target]);
    }
    public IEnumerable<Edge> GetEdges(Vertice node)
    {
        int index = Array.IndexOf(Vertices, node);
        if (index == -1) return Array.Empty<Edge>();
        return GetEdgesFromNode(index).Concat(GetEdgesToNode(index));
    }
    public IEnumerable<Edge> GetEdgesOfCategory(int category)
    {
        if (byCategory == null)
        {
            byCategory = Edges.ToLookup(x => x.Category);
        }
        return byCategory[category];
    }
    public IEnumerable<Edge> GetEdgesFromNode(int source)
    {
        if (bySourceNode == null)
        {
            bySourceNode = Edges.ToLookup(x => x.Source);
        }
        return bySourceNode[source];
    }
    public IEnumerable<Edge> GetEdgesToNode(int target)
    {
        if (byTargetNode == null)
        {
            byTargetNode = Edges.ToLookup(x => x.Target);
        }
        return byTargetNode[target];
    }
    public static Graph Read(string path)
    {
        using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        return Read(buffer);
    }
    public static Graph Read(byte[] bytes)
    {
        Graph graph = new(BitConverter.ToInt32(bytes, 0), BitConverter.ToInt32(bytes, sizeof(int)));
        int offset = 2 * sizeof(int);
        for (int i = 0; i < graph.VerticeCount; i++)
        {
            graph.Vertices[i] = bytes.GetVertice(ref offset);
        }
        for (int i = 0; i < graph.EdgeCount; i++)
        {
            graph.Edges[i] = bytes.GetEdge(ref offset);
        }
        return graph;
    }
    public static Graph Read(Stream stream)
    {
        using BinaryReader reader = new(stream);
        return Read(reader);
    }
    public static Graph Read(BinaryReader reader)
    {
        Graph graph = new(reader.ReadInt32(), reader.ReadInt32());
        for (int i = 0; i < graph.VerticeCount; i++)
        {
            graph.Vertices[i] = reader.ReadVertice();
        }
        for (int i = 0; i < graph.EdgeCount; i++)
        {
            graph.Edges[i] = reader.ReadEdge();
        }
        return graph;
    }
    public void Write(string path)
    {
        // FileStream stream = new(path, FileMode.Create, FileAccess.Write, FileShare.Write, int.Min(ByteSize / 2, 0x20000), FileOptions.SequentialScan);
        byte[] bytes = new byte[ByteSize];
        Write(bytes);
        File.WriteAllBytes(path,bytes);
        // Write(stream);
    }
    public void Write(byte[] targetBytes)
    {
        Array.Copy(BitConverter.GetBytes(VerticeCount), 0, targetBytes, 0, sizeof(int));
        Array.Copy(BitConverter.GetBytes(EdgeCount), 0, targetBytes, sizeof(int), sizeof(int));
        int offset = 2 * sizeof(int);
        for (int i = 0; i < VerticeCount; i++)
        {
            offset = targetBytes.WriteVertice(Vertices[i], offset);
        }
        for (int i = 0; i < EdgeCount; i++)
        {
            offset = targetBytes.WriteEdge(Edges[i], offset);
        }
    }
    public void Write(Stream stream)
    {
        using BinaryWriter writer = new(stream);
        Write(writer);
    }
    public void Write(BinaryWriter writer)
    {
        writer.Write(VerticeCount);
        writer.Write(EdgeCount);
        for (int i = 0; i < VerticeCount; i++)
        {
            writer.Write(Vertices[i]);
        }
        for (int i = 0; i < EdgeCount; i++)
        {
            writer.Write(Edges[i]);
        }
    }
    public int ByteSize => sizeof(int) * (2 + EdgeCount * 3) + sizeof(float) * 2 * VerticeCount;
}