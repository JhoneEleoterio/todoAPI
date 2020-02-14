using System;

namespace TodoList.Domain.SharedKernel
{
    public class Entity
    {
        public Guid Rastreador { get; private set; }
        public int Id { get; set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AlteradoEm { get; set; }
        public bool Ativo { get; set; }

        public Entity()
        {
            Rastreador = Guid.NewGuid();
            CriadoEm = DateTime.Now;
            AlteradoEm = null;
            Ativo = true;
        }
    }
}
