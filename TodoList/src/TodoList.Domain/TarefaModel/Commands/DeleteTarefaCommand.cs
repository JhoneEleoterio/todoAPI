using MediatR;

namespace TodoList.Domain.TarefaModel.Commands
{
    public class DeleteTarefaCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteTarefaCommand(int id)
        {
            Id = id;
        }
    }
}
