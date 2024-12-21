namespace SamanProject.Domain
{
    public abstract class Entity
    {
        private bool canBeDeleted;

        protected Entity()
        {
        }

        public bool CanBeDeleted() => canBeDeleted;

        protected void MarkAsDeleted() => canBeDeleted = true;
    }
}
