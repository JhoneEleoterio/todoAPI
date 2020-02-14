using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.TarefaModel.Commands;
using TodoList.Domain.TarefaModel.Interfaces;

namespace TodoList.Application.CommandsHandler
{
    public class DeleteCommandTarefaHandler : IRequestHandler<DeleteTarefaCommand, bool>
    {
        private ITarefaRepository _repository;
        public DeleteCommandTarefaHandler(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(DeleteTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefa = _repository.GetById(request.Id);

            tarefa.AlteradoEm = DateTime.Now;

            tarefa.Ativo = false;

            _repository.Update(tarefa);
            
            _repository.SaveChanges();

            return Task.FromResult(tarefa.Ativo);
        }
    }
}
