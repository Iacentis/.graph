namespace dotgraph;

public class GraphTests
{

    public static string GetTempFilePathWithExtension(string extension)
    {
        var path = Path.GetTempPath();
        var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
        return Path.Combine(path, fileName);
    }
    string TargetPath = GetTempFilePathWithExtension(".graph");
    byte[] bytes = Array.Empty<byte>();
    [Fact]
    public void WriteTest()
    {
        Graph graph = new Graph(10, 20);
        graph.Write(TargetPath);
        Assert.True(File.Exists(TargetPath));
    }
    [Fact]
    public void ReadTest()
    {
        WriteTest();
        Graph graph = Graph.Read(TargetPath);
        Assert.Equal(10, graph.VerticeCount);
        Assert.Equal(20, graph.EdgeCount);
    }
    [Fact]
    public void MemoryWriteTest()
    {
        Graph graph = new Graph(10, 20);
        bytes = new byte[graph.ByteSize];
        Assert.Equal(328, bytes.Length);
        graph.Write(bytes);
    }
    [Fact]
    public void MemoryReadTest()
    {
        MemoryWriteTest();
        Graph graph = Graph.Read(bytes);
        Assert.Equal(10, graph.VerticeCount);
        Assert.Equal(20, graph.EdgeCount);
    }
    [Fact]
    public void LookupTests()
    {
        Graph graph = new(4, 4);
        graph.Edges[0] = new Edge(0, 1, 1);
        graph.Edges[1] = new Edge(1, 2, 1);
        graph.Edges[2] = new Edge(3, 2, 2);
        graph.Edges[3] = new Edge(3, 1, 2);
        var collection = graph.GetEdgesOfCategory(2);
        Assert.Equal(2, collection.Count());
        Assert.Contains(graph.Edges[2], collection);
        Assert.Contains(graph.Edges[3], collection);
        collection = graph.GetEdgesFromNode(0);
        Assert.Single(collection);
        Assert.Contains(graph.Edges[0], collection);
        collection = graph.GetEdgesToNode(2);
        Assert.Equal(2, collection.Count());
        Assert.Contains(graph.Edges[1], collection);
        Assert.Contains(graph.Edges[2], collection);

    }
}