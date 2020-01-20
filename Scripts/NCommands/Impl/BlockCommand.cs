using Assets.Scripts.NCommand.Base;
using Assets.Scripts.NWorld;

namespace Assets.Scripts.NCommand.Impl
{
    public class Com_ChangeBlock : ICommand
    {
        BlockLocation m_BlkLoc; //
        byte m_PreBlkID;
        byte m_PreChangeBlkID;

        //Constructor
        public Com_ChangeBlock(BlockLocation blkloc, byte blkID)
        {
            m_BlkLoc = blkloc;
            m_PreChangeBlkID = blkID;
            m_PreBlkID = m_BlkLoc.CurBlockID;
        }

        public void Execute()
        {
            m_BlkLoc.CurBlockID = m_PreChangeBlkID;
        }
        public void Undo()
        {
            m_BlkLoc.CurBlockID = m_PreBlkID;
        }
    }
}
