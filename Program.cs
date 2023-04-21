using System.Diagnostics;
using System.IO;
using System.Linq;

ReadData("10M");
ReadData("100M");
ReadData("500M");
ReadData("1G");
ReadData("5G");

// Read the date sequentialy and randomly
static void ReadData(string filename)
{
    string FileName = $"Files/{filename}.txt";
    long dataSize = 0;
    int n = 0;

    // Sequential access time. Read data from the file byte by byte sequentially.
    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
    {
        // Calculate the file size.
        dataSize = fs.Length;

        Stopwatch sw = Stopwatch.StartNew();
        for (long bytesRead = 0; bytesRead < dataSize; bytesRead++)
            n = fs.ReadByte();
        sw.Stop();

        Console.WriteLine($"Sequential read of {filename} bytes took {sw.ElapsedMilliseconds} ms");
    }

    // Create random permutation of indices and fill the indices array
    long[] indices = ShuffleIndices(dataSize);

    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
    {
        byte[] bytes = new byte[1];
        Stopwatch sw = Stopwatch.StartNew();
        for (long bytesRead = 0; bytesRead < dataSize; bytesRead++)
        {
            fs.Position = indices[bytesRead];
            n = fs.Read(bytes, 0, 1);
        }
        sw.Stop();

        Console.WriteLine($"Randomly read of {filename} bytes took {sw.ElapsedMilliseconds} ms");
    }
}

// Shuffle the index array
static long[] ShuffleIndices(long arraySize)
{
    long[] indexArray = new long[arraySize];
    Random rand = new Random();
    long count = arraySize;

    for (long index = 0; index < count; index++)
        indexArray[index] = index;

    while (count > 1)
    {
        long rand_index = rand.NextInt64(count);

        // Swap array data
        long temp = indexArray[rand_index];
        indexArray[rand_index] = indexArray[--count];
        indexArray[count] = temp;
    }

    return indexArray;
}
