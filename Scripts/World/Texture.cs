using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class Frame
    {
        public float left = 0;
        public float right = 0;
        public float top = 0;
        public float bottom = 0;
    }
    [CreateAssetMenu(menuName ="TextureSheet")]
    public class TextureSheet : ScriptableObject
    {
        public Texture2D m_Texture;

        private int m_nMaxRow;
        private int m_nMaxColumn;

        private float m_fCellWidth;
        private float m_fCellHeight;
        public int MaxRow { get { return m_nMaxRow; } }
        public int MaxColumn { get { return m_nMaxColumn; } }

        public void SetMaxRow(int row)
        {
            m_nMaxRow = row;
            m_fCellHeight = 1.0f / row;
        }
        public void SetMaxColumn(int column)
        {
            m_nMaxColumn = column;
            m_fCellWidth = 1.0f / column;
        }

        public Frame GetCoord(int row, int column)
        {
            if (row > m_nMaxRow || column > m_nMaxColumn || row<0 || column<0)
            {
                Debug.Log("overflow");
                return null;
            }
            
            Frame frame = new Frame();

            frame.top = row * m_fCellHeight;
            frame.left =  column * m_fCellWidth;
            frame.bottom = (row + 1) * m_fCellHeight;
            frame.right = (column + 1) * m_fCellWidth;

            return frame;
        }
    }
}
