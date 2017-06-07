using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDo
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");

      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange, Act
      Category testCategory = new Category("Garden");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    // [Fact]
    // public void Test_Save_AssignsIdToCategoryObject()
    // {
    //   Category testCategory = new Category("Shopping");
    //   testCategory.Save();
    //
    //   Category savedCategory = Category.GetAll()[0];
    //
    //   int result = savedCategory.GetId();
    //   int testId = testCategory.GetId();
    //
    //   Assert.Equal(testId, result);
    // }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      Category testCategory = new Category("Laundry");
      testCategory.Save();

      Category foundCategory = Category.Find(testCategory.GetId());

      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_GetTasks_RetrievesAllTasksWithCategory()
    {
      Category testCategory = new Category("Cleaning");
      testCategory.Save();

      Task firstTask = new Task("Take out the trash", testCategory.GetId(), "1/2/17");
      firstTask.Save();
      Task secondTask = new Task("Mop the floor", testCategory.GetId(), "1/2/17");
      secondTask.Save();

      List<Task> testTaskList = new List<Task>{firstTask, secondTask};
      List<Task> resultTaskList = testCategory.GetTasks();

      Assert.Equal(testTaskList, resultTaskList);

    }

    [Fact]
    public void Test_Update_UpdatesCategoryInDatabase()
    {
      string name = "Home stuff";
      Category testCategory = new Category(name);
      testCategory.Save();
      string newName = "Work stuff";

      testCategory.Update(newName);

      string result = testCategory.GetName();

      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryFromDatabase()
    {
      string name1 = "Home stuff";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Work stuff";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      Task testTask1 = new Task("mow the lawn", testCategory1.GetId(), "1/2/17");
      testTask1.Save();
      Task testTask2 = new Task("wash the dishes", testCategory2.GetId(), "1/2/17");
      testTask2.Save();

      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      List<Task> resultTasks = Task.GetAll();
      List<Task> testTaskList = new List<Task> {testTask2};

      Assert.Equal(testCategoryList, resultCategories);
      Assert.Equal(testTaskList, resultTasks);
    }

    public void Dispose()
    {
      Category.DeleteAll();
      Task.DeleteAll();
      // Console.WriteLine(Task.GetAll().Count);
      // Console.WriteLine(Category.GetAll().Count);
    }
  }
}
