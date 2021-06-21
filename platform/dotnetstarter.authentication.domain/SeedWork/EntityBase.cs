using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace dotnetstarter.authentication.domain.SeedWork
{
    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; private set; }

        //[Index("IX_InternalReference", IsUnique = true)]
        [MaxLength(10)]
        [Required]
        public string InternalReference { get; private set; }

        public DateTime? DateCreated { get; private set; }

        public DateTime? DateModified { get; private set; }

        public string UserCreated { get; private set; }
        public string UserModified { get; private set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
            InternalReference = ReferenceGenerator.Generate();

            TimeZoneInfo zaTimeZone = TimeHelper.GetTimeZone("South Africa Standard Time")
                ?? TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg")
                ?? TimeZoneInfo.Local;


            DateCreated = TimeHelper.ToSpecificTimeZone(DateTime.UtcNow, zaTimeZone);
        }

        public void SetInternalReference(string reference)
        {
            InternalReference = reference;
        }

        public virtual void OnCreate(DateTime createdAt, string createdBy)
        {

            DateCreated = createdAt;
            UserCreated = createdBy;

        }

        public virtual void OnUpdate(DateTime updatedAt, string modifiedBy)
        {

            DateModified = DateModified;
            UserModified = modifiedBy;

        }
        public virtual void OnDelete() { }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
