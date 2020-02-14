using MediatR;
using System.Collections.Generic;

namespace TodoList.Domain.TarefaModel.Queries
{
    public class GetTarefaQuery : IRequest<List<TarefaViewModel>> 
    {}

    public class TarefaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}
