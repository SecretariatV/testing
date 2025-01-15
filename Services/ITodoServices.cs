using Test2Server.Data.Entities;

namespace Test2Server.Services
{
    public interface ITodoService
    {
        Task<TodoEntity> CreateTodoList(TodoEntity list);
        Task<IEnumerable<TodoEntity>> GetTodosByUserId(Guid userId);
        Task<bool> DeleteTodo(Guid todoId);
        Task<bool> UpdateTodo(Guid userId, Guid todoId, TodoEntity todo);
    }
}