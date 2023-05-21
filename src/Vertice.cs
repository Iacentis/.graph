namespace dotgraph;
public struct Vertice
{
    public Vertice(float x, float y)
    {
        X = x;
        Y = y;
    }
    public readonly float X;
    public readonly float Y;

    public (float x, float y) Deconstruct()
    {
        return (X, Y);
    }
}
public static class VerticeReadWriteExtensions
{
    public static void Write(this BinaryWriter writer, Vertice vertice)
    {
        writer.Write(vertice.X);
        writer.Write(vertice.Y);
    }
    public static Vertice ReadVertice(this BinaryReader reader)
    {
        return new Vertice(reader.ReadSingle(), reader.ReadSingle());
    }
    public static unsafe Vertice GetVertice(this byte[] span, ref int offset)
    {
        float x = 0;
        float y = 0;
        fixed (byte* numPtr = span)
        {
            x = *(float*)(numPtr + offset);
            offset += sizeof(float);
            y = *(float*)(numPtr + offset);
            offset += sizeof(float);
        }
        return new Vertice(x, y);
    }
    public static unsafe int WriteVertice(this byte[] span, Vertice vertice, int offset)
    {
        byte* p = (byte*)&vertice.X;
        span[offset] = *p++;
        span[offset + 1] = *p++;
        span[offset + 2] = *p++;
        span[offset + 3] = *p++;
        p = (byte*)&vertice.Y;
        span[offset + 4] = *p++;
        span[offset + 5] = *p++;
        span[offset + 6] = *p++;
        span[offset + 7] = *p++;
        return offset + 8;
    }
}