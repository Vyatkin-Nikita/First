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
        public char[,] a = new char[2, 4];
        public State(State t)
        {
            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = t.a[i, j];

                }

            }
        }
        public State()
        {
            a = new char[2, 4];
        }
        static public FileStream fs = new FileStream("INPUT.txt", FileMode.Open);
        static public StreamReader sr = new StreamReader(fs);
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

                }

            }           
            return s;
        }
        public State Shift(int di, int dj)
        {
            State next = new State(this);
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
        static public int Compare(State left, State right)
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
        static public bool operator ==(State left, State right)
        {
            return Equals(left, right);
        }
        static public bool operator !=(State left, State right)
        {
            return !Equals(left, right);
        }
        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Compare(this, other) != 0) return false;
            if (Compare(this, other) == 0) return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int t = 0;
                for (int i = 0; i < 2; i++)
                {

                    for (int j = 0; j < 4; j++)
                    {
                        t += a[0, 0].GetHashCode();

                    }

                }
                return t;
            }
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            State start = State.Read();
            State finish = State.Read();
            State.fs.Close();
            State.sr.Close();
            Dictionary<State, int> len = new Dictionary<State, int>();
            Queue<State> q = new Queue<State>();
            len[start] = 0;
            q.Enqueue(start);
            FileStream fs = new FileStream("OUTPUT.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            int r = 0;
            while (q.Count != 0)
            {

                State cur = new State(q.Dequeue());
                if (cur == finish)
                {
                    sw.Write(len[cur]);                   
                    sw.Close();
                    fs.Close();
                    break;
                }
                for (int di = -1; di <= 1; di++)
                {
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        if (di * di + dj * dj == 1)
                        {
                            State next = cur.Shift(di, dj);
                            if (!len.ContainsKey(next))
                            {
                                len.Add(next, len[cur] + 1);
                                q.Enqueue(next);

                            }
                            r++;
                        }
                    }
                }
            }                                
            
           
        }
    }
}
