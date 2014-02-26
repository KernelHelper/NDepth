using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    public struct MyCategory
    {
        public static MyCategory Empty = new MyCategory(-1, "<None>");

        public int CategoryId;
        public string CategoryName;

        public MyCategory(int id, string name) { CategoryId = id; CategoryName = name; }

        public static MyCategory[] GetCategories()
        {
            return new[] 
            { 
                new MyCategory(0, "Food"), 
                new MyCategory(1, "Drink"), 
                new MyCategory(2, "Drags") 
            };            
        }
    }

    public struct MyProduct
    {
        public static MyProduct Empty = new MyProduct(-1, -1, "<None>");

        public int ProductId;
        public int CategoryId;
        public string ProductName;

        public MyProduct(int id, int catId, string name) { ProductId = id; CategoryId = catId; ProductName = name; }

        public static MyProduct[] GetProducts()
        {
            return new[] 
            { 
                new MyProduct(0, 0, "Bread"), 
                new MyProduct(1, 0, "Meat"), 
                new MyProduct(2, 1, "Beer"), 
                new MyProduct(3, 1, "Cola"), 
                new MyProduct(4, 3, "Boots"), 
                new MyProduct(5, 4, "Knife") 
            };
        }
    }

    partial class Program
    {
        [Category("Join Operators")]
        [Description("Example of select many join which is equivalent to inner join.")]
        static void LinqSelectManyJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var selectManyJoin = 
                from p in MyProduct.GetProducts()
                from c in MyCategory.GetCategories()
                where (p.CategoryId == c.CategoryId)
                select new { p.ProductName, c.CategoryName };

            // Fluent expression equivalent.
            // var selectManyJoin = MyProduct.GetProducts().SelectMany(p => MyCategory.GetCategories().Where(c => p.CategoryId == c.CategoryId).Select(c => new { p.ProductName, c.CategoryName }));

            Console.WriteLine("Select many join:");
            foreach (var v in selectManyJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of cross join (any with any).")]
        static void LinqCrossJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var crossJoin =
                from p in MyProduct.GetProducts()
                from c in MyCategory.GetCategories()
                select new { p.ProductName, c.CategoryName };

            // Fluent expression equivalent.
            // var crossJoin = MyProduct.GetProducts().SelectMany(p => MyCategory.GetCategories().Select(c => new { p.ProductName, c.CategoryName }));

            Console.WriteLine("Cross join:");
            foreach (var v in crossJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of inner join.")]
        static void LinqInnerJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var innerJoin =
                from p in MyProduct.GetProducts()
                join c in MyCategory.GetCategories() on p.CategoryId equals c.CategoryId
                select new { p.ProductName, c.CategoryName };

            // Fluent expression equivalent.
            // var innerJoin = MyProduct.GetProducts().Join(MyCategory.GetCategories(), p => p.CategoryId, c => c.CategoryId, (p, c) => new { p.ProductName, c.CategoryName });

            Console.WriteLine("Inner join:");
            foreach (var v in innerJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of left outer join.")]
        static void LinqLeftOuterJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var leftOuterJoin =
                from p in MyProduct.GetProducts()
                join c in MyCategory.GetCategories() on p.CategoryId equals c.CategoryId into pc
                from cat in pc.DefaultIfEmpty(MyCategory.Empty) 
                select new { p.ProductName, cat.CategoryName };

            // Fluent expression equivalent.
            // var leftOuterJoin = MyProduct.GetProducts().GroupJoin(MyCategory.GetCategories(), p => p.CategoryId, c => c.CategoryId, (p, cs) => new { p.ProductName, Categories = cs }).SelectMany(tmp => tmp.Categories.DefaultIfEmpty(MyCategory.Empty), (tmp, cat) => new { tmp.ProductName, cat.CategoryName });

            Console.WriteLine("Left outer join:");
            foreach (var v in leftOuterJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of right outer join.")]
        static void LinqRightOuterJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var rightOuterJoin =
                from c in MyCategory.GetCategories()
                join p in MyProduct.GetProducts() on c.CategoryId equals p.CategoryId into pc
                from prod in pc.DefaultIfEmpty(MyProduct.Empty)
                select new { prod.ProductName, c.CategoryName };

            // Fluent expression equivalent.
            // var rightOuterJoin = MyCategory.GetCategories().GroupJoin(MyProduct.GetProducts(), c => c.CategoryId, p => p.CategoryId, (c, ps) => new { Products = ps, c.CategoryName }).SelectMany(tmp => tmp.Products.DefaultIfEmpty(MyProduct.Empty), (tmp, prod) => new { prod.ProductName, tmp.CategoryName });

            Console.WriteLine("Right outer join:");
            foreach (var v in rightOuterJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of full outer join.")]
        static void LinqFullOuterJoin()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var fullOuterJoin = (
                from p in MyProduct.GetProducts()
                join c in MyCategory.GetCategories() on p.CategoryId equals c.CategoryId into pc
                from cat in pc.DefaultIfEmpty(MyCategory.Empty)
                select new { p.ProductName, cat.CategoryName }).Union(
                from c in MyCategory.GetCategories()
                join p in MyProduct.GetProducts() on c.CategoryId equals p.CategoryId into pc
                from prod in pc.DefaultIfEmpty(MyProduct.Empty)
                select new { prod.ProductName, c.CategoryName });

            // Fluent expression equivalent.
            // var fullOuterJoin = (MyProduct.GetProducts().GroupJoin(MyCategory.GetCategories(), p => p.CategoryId, c => c.CategoryId, (p, cs) => new { p.ProductName, Categories = cs }).SelectMany(tmp => tmp.Categories.DefaultIfEmpty(MyCategory.Empty), (tmp, cat) => new { tmp.ProductName, cat.CategoryName })).Union(
            //                      MyCategory.GetCategories().GroupJoin(MyProduct.GetProducts(), c => c.CategoryId, p => p.CategoryId, (c, ps) => new { Products = ps, c.CategoryName }).SelectMany(tmp => tmp.Products.DefaultIfEmpty(MyProduct.Empty), (tmp, prod) => new { prod.ProductName, tmp.CategoryName }));

            Console.WriteLine("Full outer join:");
            foreach (var v in fullOuterJoin)
            {
                Console.WriteLine(v);
            }
        }

        [Category("Join Operators")]
        [Description("Example of group join.")]
        static void LinqGroupJoin1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            /*
            var groupJoin =
                from c in MyCategory.GetCategories()
                join p in MyProduct.GetProducts() on c.CategoryId equals p.CategoryId into pc
                select new { c.CategoryName, Products = pc };
            */ 

            // Fluent expression equivalent.
            var groupJoin = MyCategory.GetCategories().GroupJoin(MyProduct.GetProducts(), c => c.CategoryId, p => p.CategoryId, (c, ps) => new { c.CategoryName, Products = ps });

            Console.WriteLine("Group join:");
            foreach (var c in groupJoin)
            {
                Console.WriteLine("  Category: " + c.CategoryName);
                foreach (var p in c.Products)
                {
                    Console.WriteLine("    " + p.ProductName);
                }
            }
        }

        [Category("Join Operators")]
        [Description("Example of group join with flat collection.")]
        static void LinqGroupJoin2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var groupJoin =
                from c in MyCategory.GetCategories()
                join p in MyProduct.GetProducts() on c.CategoryId equals p.CategoryId into pc
                from prod in pc
                select new { c.CategoryName, prod.ProductName };

            // Fluent expression equivalent.
            // var groupJoin = MyCategory.GetCategories().GroupJoin(MyProduct.GetProducts(), c => c.CategoryId, p => p.CategoryId, (c, ps) => new { c.CategoryName, Products = ps }).SelectMany(tmp => tmp.Products, (tmp, prod) => new { tmp.CategoryName, prod.ProductName });

            Console.WriteLine("Group join with flat collection:");
            foreach (var v in groupJoin)
            {
                Console.WriteLine(v.CategoryName + " - " + v.ProductName);
            }
        }
    }
}
