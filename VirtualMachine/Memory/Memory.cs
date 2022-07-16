using System.Runtime.CompilerServices;

namespace VirtualMachine.Memory;

public partial class Memory
{
    public Memory(int initialBlocksCount = 4)
    {
        Blocks = new List<Block>(initialBlocksCount);
        for (int i = 0; i < initialBlocksCount; i++)
        {
            Blocks.Add(CreateNewBlock());
        }
    }
    private List<Block> Blocks;

    private static Block CreateNewBlock()
    {
        return new Block();
    }

    public static int CreateRef(int blockId, int location)
    {
        return blockId << 10 | location;
    }

    public int Allocate(int size)
    {
        var block = -1;
        var location = -1;
        var locationFound = false;
        var counted = 0;
        for (var i = 0; i < Blocks.Count; i++)
        {
            var map = Blocks[i].Map;

            if (counted == 0)
                block = i;
            for (var j = 0; j < Block.BlockSize; j++)
            {
                if ((map[j / Block.MapSize] & (1 << (j % 8))) == 0)
                {
                    if (counted == 0)
                        location = j;
                    counted += 1;
                }
                else
                    counted = 0;

                if (counted != size)
                    continue;
                locationFound = true;
                break;
            }
            if (locationFound)
                break;
        }

        SetMapAllocation(1, block, location, size);

        return CreateRef(block, location);
    }

    public void Free(int @ref, int size)
    {
        SetMapAllocation(0, @ref.BlockId(), @ref.Location(), size);
    }

    private void SetMapAllocation(byte value, int blockId, int location, int size)
    {
        var blocksEnd = blockId +
                        (location + size) / Block.BlockSize -
                        1 +
                        ((location + size) % Block.BlockSize > 0 ? 1 : 0);

        var firstEnd = Min(size, Block.BlockSize - location);
        Blocks[blockId].SetMapAllocation(value, location, location + firstEnd);
        for (var i = blockId + 1; i < blocksEnd; i++)
        {
            Blocks[i].SetMapAllocation(value, 0, Block.BlockSize);
        }
        if (blocksEnd <= blockId)
            return;
        var end = (size - firstEnd) % Block.BlockSize;
        if (end == 0)
            end = Block.BlockSize;
        Blocks[blocksEnd].SetMapAllocation(value, 0, end);
    }

    public void SetData(int @ref, byte[] data)
    {
        var blockId = @ref.BlockId();
        var location = @ref.Location();

        var length = data.Length;

        var blocksEnd = blockId +
                        (location + length) / Block.BlockSize -
                        1 +
                        ((location + length) % Block.BlockSize > 0 ? 1 : 0);

        var firstEnd = Min(length, Block.BlockSize - location);
        var prevEnd = firstEnd;
        Blocks[blockId].SetData(data, 0, prevEnd, location);
        for (var i = blockId + 1; i < blocksEnd - 1; i++)
        {
            Blocks[i].SetData(data, prevEnd, prevEnd += Block.BlockSize, 0);
        }
        if (blocksEnd <= blockId)
            return;
        Blocks[blocksEnd].SetData(data, prevEnd, length, 0);
    }

    public byte[] GetData(int @ref, int size)
    {
        var blockId = @ref.BlockId();
        var location = @ref.Location();
        var data = new byte[size];

        var blocksEnd = blockId +
                        (location + size) / Block.BlockSize -
                        1 +
                        ((location + size) % Block.BlockSize > 0 ? 1 : 0);

        var firstEnd = Min(size, Block.BlockSize - location);
        var prevEnd = firstEnd;
        Blocks[blockId].GetData(data, 0, prevEnd, location);
        for (var i = blockId + 1; i < blocksEnd - 1; i++)
        {
            Blocks[i].GetData(data, prevEnd, prevEnd += Block.BlockSize, 0);
        }
        if (blocksEnd > blockId)
            Blocks[blocksEnd].GetData(data, prevEnd, prevEnd + (size - firstEnd) % Block.BlockSize, 0);

        return data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int a, int b)
    {
        return a < b ? a : b;
    }
}

public partial class Block
{
    internal byte[] Data = new byte[BlockSize];
    internal int[] Map = new int[BlockSize / MapSize];
    public const int BlockSize = 1024;
    public const int MapSize = 32;
    public void SetData(byte[] data, int start, int end, int targetStart)
    {
        if (end - start > BlockSize - targetStart)
            throw new ArgumentException("Data is too long for a block");

        for (var i = 0; i < end - start; i++)
        {
            Data[i + targetStart] = data[i + start];
        }
    }

    public void SetMapAllocation(byte value, int targetStart, int targetEnd)
    {
        if (targetEnd - targetStart > BlockSize - targetStart)
            throw new ArgumentException("Allocated block is too long for a block");

        if (value == 0)
            for (var i = targetStart; i < targetEnd; i++)
                Map[i / MapSize] &= ~(1 << (i % MapSize));
        else
            for (var i = targetStart; i < targetEnd; i++)
                Map[i / MapSize] |= 1 << (i % MapSize);
    }

    public void GetData(byte[] target, int start, int end, int targetStart)
    {
        for (var i = 0; i < end - start; i++)
        {
            target[i + start] = Data[i + targetStart];
        }
    }
}

internal static class MemoryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BlockId(this int @ref)
    {
        return @ref >> 10;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Location(this int @ref)
    {
        return @ref & 1023;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsAllocated(this byte[] block)
    {
        return block[Block.BlockSize] >> 7 == 1;
    }

}