using Assets.Scripts.Pattern;


namespace Assets.Scripts.BehaviorTree
{
    //Behavior states
    public enum eRunningState
    {
        Failed = 0,
        Suceed = 1,
        Ready,
        Running,
        Completed
    };

    //public static class RunningState
    //{
    //    const int Failed = 0x00000000;
    //    const int Suceed = 0x00000001;
    //    const int Running = 0x00000010;

    //    static bool And()
    //    {
    //        return true;
    //    }
    //}

    //Working Data
    public class BevData
    {
        public BevData()
        {
            m_BLackBoard = new BlackBoard();
        }
        //Field
        //------------------------------------------------------------
        private BlackBoard m_BLackBoard;

        public void SetValue(string key, object value)
        {
            m_BLackBoard.SetValue(key, value);
        }
        public bool GetValue<T>(string key, out T outData)
        {
            return m_BLackBoard.GetValue<T>(key, out outData);
        }
    }
}
