using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

using SportStore.Models;
using SportStore.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
    public class CartPageTests
    {
        
        [Fact]
        public void Can_Load_Cart()
        {
            //Arrane
            //Create a mock repository
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };

            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[]
            {
                p1,p2
            }).AsQueryable<Product>());

            //Create a cart
            Cart TestCart = new Cart();
            TestCart.AddItem(p1, 2);
            TestCart.AddItem(p2, 1);

            // - create a mock page context and session
            Mock<ISession> mockSession = new Mock<ISession>();
            byte[] data = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(TestCart));

            mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(),out data));

            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

            //Action
            CartModel cartModel = new CartModel(mockRepo.Object)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext(new Microsoft.AspNetCore.Mvc.ActionContext
                {
                    HttpContext=mockContext.Object,
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                })
            };
            cartModel.OnGet("myUrl");

            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl",cartModel.ReturnUrl);

        }


        [Fact]
        public void Can_Update_Cart()
        {
            //Arrange
            //Create a mock repository
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[]{
                new Product{ProductId=1,Name="P1"}
            }).AsQueryable<Product>());


            Cart testCart = new Cart();

            Mock<ISession> mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) =>
                {
                    testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
                });

            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

            //Action
            CartModel cartModel = new CartModel(mockRepo.Object)
            {
                PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext(new Microsoft.AspNetCore.Mvc.ActionContext
                {
                    HttpContext = mockContext.Object,
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new PageActionDescriptor(),
                })
            };

            cartModel.OnPost(1, "myUrl");

            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);

        }


    }
}
