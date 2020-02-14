using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Domain.TarefaModel.Commands;
using TodoList.Domain.TarefaModel.Queries;

namespace TodoList.API.Controllers
{
    /// <summary>
    /// Tarefas
    /// </summary>
    [Route("api/tarefas")]
    public class TarefasController : ControllerBase
    {
        private IMediator _mediator;

        public TarefasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar todas as tarefas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<List<TarefaViewModel>> GetAll() => _mediator.Send(new GetTarefaQuery());

        /// <summary>
        /// Adicionar tarefa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<int> Create([FromBody] CreateTarefaCommand command) => _mediator.Send(command);

        /// <summary>
        /// Atualizar tarefa
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<int> Update(UpdateTarefaCommand command) => _mediator.Send(command);

        /// <summary>
        /// Excluir Tarefa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id) => _mediator.Send(new DeleteTarefaCommand(id));
    }
}
