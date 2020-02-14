using MediatR;

namespace TodoList.Domain.TarefaModel.Commands
{
    public class CreateTarefaCommand : IRequest<int>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}
