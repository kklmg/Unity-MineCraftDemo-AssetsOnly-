using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NWorld;
using UnityEngine;

public interface ICommand
{
    //void Enter();
    bool IsExecuted { set; get; }
    void Execute();
    void Undo();
}

public abstract class CommandBase : ICommand
{
    public bool IsExecuted { set; get; }
    public void Execute()
    {
        if (IsExecuted==false)
        {
            AExecute();
            IsExecuted = true;
        }
    }
    public void Undo()
    {
        if (IsExecuted == true)
        {
            AExecute();
            IsExecuted = false;
        }
    }

    protected abstract void AExecute();
    protected abstract void AUndo();
}

public class ComChangeBlock : CommandBase
{
    BlockPosition m_BlkPos; //
    byte m_PreBlkID;
    byte m_PreChangeBlkID;

    //Constructor
    public ComChangeBlock(BlockPosition blkpos, byte blkID)
    {
        m_BlkPos = blkpos;
        m_PreChangeBlkID = blkID;
        m_PreBlkID = m_BlkPos.CurBlockID;
        IsExecuted = false;
    }

    protected override void AExecute()
    {
        m_BlkPos.CurBlockID = m_PreChangeBlkID;
    }
    protected override void AUndo()
    {
        m_BlkPos.CurBlockID = m_PreBlkID;
    }
}


