using Assets.Scripts.NWorld;
using Assets.Scripts.NCommand;

namespace Assets.Scripts.NEvent
{
    class E_Block_Change : EventBase<E_Block_Change>
    {
        public Com_ChangeBlock Request { set; get; }
        public E_Block_Change(ref BlockLocation Loc, byte BlockID)
        {
            Request = new Com_ChangeBlock(Loc, BlockID);
        }
    }

    class E_Block_Recover : EventBase<E_Block_Recover>
    {
    }



}
