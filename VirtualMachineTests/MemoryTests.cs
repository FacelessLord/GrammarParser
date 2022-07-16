using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using VirtualMachine.Memory;

namespace VirtualMachineTests;

public class MemoryTests
{
    private Memory Memory;
    [SetUp]
    public void Setup()
    {
        Memory = new Memory(4);
    }

    [Test]
    public void SetsMapAllocation()
    {
        Memory.Allocate(1536);

        var blocks = Memory.Test_GetBlocks();
        var maps = blocks.Select(b => b.Test_GetMap()).ToList();

        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[0][i].Should().Be(-1); // 0b1111_1111_1111_1111_1111_1111_1111_1111
        }
        for (var i = 0; i < Block.BlockSize / 2 / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(-1);
        }
        for (var i = Block.BlockSize / 2 / Block.MapSize; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(0);
        }
        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[2][i].Should().Be(0);
        }
    }
    
    [Test]
    public void SetsMapAllocationInTheMiddle()
    {
        var ref1 = Memory.Allocate(Block.BlockSize*3/2);
        Memory.Allocate(Block.BlockSize*3/2);
        Memory.Free(ref1, Block.BlockSize*3/2);

        var blocks = Memory.Test_GetBlocks();
        var maps = blocks.Select(b => b.Test_GetMap()).ToList();

        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[0][i].Should().Be(0);
        }
        for (var i = 0; i < Block.BlockSize / 2 / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(0);
        }
        for (var i = Block.BlockSize / 2 / Block.MapSize; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(-1);
        }
        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[2][i].Should().Be(-1);
        }
    }
    
    [Test]
    public void ResetsMapAllocation()
    {
        var ref1 = Memory.Allocate(4096);
        Memory.Free(ref1, 1536);

        var blocks = Memory.Test_GetBlocks();
        var maps = blocks.Select(b => b.Test_GetMap()).ToList();

        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[0][i].Should().Be(0); // 0b1111_1111_1111_1111_1111_1111_1111_1111
        }
        for (var i = 0; i < Block.BlockSize / 2 / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(0);
        }
        for (var i = Block.BlockSize / 2 / Block.MapSize; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[1][i].Should().Be(-1);
        }
        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[2][i].Should().Be(-1);
        }
        for (var i = 0; i < Block.BlockSize / Block.MapSize; i++)
        {
            maps[3][i].Should().Be(-1);
        }
    }

    [Test]
    public void WritesDataFromStart()
    {
        var ref1 = Memory.Allocate(Block.BlockSize*3/2);

        var data = new byte[Block.BlockSize*3/2];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)((2 * i + 1) % 237);
        }

        Memory.SetData(ref1, data);
        
        var blocks = Memory.Test_GetBlocks();
        
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[0].Test_GetData()[i].Should().Be(data[i]);
        }
        for (var i = 0; i < Block.BlockSize/2; i++)
        {
            blocks[1].Test_GetData()[i].Should().Be(data[i+Block.BlockSize]);
        }
        for (var i = Block.BlockSize/2; i < Block.BlockSize; i++)
        {
            blocks[1].Test_GetData()[i].Should().Be(0);
        }
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[2].Test_GetData()[i].Should().Be(0);
        }
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[3].Test_GetData()[i].Should().Be(0);
        }
    } 
    
    [Test]
    public void WritesDataInTheMiddle()
    {
        Memory.Allocate(Block.BlockSize*3/2);
        var ref2 = Memory.Allocate(Block.BlockSize*3/2);

        var data = new byte[Block.BlockSize*3/2];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)((4 * i + 1) % 237);
        }

        Memory.SetData(ref2, data);
        
        var blocks = Memory.Test_GetBlocks();
        
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[0].Test_GetData()[i].Should().Be(0);
        }
        for (var i = 0; i < Block.BlockSize/2; i++)
        {
            blocks[1].Test_GetData()[i].Should().Be(0);
        }
        for (var i = Block.BlockSize/2; i < Block.BlockSize; i++)
        {
            blocks[1].Test_GetData()[i].Should().Be(data[i-Block.BlockSize/2]);
        }
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[2].Test_GetData()[i].Should().Be(data[i+Block.BlockSize/2]);
        }
        for (var i = 0; i < Block.BlockSize; i++)
        {
            blocks[3].Test_GetData()[i].Should().Be(0);
        }
    } 
}