using Microsoft.EntityFrameworkCore;
using Test2Server.Data;
using Test2Server.Data.Entities;

namespace Test2Server.Services
{
    public class todoService : ITodoService
    {
        private readonly AppDbContext _dbContext;

        public todoService(AppDbContext dbContext)
        { _dbContext = dbContext; }

        public async Task<TodoEntity> CreateTodoList(TodoEntity list)
        {
            await _dbContext.todos.AddAsync(list);
            await _dbContext.SaveChangesAsync();
            return list;
        }

        public async Task<IEnumerable<TodoEntity>> GetTodosByUserId(Guid userId)
        {
            return await _dbContext.todos.Include(list => list.TodoItems.Where(item => item.Deleted == false)).Where(t => t.UserId == userId && t.DeletedAt == null).ToListAsync();
        }

        public async Task<bool> DeleteTodo(Guid todoId)
        {
            var todo = await _dbContext.todos.FindAsync(todoId);

            if (todo == null) return false;

            todo.DeletedAt = DateTime.UtcNow;

            _dbContext.todos.Update(todo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTodo(Guid userId, Guid todoId, TodoEntity todo)
        {
            var existingTodo = await _dbContext.todos
                .Include(t => t.TodoItems)
                .Where(t => t.DeletedAt == null && t.Id == todoId && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (existingTodo != null)
            {
                existingTodo.UpdatedAt = DateTime.UtcNow;
                existingTodo.DueDate = todo.DueDate;
                existingTodo.Title = todo.Title;
                existingTodo.TodoItems = todo.TodoItems.Select(item => new TodoItemEntity
                {
                    ItemTitle = item.ItemTitle,
                    Status = item.Status,
                    Deleted = item.Deleted
                }).ToList();

                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;

        }
    }
}