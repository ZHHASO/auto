using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自动零件盒
{
    [Serializable]
    public class Componets
    {
        /// <summary>
        /// 表示零件的位置
        /// </summary>
        public ushort Location { get; set; }
        /// <summary>
        /// 表示零件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示零件类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 表示零件的字类型
        /// </summary>
        public string Typesmall { get; set; }
        /// <summary>
        /// 表示零件的数量
        /// </summary>
        public int Pieces { get; set; }
        /// <summary>
        /// 表示零件的描述，相关信息
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// 表示零件图片
        /// </summary>
        public string Picture { get; set; }
        public Componets(string Name, string Type, string Typesmall, int Pieces, string Information, ushort location, string pict)
        {
            if (Pieces < 0)
            {
                Pieces = 0;
            }
            this.Name = Name;
            this.Type = Type;
            this.Typesmall = Typesmall;
            this.Pieces = Pieces;
            this.Information = Information;
            this.Location = location;
            this.Picture = pict;
        }
        /// <summary>
        /// 输入零件信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="Typesmall"></param>
        /// <param name="Pieces"></param>
        /// <param name="Information"></param>
        public Componets(string Name,string Type,string Typesmall,int Pieces,string Information)
        {
            if(Pieces<0)
            {
                Pieces = 0;
            }
            this.Name = Name;
            this.Type = Type;
            this.Typesmall = Typesmall;
            this.Pieces = Pieces;
            this.Information = Information;
        }

        public Componets(string Name, string Type, string Typesmall, int Pieces, string Information,string pict)
        {
            if (Pieces < 0)
            {
                Pieces = 0;
            }
            this.Name = Name;
            this.Type = Type;
            this.Typesmall = Typesmall;
            this.Pieces = Pieces;
            this.Information = Information;
            this.Picture = pict;
        }
        /// <summary>
        /// 输入零件信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="Typesmall"></param>
        /// <param name="Pieces"></param>
        public Componets(string Name, string Type, string Typesmall, int Pieces)
        {
            if (Pieces < 0)
            {
                Pieces = 0;
            }
            this.Name = Name;
            this.Type = Type;
            this.Typesmall = Typesmall;
            this.Pieces = Pieces;
        }
        /// <summary>
        /// 添加零件
        /// </summary>
        /// <param name="Nums"></param>
        public void Add(int Nums)
        {this.Pieces+=Nums;}
        /// <summary>
        /// 移除零件
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        public bool Pick(int Nums)
        {
            if(Nums>this.Pieces)
            { return false; }
            this.Pieces -= Pieces;
            return true;
        }
    }
}
