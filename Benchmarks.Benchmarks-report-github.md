``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702/22H2/2022Update/SunValley2)
12th Gen Intel Core i9-12900KF, 1 CPU, 24 logical and 16 physical cores
.NET SDK=7.0.302
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2


```
|      Method |     Mean |    Error |   StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
|------------ |---------:|---------:|---------:|---------:|---------:|---------:|------------:|
|        Read | 44.13 ms | 0.699 ms | 0.654 ms | 333.3333 | 333.3333 | 333.3333 | 200000526 B |
|       Write | 89.40 ms | 0.454 ms | 0.379 ms |        - |        - |        - | 100000276 B |
|  ReadMemory | 19.86 ms | 0.392 ms | 0.696 ms | 156.2500 | 156.2500 | 156.2500 | 100000174 B |
| WriteMemory | 27.89 ms | 0.538 ms | 0.598 ms |        - |        - |        - |        83 B |
