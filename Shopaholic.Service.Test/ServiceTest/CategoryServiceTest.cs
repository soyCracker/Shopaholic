using NUnit.Framework;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using Shopaholic.Util.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Test.ServiceTest
{
    public class CategoryServiceTest
    {
        private CategoryService categoryService;
        

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void AddTest()
        {
            //Arrange
            string categoryName = "nunit_test";
            string categoryName2 = "nunit_test2";
            bool res1 = false;
            bool res2 = false;
            bool res3 = false;
            List<CategoryDTO> list = new List<CategoryDTO>();
            List<CategoryDTO> list2 = new List<CategoryDTO>();
            CategoryDTO dto = null;
            CategoryDTO dto2 = null;

            //Act
            categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
            res1 = categoryService.AddCategory(categoryName);

            categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
            dto = categoryService.GetCategory(categoryName);

            if (dto != null)
            {
                categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
                res2 = categoryService.UpdateCategory(dto.Id, categoryName2);

                categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
                list = categoryService.GetCategoryList();

                categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
                res3 = categoryService.DeleteCategory(dto.Id);

                categoryService = new CategoryService(DbContextUtil.GetDbContextFromMemory());
                list2 = categoryService.GetCategoryList();
            }

            //Assert
            Assert.IsTrue(res1 && res2 && res3);
            Assert.AreEqual(categoryName, dto.Name);
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(categoryName2, list[0].Name);
            Assert.Zero(list2.Count());
        }
    }
}
