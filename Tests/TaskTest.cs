using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDo
{
  [Collection("ToDo")]

  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", "1/2/17");
      Task secondTask = new Task("Mow the lawn", "1/2/17");

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", "1/2/17");

      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      Task testTask = new Task("Do the dishes", "1/2/17");
      testTask.Save();

      Task foundTask = Task.Find(testTask.GetId());


      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_AddCategory_AddsCategoryToTask()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", "1/2/17");
      testTask.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Task_GetCategories_ReturnsAllTaskCategories()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", "1/2/17");
      testTask.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      //Act
      testTask.AddCategory(testCategory1);
      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      //Assert
      Assert.Equal(testList, result);
    }


    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
