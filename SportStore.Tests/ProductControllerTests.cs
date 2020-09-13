using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using SportStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
    public class ProductControllerTests
    {
        
        [Fact]
        public void Can_Use_Repository()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"}
            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            ////Act
            var result =
               (controller.Index() as ViewResult).ViewData.Model as ProductListViewModel;


            ////Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P1", prodArray[0].Name);
            Assert.Equal("P2", prodArray[1].Name);

        }



        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"},
                new Product{ProductId=3,Name="P3"},
                new Product{ProductId=4,Name="P4"},
                new Product{ProductId=5,Name="P5"},

            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            controller.pageSize = 3;

            //Act

            var result =
                (controller.Index(2) as ViewResult).ViewData.Model as ProductListViewModel;



            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);

        }


        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"},
                new Product{ProductId=3,Name="P3"},
                new Product{ProductId=4,Name="P4"},
                new Product{ProductId=5,Name="P5"},

            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object) { pageSize = 3 };

            //Act

            ProductListViewModel result = (controller.Index(2) as ViewResult ).ViewData.Model as ProductListViewModel;


            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemPerPage);
            Assert.Equal(5, pageInfo.TotalItem);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
