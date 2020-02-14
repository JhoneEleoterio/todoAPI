using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.TarefaModel.Commands;
using TodoList.Domain.TarefaModel.Interfaces;

namespace TodoList.Application.CommandsHandler
{
    public class UpdateCommandTarefaHandler : IRequestHandler<UpdateTarefaCommand, int>
    {
        private ITarefaRepository _repository;

        public UpdateCommandTarefaHandler(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public Task<int> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefa = _repository.GetById(request.Id);

            if (tarefa == null)
                throw new Exception("Id informado inválido");

            tarefa.Titulo = request.Titulo;
            tarefa.Descricao = request.Descricao;
            tarefa.AlteradoEm = DateTime.Now;

            _repository.Update(tarefa);
            _repository.SaveChanges();

            return Task.FromResult(tarefa.Id);
        }
    }
}
