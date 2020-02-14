using MediatR;

namespace TodoList.Domain.TarefaModel.Commands
{
    public class UpdateTarefaCommand : IRequest<int>
    {
        public int Id { get;  set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

    }
}
