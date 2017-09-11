using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace First
{
    class State
    {
        char[,] a = new char[2, 4];
        public State(State t)
        {
            a = t.a;
        }
        public State()
        {
            a = new char[2, 4];
        }       
        static FileStream fs = new FileStream("INPUT.txt", FileMode.Open);
        static StreamReader sr = new StreamReader(fs);
        public static State Read()
        {
            State s = new State();
            string str = "";            
            for (int i = 0; i < 2; i++)
            {
                str = sr.ReadLine();
                for (int j = 0; j < 4; j++)
                {
                    s.a[i, j] = str[j];
                    Console.Write(s.a[i, j]);

                }
                
            }
            Console.WriteLine();       
            return s;
        }
        public State Shift(int di, int dj)
        {
            State next = this;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (a[i, j] == '#')
                    {
                        int ni = i + di;
                        int nj = j + dj;
                        if (0 <= ni && ni < 2 && 0 <= nj && nj < 4)
                        {
                            char t = next.a[i, j];
                            next.a[i, j] = next.a[ni, nj];
                            next.a[ni, nj] = t;
                        }
                        return next;
                    }
                }
            }
            return next;

        }
        static public int Compare (State left, State right)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (left.a[i, j] != right.a[i, j]) { return -1; }
                }
            }
            return 0;
        }
        //static public bool operator <(State left, State right)
        //{
        //    return Compare(left, right) < 0;
        //}
        //static public bool operator >(State left, State right)
        //{
        //    return Compare(left, right) > 0;
        //}
        static public bool operator ==(State left, State right)
        {
            return Compare(left, right) == 0;
        }
        static public bool operator !=(State left, State right)
        {
            return Compare(left, right) != 0;
        }
    }
}
