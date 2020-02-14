using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.TarefaModel;

namespace TodoList.Infrastructure.Mappings
{
    public class TarefaMapping : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.ToTable("Tarefa");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Titulo).HasMaxLength(155);
            builder.Property(_ => _.Descricao).HasMaxLength(255);
        }
    }
}
