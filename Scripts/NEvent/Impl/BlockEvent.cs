using Assets.Scripts.NWorld;
using Assets.Scripts.NCommand.Impl;

namespace Assets.Scripts.NEvent.Impl
{
    class E_Block_Change : EventBase<E_Block_Change>
    {
        public Com_ChangeBlock Change { set; get; }
        public E_Block_Change(ref BlockPosition pos, byte BlockID)
        {
            Change = new Com_ChangeBlock(pos, BlockID);
        }
    }

    class E_Block_Destroy : EventBase<E_Block_Destroy>
    {
        private Com_ChangeBlock m_Command;
        public E_Block_Destroy(ref BlockPosition pos)
        {
            m_Command = new Com_ChangeBlock(pos, 0);
        }
    }
}
