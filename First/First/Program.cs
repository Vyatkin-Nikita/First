using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace First
{
    class Program
    {
        static void Main(string[] args)
        {
            State start = State.Read();
            State finish = State.Read();
            Dictionary<State, int> len = new Dictionary<State, int>();
            Queue<State> q = new Queue<State>();
            len[start] = 0;
            q.Enqueue(start);
            FileStream fs = new FileStream("OUTPUT.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            int r = 0;
            while (q.Count != 0)
            {
               
                State cur = q.Dequeue();
                if (cur == finish)
                {                    
                    sw.Write(len[cur]);
                    Console.ReadLine();
                    sw.Close();
                    fs.Close();
                    Environment.Exit(0);
                }
                for (int di = -1; di <= 1; di++)
                {
                    for(int dj = -1; dj <= 1; dj++)
                    {
                        if (di * di + dj * dj == 1)
                        {
                            State next = cur.Shift(di,dj);
                            if (!len.ContainsKey(next))
                            {
                                len[next] = len[cur] + 1;                                                            
                                q.Enqueue(next);
                                
                            }
                            r++;
                        }
                    }
                }
            }
            sw.Write(-1);
            Console.WriteLine(r);
            sw.Close();
            fs.Close();
            Console.ReadLine();
           
        }
    }
}
