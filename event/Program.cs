using System;
using System.Threading;

namespace @event
{
    class Program
    {

        /*这是一个模拟顾客进店点餐的代码：
         代码功能按照以下顺序实现：
         顾客进店，坐下，思考，然后呼叫服务员点餐，服务员记账，用餐完毕付账。  */
        static void Main(string[] args)
        {
            Customer customer = new Customer();//实例化事件的拥有着：顾客
            Waiter waiter = new Waiter();//实例化事件的订阅者：服务员
            customer.Order += waiter.Action;//绑定事件的处理器（订阅事件）
            customer.Action();//顾客：开始行动
            customer.PayTheBill();//顾客：付款
            Console.ReadLine();//暂停程序显示数据
        }
    }

    /*声明一个订单（点餐事件的信息），包含菜名和分量两个信息*/
    public class OrderEventArgs : EventArgs//并不强制，但规定凡事件的信息需继承自EventArgs类
    {
        public string DishName { get; set; }//菜名
        public string Size { get; set; }//分量
    }

    /*声明订餐的委托类，注意这是个类，不要声明在其他类中*/
    public delegate void OrderEventHander(Customer customer, OrderEventArgs e);

    /*声明顾客类，其包含以下信息
     订餐事件、走进餐馆、坐下、思考、呼叫服务员点餐的方法，并有付账方法以及一个记录款项总额的字段用于记录服务员提供的订单*/
    public class Customer
    {
        private OrderEventHander orderEventHander;//声明委托成员，用于记录收听订餐事件的方法

        public event OrderEventHander Order;
        /*  //这里是标准的事件定义方法

        public event OrderEventHander Order//声明点餐事件（事件成员）
        {
            add//添加收听该事件的方法
            {
                this.orderEventHander += value;
            }

            remove//移除收听该事件的方法
            {
                this.orderEventHander -= value;
            }
            //这里很像属性
        }
        */


        public double Bill { get; set; }//声明一个用于记录服务员回传款项的字段
        public void PayTheBill()
        {
            Console.WriteLine("Customer：I will pay ${0}.", this.Bill);
        }//顾客埋单

        public void Walkin()
        {
            Console.WriteLine("Customer：Walk in the restaurant.");
        }//顾客走进餐馆

        public void SetDown()
        {
            Console.WriteLine();
        }//顾客坐下

        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Customer：Let me think...");
                Thread.Sleep(1000);
            }
            //顾客想了5秒
            /* //标准的事件调用方法
                if (this.orderEventHander != null)//排除无方法收听的情况
                {
                    OrderEventArgs e = new OrderEventArgs();//实例化一个订单信息
                    e.DishName = "Kongpao Chicken";//顾客点了宫保鸡丁
                    e.Size = "large";//大份宫保鸡丁
                    this.orderEventHander.Invoke(this, e);//呼叫服务员（执行订餐委托）
                    */
            if (this.Order != null)//排除无方法收听的情况
            {
                OrderEventArgs e = new OrderEventArgs();//实例化一个订单信息
                e.DishName = "Kongpao Chicken";//顾客点了宫保鸡丁
                e.Size = "large";//大份宫保鸡丁
                this.Order.Invoke(this, e);//呼叫服务员（执行订餐委托）

            }
        }//顾客思考并下单

        public void Action()//顾客进店以来点餐前的行动
        {
            this.Walkin();
            this.SetDown();
            this.Think();
        }
    }

    /*声明一个服务员类，包含以下信息：
     服务员收听到顾客的召唤（执行订餐委托）所执行的方法*/
    public class Waiter
    {
        public void Action(Customer customer, OrderEventArgs e)//收听订餐委托的方法，接受顾客身份和一道菜的订单
        {
            Console.WriteLine("Waiter：I will serve you the dish -{0}", e.DishName);//为顾客报菜名
            double price = 10;//设置基准价格
            switch (e.Size)//大份菜是基准价格的1.5倍，小份菜是基准价格的0.5倍
            {
                case "large":
                    price *= 1.5;
                    break;
                case "small":
                    price *= 0.5;
                    break;
                default: break;
            }

            customer.Bill += price;//为顾客的账单添加一道菜的款项
        }
    }

    /*
     * 事件的拥有者：Customer
     * 事件成员：Order
     * 事件的响应者：Waiter
     * 事件处理器：wait.Action(Customer customer, OrderEventArgs e)
     * 事件订阅：customer.Order += waiter.Action;
     */
}
