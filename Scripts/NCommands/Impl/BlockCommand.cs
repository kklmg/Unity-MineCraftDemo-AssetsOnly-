using Assets.Scripts.NCommand.Base;
using Assets.Scripts.NWorld;

namespace Assets.Scripts.NCommand.Impl
{
    public class Com_ChangeBlock : ICommand
    {
        BlockPosition m_BlkPos; //
        byte m_PreBlkID;
        byte m_PreChangeBlkID;

        //Constructor
        public Com_ChangeBlock(BlockPosition blkpos, byte blkID)
        {
            m_BlkPos = blkpos;
            m_PreChangeBlkID = blkID;
            m_PreBlkID = m_BlkPos.CurBlockID;
        }

        public void Execute()
        {
            m_BlkPos.CurBlockID = m_PreChangeBlkID;
        }
        public void Undo()
        {
            m_BlkPos.CurBlockID = m_PreBlkID;
        }
    }
}
