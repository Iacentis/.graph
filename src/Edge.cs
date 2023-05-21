namespace dotgraph;
public struct Edge
{
    public Edge(int source, int target, int category)
    {
        Source = source;
        Target = target;
        Category = category;
    }
    public readonly int Source;
    public readonly int Target;
    public readonly int Category;
}
public static class EdgeReadWriteExtensions
{
    public static void Write(this BinaryWriter writer, Edge edge)
    {
        writer.Write(edge.Source);
        writer.Write(edge.Target);
        writer.Write(edge.Category);
    }
    public static Edge ReadEdge(this BinaryReader reader)
    {
        return new Edge(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }
    public static unsafe Edge GetEdge(this byte[] span, ref int offset)
    {

        int source = 0;
        int target = 0;
        int category = 0;
        fixed (byte* numPtr = span)
        {
            source = *(int*)(numPtr + offset);
            offset += sizeof(int);
            target = *(int*)(numPtr + offset);
            offset += sizeof(int);
            category = *(int*)(numPtr + offset);
            offset += sizeof(int);
        }
        return new Edge(source, target, category);
    }
    public static unsafe int WriteEdge(this byte[] span, Edge edge, int offset)
    {

        byte* p = (byte*)&edge.Source;
        span[offset] = *p++;
        span[offset + 1] = *p++;
        span[offset + 2] = *p++;
        span[offset + 3] = *p++;
        p = (byte*)&edge.Target;
        span[offset + 4] = *p++;
        span[offset + 5] = *p++;
        span[offset + 6] = *p++;
        span[offset + 7] = *p++;
        p = (byte*)&edge.Category;
        span[offset + 8] = *p++;
        span[offset + 9] = *p++;
        span[offset + 10] = *p++;
        span[offset + 11] = *p++;
        return offset + 12;
    }
}