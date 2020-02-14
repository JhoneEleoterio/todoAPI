using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.TarefaModel;
using TodoList.Domain.TarefaModel.Commands;
using TodoList.Domain.TarefaModel.Interfaces;

namespace TodoList.Application.CommandsHandler
{
    public class CreateCommandTarefaHandler : IRequestHandler<CreateTarefaCommand, int>
    {
        private readonly ITarefaRepository _repository;
        public CreateCommandTarefaHandler(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public Task<int> Handle(CreateTarefaCommand request, 
                                CancellationToken cancellationToken)
        {
            var model = new Tarefa
            (
                request.Titulo,
                request.Descricao
            );

            _repository.Add(model);
            _repository.SaveChanges();

            return Task.FromResult(model.Id);
        }
    }
}
