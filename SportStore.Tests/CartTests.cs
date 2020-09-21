using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //Arrange
            //Create Some test products

            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };


            //Arrange
            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] result = target.Lines.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(p1, result[0].Product);
            Assert.Equal(p2, result[1].Product);

        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {

            //Arrange 
            //Create Some test product
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };

            //Arrange -create a new cart
            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductId).ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Arrange
            //Create some test products
            Product p1 = new Product { Name = "P1", ProductId = 1 };
            Product p2 = new Product { Name = "P2", ProductId = 2 };
            Product p3 = new Product { Name = "P3", ProductId = 3 };

            //Arrange create a new Cart

            Cart target = new Cart();

            //Arrane - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            //Act
            target.RemoveLine(p2);

            //Assert
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count());

        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //Arrane create some test product
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            //Arrane -create a new cart
            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            //Assert
            Assert.Equal(450M, result);
        }
        [Fact]
        public void Can_Clear()
        {
            //Arrane
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P1", Price = 100M };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);

            //act
            target.Clear();

            //Assert
            Assert.Empty(target.Lines);

        }
    }
}
