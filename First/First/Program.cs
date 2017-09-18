using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace First
{
    /*
     * Реализация игры Jammed. 
     * Программа считывает из указанных файлов начальное и конечной состояние игрового поля
     * и определяет, за какое минимальное количество перемещений пустой ячейки можно получить это состояние. 
     * Ответ записывается в указанный файл.
     * Для однозначного определения минимального количества перемещений используется очередь.
     * Для хранения состояний игрового поля и количества перемещений до их получения используется словарь.
     * Для описания состояния игрового поля создан отдельный класс.
     * Так как класс - ссылочный тип, пришлось реализовать специальные методы,
     * которые при копировании состояний игрового поля или их сравнения
     *  копируют/сравнивают сами состояния, а не ссылки на них.
    */
    class State //Этот класс описывает состояние игрового поля
    {
        public char[,] a = new char[2, 4];//В этом массив записывается само поле (каждый элемент массива соответствует элементу игрового поля)
        public State(State t)//Конструктор, описывает состояние игрвого поля на основе заданного состояния (по сути копирует его)
        {
            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = t.a[i, j];

                }

            }
        }
        public State (char [,] t)
        {
            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = t[i, j];

                }

            }
        }
        public State()//Конструктор, создаёт пустое игровое поле (на нём нет элементов)
        {
            a = new char[2, 4];
        }
        static public FileStream fs = new FileStream("INPUT.txt", FileMode.Open);//Поток для считывания входных данных из указанного файла
        static public StreamReader sr = new StreamReader(fs);//Считывание строк из указанного потока
        public static State Read()//Метод для чтения данных из файла и копирования их на игровое поле
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
        public State Shift(int di, int dj)//Сдвиг пустой ячейки в указанном игровом поле на указанное количество клеток вверх/вниз и влево/вправо
        {
            State next = new State(this);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (a[i, j] == '#')//Если найдена пустая ячейка
                    {
                        int ni = i + di;//i координата нового положения игрового поля
                        int nj = j + dj;//j координата нового положения игрового поля
                        if (0 <= ni && ni < 2 && 0 <= nj && nj < 4)//Если после перемещения пустая ячейка не окажется вне игрового поля - её перемещение
                        {
                            char t = next.a[i, j];
                            next.a[i, j] = next.a[ni, nj];
                            next.a[ni, nj] = t;
                        }
                        return next;//Возвращение нового состояния игрового поля
                    }
                }
            }
            return next;//если пустую ячейку нельзя передвинуть в указанном направлении (выход за игровое поле) - возвращение неизменённого игрового поля

        }
        static public int Compare(State left, State right)//Метод сравнения указанных состояний игрового поля (если вернулся 0 - значит сосотояния совпадают, если -1 - нет)
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
        static public bool operator ==(State left, State right)//Перегрузка оператора ==
        {
            return Equals(left, right);
        }
        static public bool operator !=(State left, State right)//Перегрузка оператора !=
        {
            return !Equals(left, right);
        }
        public bool Equals(State other)//Метод сравнения указанных состояний игрового поля (для словаря)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Compare(this, other) != 0) return false;
            if (Compare(this, other) == 0) return true;
            return false;
        }
        public override bool Equals(object obj)//Перегрузка Equals
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State)obj);
        }
        public override int GetHashCode()//Перегрузка метода получения хеш-кода(для словаря)
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
            State start = State.Read();//Считывание стартовой позиции игрового поля
            State finish = State.Read();//Считывание образца (финальной позиции) игрового поля
            State.fs.Close();
            State.sr.Close();
            Dictionary<State, int> len = new Dictionary<State, int>();//Словарь, где ключ - определённое состояние игрового поля, а значение - количество перемещений пустой ячейки для достижения этого состояния
            Queue<State> q = new Queue<State>();//Очередь возможных при перемещении пустой ячейки состояний
            len[start] = 0;
            q.Enqueue(start);
            FileStream fs = new FileStream("OUTPUT.txt", FileMode.Create);//Создание потока для записи выходных данных в указанный файл
            StreamWriter sw = new StreamWriter(fs);//Запись данных в указанный поток
            while (q.Count != 0)//Пока не закончатся варианты перемещения пустой ячейки
            {
                State cur = new State(q.Dequeue());//cur - текущее состояние игрового поля. Извлечение из очереди состояния и его запись в cur 
                if (cur == finish)//Если текущее состояние совпадает с образцом, то программа записывает в исходный файл количество перемещений до достижения этого состояния и завершает работу
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
                        if (di * di + dj * dj == 1)//Определение варинтов перемещения пустой клетки (её можно переместить только на одну клетку вниз/вверх/вправо/влево)
                        {
                            State next = cur.Shift(di, dj);//next - состояние игрового поля после перемещения. Сдвиг пустой ячейки в состояние cur и запись получившего состояния в next
                            if (!len.ContainsKey(next))//Если в словаре нет получившегося состояния игрового поля - добавление его в словарь и очередь
                            {
                                len.Add(next, len[cur] + 1);
                                q.Enqueue(next);

                            }                            
                        }
                    }
                }
            }                                
            
           
        }
    }
}
