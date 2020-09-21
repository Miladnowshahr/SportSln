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
               (controller.Index(null) as ViewResult).ViewData.Model as ProductListViewModel;


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
                new Product{ProductId=1,Name="P1",Category=""},
                new Product{ProductId=2,Name="P2",Category=""},
                new Product{ProductId=3,Name="P3",Category=""},
                new Product{ProductId=4,Name="P4",Category=""},
                new Product{ProductId=5,Name="P5",Category=""},

            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            controller.pageSize = 3;

            //Act

            var result =
                (controller.Index("",2) as ViewResult).ViewData.Model as ProductListViewModel;



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

            ProductListViewModel result = (controller.Index("",2) as ViewResult ).ViewData.Model as ProductListViewModel;


            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemPerPage);
            Assert.Equal(5, pageInfo.TotalItem);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Product()
        {
            //Arrange
            //create mock repository
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1",Category="Cat1"},
                new Product{ProductId=2,Name="P2",Category="Cat2"},
                new Product{ProductId=3,Name="P3",Category="Cat1"},
                new Product{ProductId=4,Name="P4",Category="Cat2"},
                new Product{ProductId=5,Name="P5",Category="Cat3"},

            }).AsQueryable<Product>());

            //Arrange
            //create controller and make the page size 3 item
            HomeController homeController = new HomeController(mock.Object);
            homeController.pageSize = 3;

            //Action
            Product[] result = ((homeController.Index("Cat2", 1) as ViewResult).ViewData.Model as ProductListViewModel).Products.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
               

        }
    }
}
