
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using dotgraph;
namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private string path = GetTempFilePathWithExtension(".graph");
        private byte[] src = Array.Empty<byte>();
        private Graph? graph;

        public int VerticeCount = 5_000_000;
        public int EdgeCount = 5_000_000;


        [GlobalSetup]
        public void Setup()
        {
            graph = new(VerticeCount, EdgeCount);
            for (int i = 0; i < VerticeCount; i++)
            {
                graph.Vertices[i] = new Vertice(Random.Shared.NextSingle(), Random.Shared.NextSingle());
            }
            for (int i = 0; i < EdgeCount; i++)
            {
                graph.Edges[i] = new Edge(Random.Shared.Next(VerticeCount), Random.Shared.Next(VerticeCount), Random.Shared.Next(10));
            }
            graph.Write(path);
            src = new byte[graph.ByteSize];
            graph.Write(src);
        }
        [Benchmark]
        public Graph Read()
        {
            return Graph.Read(path);
        }
        [Benchmark]
        public void Write()
        {
            graph.Write(path);
        }

        [Benchmark]
        public Graph ReadMemory()
        {
            return Graph.Read(src);
        }
        [Benchmark]
        public void WriteMemory()
        {
            graph.Write(src);
        }

        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
            return Path.Combine(path, fileName);
        }
    }
    public static class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
