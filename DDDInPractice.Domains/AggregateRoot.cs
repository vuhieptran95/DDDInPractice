using System.Collections.Generic;

namespace DDDInPractice.Domains
{
    public class AggregateRoot
    {
        private readonly ICollection<DomainEvent> _domainEvents;

        public AggregateRoot()
        {
            _domainEvents = new List<DomainEvent>();
        }

        public IEnumerable<DomainEvent> DomainEvents => _domainEvents;

        public void AddEvents(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveEvents()
        {
            _domainEvents.Clear();
        }
    }
}