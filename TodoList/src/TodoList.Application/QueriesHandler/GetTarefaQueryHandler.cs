using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.TarefaModel.Interfaces;
using TodoList.Domain.TarefaModel.Queries;

namespace TodoList.Application.QueriesHandler
{
    public class GetTarefaQueryHandler : IRequestHandler<GetTarefaQuery, List<TarefaViewModel>>
    {
        private ITarefaRepository _repository;

        public GetTarefaQueryHandler(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public Task<List<TarefaViewModel>> Handle(GetTarefaQuery request, 
                                                  CancellationToken cancellationToken)
        {
            var tarefas = _repository.GetAll();

            var viewModel = tarefas.Where(_ => _.Ativo == true)
                                   .Select(_ => new TarefaViewModel
                                    {
                                        Id = _.Id,
                                        Titulo = _.Titulo,
                                        Descricao = _.Descricao
                                    })
                                     .OrderByDescending(_ => _.Id)
                                     .ToList();

            return Task.FromResult  (viewModel);

        }
    }
}
