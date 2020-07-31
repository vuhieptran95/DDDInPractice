using ResponsibilityChain;

namespace DDDInPractice.Domains
{
    public abstract class DomainEvent : IRequest<Nothing>
    {
        public Nothing Response { get; set; }
    }
}