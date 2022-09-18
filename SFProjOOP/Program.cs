using System;
using System.Text;

namespace Program
{
    class MainProgram
    {
        public static void Main()
        {
            var order1 = new Order<Delivery>(new HomeDelivery());
            var order2 = new Order<Delivery>(new PickPointDelivery());
            var order3 = new Order<Delivery>(new ShopDelivery());
            
            order1.Description = "Ноутбук";
            order1.Address = "Москва";
            
            order2.Description = "Смартфон";
            order2.Address = "Санкт-Петербург";
            
            order3.Description = "Монитор";
            order3.Address = "Минск";

            MyOrders orders = MyOrders.InitializeOrders();

            orders.addOrder<Delivery>(order1);
            Console.WriteLine(orders);

            orders.addOrder<Delivery>(order2, order3);
            Console.WriteLine(orders);

            Console.ReadLine();
        }
    }

    abstract class Delivery
    {
        public abstract override string ToString();
    }

    class HomeDelivery : Delivery
    {
        public override string ToString()
        {
            return "Доставка на дом.";
        }
    }

    class PickPointDelivery : Delivery
    {
        public override string ToString()
        {
            return "Доставка в пункт выдачи.";
        }
    }

    class ShopDelivery : Delivery
    {
        public override string ToString()
        {
            return "Доставка в магазин.";
        }
    }

    class Order<TDelivery> where TDelivery : Delivery
    {
       
        //счетчик id, придающий кажому номеру заказа Number уникальное число
        public static int id { get; private set; }
        //проверка на то, что установилось ли свойство в 0 в конструкторе
        private static bool idIsInit = false;
        //уникальный номер каждого заказа
        public int Number { get; private set; }
        
        //описание заказа
        public string Description;
        //адресс заказа
        public string Address;
        //экземпляр класса доставки
        public TDelivery Delivery;

        public void DisplayAddress()
        {
            Console.WriteLine(Address);
        }

        public Order(TDelivery delivery, string address = "Нет адресса", string description = "Нет описания")
        {
            if (!idIsInit)
            {
                id = 0;
                idIsInit = true;
            }

            Number = ++id;
            Delivery = delivery;
            Address = address;
            Description = description;
        }

        //перегруженный метод ToString класса object 
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Delivery.ToString() + $" Номер заказа: {Number}. Адресс доставки: {Address}. Описание заказа: {Description}.\n");
            return stringBuilder.ToString();
        }
    }

    //В данном классе реализован паттерн Одиночка(Singletone)
    class MyOrders
    {
        //ссылка на единственный экземпляр данного класса
        public static MyOrders RefOnExemplar { get; private set; }
        public static bool isInit { get; private set; }

        //массив заказов
        static Order<Delivery>[] orders;

        //статическая функция инициализации единственного экземпляра
        public static MyOrders InitializeOrders()
        {
            if (isInit == false)
            {
                isInit = true;
                return RefOnExemplar = new MyOrders();
            }
            return RefOnExemplar;
        }
        
        //закрытый конструктор для реализации паттерна
        private MyOrders()
        {

        }

        //метод добавления нового заказа
        public void addOrder<TDelivery>(params Order<Delivery>[] newOrders)
        {
            int lastSize = (orders == null) ? 0 : orders.Length;
            Helper.Resize(ref orders, lastSize + newOrders.Length);
            for (int i = 0; i < newOrders.Length; i++)
            {
                orders[lastSize + i] = newOrders[i];
            } 
            
        }

        //перегрузка оператора индексирования
        public Order<Delivery> this[int index]
        {
            get
            {
                return (0 <= index && index < orders.Length) ? orders[index] : null;
            }
            set
            {
                if (0 <= index && index < orders.Length) orders[index] = value;
            }
        }

        //перегрузка ToString()
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Order<Delivery> order in orders)
            {
                stringBuilder.Append(order);
            }
            return stringBuilder.ToString();
        }
    }

    //Статический класс, в котором реализованы вспомогательные методы
    static class Helper
    {
        //метод для увеличения размера массива
        public static void Resize<T>(ref T[] array, int n)
        {
            if (array?.Length >= n) return;

            T[] tempArray = new T[n];
            for(int i = 0; i < array?.Length; i++)
            {
                tempArray[i] = array[i];
            }

            array = tempArray;
        }

        //метод для вывода элементов массива
        public static void Display<T>(in T[] array) 
        {
            foreach(T item in array)
            {
                Console.WriteLine(item);
            }
        }
    }

    enum TypeOfDeliveries
    {
        HomeDelivery,
        PickPointDelivery,
        ShopDelivery
    }
}