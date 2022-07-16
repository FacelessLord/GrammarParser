namespace VirtualMachine.Memory;

public partial class Memory
{
    public List<Block> Test_GetBlocks()
    {
        return Blocks;
    }
}

public partial class Block
{
    public byte[] Test_GetData()
    {
        return Data;
    }

    public int[] Test_GetMap()
    {
        return Map;
    }
}