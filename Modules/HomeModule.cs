using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace ToDo
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {

      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      // Get["/"] = _ => {
      //   List<Category> allCategories = Category.GetAll();
      //   return View["index.cshtml", allCategories];
      // };
      Get["/tasks"] = _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };

      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };



      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/tasks/new"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["tasks_form.cshtml", AllCategories];
      };
      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["task-description"], Request.Form["category-id"], Request.Form["due-date"]);
        newTask.Save();
        return View["success.cshtml"];
      };
      Post["/tasks/delete"] = _ => {
        Task.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category SelectedCategory = Category.Find(parameters.id);
        List<Task> CategoryTasks = SelectedCategory.GetTasks();
        model.Add("category", SelectedCategory);
        model.Add("tasks", CategoryTasks);
        return View["category.cshtml", model];
      };
      Get["category/edit/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        return View["category_edit.cshtml", SelectedCategory];
      };
      Patch["category/edit/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        SelectedCategory.Update(Request.Form["category-name"]);
        return View["success.cshtml"];
      };
      Get["category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        return View["category_delete.cshtml", SelectedCategory];
      };
      Delete["category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        SelectedCategory.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
