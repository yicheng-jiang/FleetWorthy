using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SalesDetail
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Enter blank line to stop");
            Console.WriteLine("Give the input");
            bool stop = false;
            string input;
            Sales sales = new Sales();

            while (!stop)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    stop = true;
                    break;
                }
                Product item = Sales.parseInput(input);
                if (item != null)
                {
                    sales.AddProduct(item);   
                }
            }
            sales.output();
            Console.ReadLine();
        }
    }

    public class Sales
    {
        public Sales()
        {
            this.products = new Collection<Product>();
        }

        public Collection<Product> products { get; set; }

        public void AddProduct(Product product)
        {
            this.products.Add(product);
        }

        public void output()
        {
            foreach (Product product in products)
            {
                Console.WriteLine(product.count + " " + product.name + ":" + product.Cost().ToString("N2"));
            }
            Console.WriteLine("Sales Taxes:" + this.Taxes().ToString("N2"));
            Console.WriteLine("Total: " + this.Total().ToString("N2"));
        }

        public decimal Total()
        {
            return this.products.Sum(item => item.Cost());
        }

        public decimal Taxes()
        {
            return this.products.Sum(item => item.Tax());
        }

        static bool IsTaxableItem(string name)
        {
            if (name.IndexOf("book") >= 0 || name.IndexOf("chocolate") >= 0 || name.IndexOf("pills") >= 0)
            {
                return false;
            }
            return true;
        }

        static bool IsImportItem(string name)
        {
            if (name.IndexOf("imported") >= 0)
            {
                return true;
            }
            return false;
        }

        static public Product parseInput(string input)
        {
            int count;
            string name;
            decimal price;

            int pos1 = input.IndexOf(" ");
            int.TryParse(input.Substring(0, pos1), out count);
            if (count < 1)
            {
                return null;
            }
            int pos2 = input.IndexOf(" at ");
            name = input.Substring(pos1 + 1, pos2 - pos1 - 1);
            decimal.TryParse(input.Substring(pos2 + 4), out price);
            if (price <= 0)
            {
                return null;
            }
            decimal rate = 0;
            if (IsTaxableItem(name))
            {
                rate = (decimal)0.1;
            }
            if (IsImportItem(name))
            {
                return new ImportProduct(name, count, price, rate, (decimal)0.05);
            }
            return new Product(name, count, price, rate);
        }
    }

    public class Product
    {
        public string name { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public decimal taxRate { get; set; }

        public Product(string name, int count, decimal price, decimal rate)
        {
            this.name = name;
            this.count = count;
            this.price = price;
            this.taxRate = rate;
        }

        public virtual decimal Tax()
        {
            return Math.Ceiling(count * price * taxRate * 20) / 20;
        }

        public decimal Cost()
        {
            return (count * price + Tax());
        }

    }

    public class ImportProduct : Product
    {
        public decimal dutyRate { get; set; }

        public ImportProduct(string name, int count, decimal price, decimal taxRate, decimal dutyRate) : base(name, count, price,taxRate)
        {
            this.dutyRate = dutyRate;
        }

        public override decimal Tax()
        {
            return Math.Ceiling(count * price * (taxRate + dutyRate) * 20) / 20;
        }
    }



}
