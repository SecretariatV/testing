using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Test2Server.Data.Entities;
using Test2Server.Dtos;
using Test2Server.Services;

namespace Test2Server.Controllers
{
    [ApiController]
    [Route("api/v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(userIdClaim);

            var todo = new TodoEntity
            {
                Title = request.Title,
                DueDate = request.DueDate,
                UserId = userId,
                TodoItems = request.TodoItems.Select(item => new TodoItemEntity
                {
                    ItemTitle = item.ItemTitle,
                    Status = item.Status,
                    Deleted = item.Deleted,
                }).ToList()
            };

            var createTodo = await _todoService.CreateTodoList(todo);

            return CreatedAtAction(nameof(CreateTodoList), new { id = createTodo.Id }, createTodo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(userIdClaim);

            var lists = await _todoService.GetTodosByUserId(userId);

            return Ok(new { List = lists });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User not authenticated.");
            }

            var result = await _todoService.DeleteTodo(id);
            if (!result) return NotFound();

            return Ok(new { Success = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDate(Guid id, [FromBody] TodoRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User not authenticated.");
            }

            var userId = Guid.Parse(userIdClaim);

            var todo = new TodoEntity
            {
                DueDate = request.DueDate,
                Title = request.Title,
                TodoItems = request.TodoItems.Select(item => new TodoItemEntity
                {
                    ItemTitle = item.ItemTitle,
                    Status = item.Status,
                    Deleted = item.Deleted,
                }).ToList(),
            };

            var result = await _todoService.UpdateTodo(userId, id, todo);

            return Ok(new { Success = result });
        }
    }
}