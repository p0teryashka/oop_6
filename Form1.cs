using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба_ооп6
{
    public partial class Form1 : Form
    {
        Storage<Shape> MyStorage = new Storage<Shape>();//хранилище фигурок
        public Form1()
        {
            InitializeComponent();
            (pictureBox1 as Control).KeyPress += new KeyPressEventHandler(PressEventHandler);//считывание нажатия мышки и выделение после 
        }

        public void PressEventHandler(object sender, KeyPressEventArgs e)
        {
            if (MyStorage.size() != 0)
            {//движение фигуры и назначение других клавиш 
                if (e.KeyChar == 119) MyStorage.get().OffsetXY(0, -1); 
                if (e.KeyChar == 115) MyStorage.get().OffsetXY(0, 1);
                if (e.KeyChar == 97) MyStorage.get().OffsetXY(-1, 0);
                if (e.KeyChar == 100) MyStorage.get().OffsetXY(1, 0);
                if (e.KeyChar == 98) MyStorage.get().Grow(1);//увеличение
                if (e.KeyChar == 118) MyStorage.get().Grow(-1);//уменьшение 
                if (e.KeyChar == 122) MyStorage.prevCur();//последующий
                if (e.KeyChar == 120) MyStorage.nextCur();//предыдущий 
                if (e.KeyChar == 8) MyStorage.del();//удалить
                if (e.KeyChar == 32)//пробел удаление 
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }



                if (e.KeyChar == 110)//удаление через клавишу n
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }
                if (e.KeyChar == 99)//цвет
                {
                    
                    if (colorDialog1.ShowDialog() == DialogResult.OK) MyStorage.get().SetColor(colorDialog1.Color);
                }

                Random rnd = new Random();
                int obj = rnd.Next(1, 3);
                if (e.KeyChar == 49)//1
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new CCircle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 51)//3
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Rhombus(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 50)//2
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Triangle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }

            }
            pictureBox1.Invalidate();//перерисовка
        }



        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)//метод рисовки
        {
            if (MyStorage.size() != 0)//если не 0 в хранилище
            {
                MyStorage.toFirst();//переходит к 1 отрисованной
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())//перебирает все до последнего
                {
                    MyStorage.getIterator().DrawObj(e.Graphics);//отрисовка 
                    //выделение на ПКМ
                    if (MyStorage.isChecked() == true) MyStorage.getIterator().DrawRectangle(e.Graphics, new Pen(Color.Yellow, 2));
                }
                //выделение на ЛКМ
                MyStorage.get().DrawRectangle(e.Graphics, new Pen(Color.Red, 1));
            }

        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            bool isFinded = false; //булевая переменная которая проверяет на попадание мышки на текстуру
            if (MyStorage.size() != 0)
            {
                MyStorage.toFirst();
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())
                {
                    //если лкм то выполняем функцию setCurPTR и создаем красное выделение
                    if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Left)
                    {
                        isFinded = true; //булевая переменная которая проверяет на попадание мышки на текстуру 
                        MyStorage.setCurPTR();
                        break;
                    }//если пкм то выполняем функцию check и создаем желтое выделение
                    if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Right)
                    {
                        isFinded = true; //булевая переменная которая проверяет на попадание мышки на текстуру
                        MyStorage.check();
                        break;
                    }
                }
            }
            if (isFinded == false) //булевая переменная которая проверяет на попадание мышки на текстуру
            {
                if (radioButton1.Checked == true)//если тыкаем на пустое место, то создастся новая фигура круг
                {
                    Random rnd = new Random();//переменная рандома, в которой функция рандома
                    int rad = rnd.Next(10, 100);//рандомный радиус от 10 до 100
                    //добавление случайной фигуры с рандомным цветом
                    MyStorage.add(new CCircle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton2.Checked == true)//если тыкаем на пустое место, то создастся новая фигура ромб
                {
                    Random rnd = new Random();//переменная рандома, в которой функция рандома
                    int rad = rnd.Next(10, 100);//рандомный радиус от 10 до 100
                    //добавление случайной фигуры с рандомным цветом
                    MyStorage.add(new Rhombus(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton3.Checked == true)//если тыкаем на пустое место, то создастся новая фигура треугольник
                {
                    Random rnd = new Random();//переменная рандома, в которой функция рандома
                    int rad = rnd.Next(10, 100);//рандомный радиус от 10 до 100
                    //добавление случайной фигуры с рандомным цветом
                    MyStorage.add(new Triangle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }

            }
            //перерисовка фигуры
            pictureBox1.Invalidate();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
        //фокус ввода для элемента управления
            pictureBox1.Focus();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

};
public abstract class Shape : ObjObserved
{
    protected Rectangle rect; //область объекта для его отрисовки и выделения
    protected int X, Y, width, height; //x, y для позиции объектов и групп, w и h для учитывания границ отрисовки
    public bool sticky = false;
    abstract public void Resize();//переделываение размера фигуры
    abstract public void SetXY(int _x, int _y);//установка положения фигуры
    abstract public void OffsetXY(int _x, int _y); //считывание с мышки положения
    abstract public void SetColor(Color c);//постановка цвета
    abstract public void Grow(int gr);//увеличение и уменьшение фигуры 
    abstract public void DrawObj(System.Drawing.Graphics e); // отрисовка фигуры
    abstract public void DrawRectangle(System.Drawing.Graphics e, Pen pen);//отрисовка границы фигуры
    abstract public bool Find(int _x, int _y);//функция проверки попадания мышки по фигуре 
    abstract public Rectangle GetRectangle();  //получить границы фигуры для контроля выхода за пределы
}

public class CCircle : Shape
{
    protected Color color;
    protected int R;
    //конструктор
    public CCircle()
    {

        X = 0;
        Y = 0;
        R = 0;
    }
    //конст с параметрами
    public CCircle(int x, int y, int r, Color c, int Width, int Height)
    {
        //описание присвоения координат или значений от пользователя к нашей фигуре
        this.X = x;
        this.Y = y;
        width = Width;
        height = Height;
        color = c;
        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        R = r;
        rect = new Rectangle(x - R, y - R, 2 * R, 2 * R);

    }
    public override void SetXY(int x, int y)
    {
        //получение координат по мышке ( заранее описанный метод ) 
        throw new NotImplementedException();
    }
    public override void Resize()
    {//для изменения размера фигуры (увеличения и уменьшения)
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void Grow(int inc)//для увеличения и уменьшения фигуры 
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void SetColor(Color c)
    {//выбирается и ставится цвет 
        color = c;
    }

    public override void DrawObj(Graphics e)
    {//метод отрисовки фигуры
        e.FillEllipse(new SolidBrush(color), rect);
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {//отрисовка границы фигуры
        e.DrawRectangle(pen, rect);
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }
    public override Rectangle GetRectangle()
    {//геттер для GetRectangle
        return rect;
    }

    public override bool Find(int _x, int _y)
    {//для проверки мышка попала или нет 
        if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= R * R) return true; else return false;
    }
}
public class Rhombus : CCircle //реализация ромба 
{
    private int n = 4;
    List<PointF> first;

    public Rhombus(int x, int y, int r, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {

        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        Resize();
    }

    public override void DrawObj(Graphics e)
    {
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = 0; i < 360; i += 360 / n)
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R); //границы красные
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {

    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
}

public class Triangle : CCircle //реализация треугольника 
{
    private int n = 3;
    List<PointF> first;

    public Triangle(int x, int y, int r, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {

        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height -y;
        Resize();
    }

    public override void DrawObj(Graphics e)
    {
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = 30; i < 390; i += 360 / n) //настройка вершин треугольника
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {

    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
}



public class ObjObserved
{
    public Storage<Shape> storage;
    //для обновления хранилища 
    public void AddStorage(Storage<Shape> MyStorage)
    {
        storage = MyStorage;
    }
}


public class Storage<MStorage> //реализация хранилища
{
    public class list
    {
        public MStorage data { get; set; }// сам список 
        public list right { get; set; }//вспомогательные средства для перемещения по списку направо и налево 
        public list left { get; set; }//вспомогательные средства для перемещения по списку направо и налево 
        public bool isChecked = false;//нынешний обьект

    };
    private list first;//первый объект
    private list last;//второй объект
    private list current;//нынешний объект
    private list iterator;//индекс объекта

    private int rate;//переменная для помощи индекса 
    public Storage()
    {
        first = null;
        rate = 0;
    }
    public void add(MStorage figure) //метод добавления фигуры в харинилще 
    {
        list tmp = new list();//дополнительный список 
        tmp.data = figure; //присвоение фигуры 
        if (first != null)
        {//создание элементов с сохранением последовательности и прежде идущих
            tmp.left = last;
            last.right = tmp;
            last = tmp;
        }
        else
        //в случае если не было найдено фигур 
        {
            first = tmp;
            last = first;
            current = first;
        }
        last.right = first;
        current = tmp;
        first.left = last;
        rate++;
    }
    public void addBefore(MStorage figure) //добавление в хранилище ПЕРЕД
    {//для работы с предыдущими элементами 
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = (current.left);
            (current.left).right = tmp;
            current.left = tmp;
            tmp.right = current;
            if (current == first) first = current.left;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void addAfter(MStorage figure)//добавление в хранилище ПЕРЕД
    {//для работы с последующими элементами 
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = current;
            tmp.right = current.right;
            (current.right).left = tmp;
            current.right = tmp;
            if (current == last) last = current.right;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void toFirst()//реализация метода который позволяет перейти к первому обьекту 
    {
        iterator = first;
    }
    public void toLast()//реализация метода который позволяет перейти к последнему обьекту 
    {
        iterator = last;
    }
    public void next()//индекс последующего элемента 
    {
        iterator = iterator.right;
    }
    public void prev()//индекс предыдущего элемента 
    {
        iterator = iterator.left;
    }
    public void nextCur()//изменение обьектов на последующий
    {
        current = current.right;
    }
    public void prevCur()//изменение обьектов на предыдущий
    {
        current = current.left;
    }
    public void del()//метод удаления самих фигур с экрана 
    {
        if (rate == 1)//случай когда фигура одна и некуда двигаться 
        {
            first = null;
            last = null;
            current = null;
        }
        else
        {//соблюдение индекосв с учетом удаления 
            (current.left).right = current.right;
            (current.right).left = current.left;
            list tmp = current;
            if (current == last)
            {
                current = current.left;
                last = current;
            }
            else
            {
                if (current == first) first = current.right;
                current = current.right;
            }
        }
        rate--;
    }
    public void delIterator()//метод удаления индексов обьектов 
    {
        if (rate == 1)
        {
            first = null;
            last = null;
            iterator = null;
        }
        else
        {
            (iterator.left).right = iterator.right;
            (iterator.right).left = iterator.left;
            if (iterator == last)
            {
                iterator = iterator.left;
                last = iterator;
            }
            else
            {
                if (iterator == first) first = iterator.right;
                iterator = iterator.left;
            }
        }
        rate--;
    }
    public int size() //размер хранилища 
    {
        return rate;
    }
    public list getIteratorPTR() //геттер для итераторов 
    {
        return iterator;
    }
    public list getCurPTR() // геттер для фигур нынешних 
    {
        return current;
    }
   
    public void setCurPTR() //сеттер для нынешних 
    {
        current = iterator;
    }
    public bool isChecked() //проверка на выделение 
    {
        if (iterator.isChecked == true) return true; else return false;
    }
    public void check() //выделение 
    {
        iterator.isChecked = !iterator.isChecked;
    }
    public MStorage getIterator() //геттер для итераторов 
    {
        return (iterator.data);
    }
    public MStorage get() // получение значения с даты 
    {
        return (current.data);
    }
}
