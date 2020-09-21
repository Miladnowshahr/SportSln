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
    public class HomeControllerTest
    {

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable<Product>);

            

            HomeController target = new HomeController(mock.Object);
            target.pageSize = 3;

            Func<ViewResult, ProductListViewModel> GetModel = result
                => result?.ViewData?.Model as ProductListViewModel;

            //Action
            int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItem;
            int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItem;
            int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItem;
            int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItem;

            //Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);


        }


        [Fact]
        public void Can_Use_Repository()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
                { 
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"},
                
                }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object) ;

            //Act
            ProductListViewModel result = (controller.Index(null) as ViewResult).ViewData.Model as ProductListViewModel;


            //Assert
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
            ProductListViewModel result = (controller.Index(null, 2) as ViewResult).ViewData.Model as ProductListViewModel;

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
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ ProductId=1,Name="P1"},
                new Product{ ProductId=2,Name="P2"},
                new Product{ ProductId=3,Name="P3"},
                new Product{ ProductId=4,Name="P4"},
                new Product{ ProductId=5,Name="P5"},

            }).AsQueryable<Product>());

            
            HomeController controller = new HomeController(mock.Object) { pageSize = 3 };

            //Arrane
            ProductListViewModel result = (controller.Index(null, 2) as ViewResult).ViewData.Model as ProductListViewModel;

            //Assert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemPerPage);
            Assert.Equal(5, pagingInfo.TotalItem);
            Assert.Equal(2, pagingInfo.TotalPages);

        }
    }

}
