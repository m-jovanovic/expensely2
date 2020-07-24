using System;

namespace Expensely.Application.Contracts.Common
{
    /// <summary>
    /// Represents the entity created response.
    /// </summary>
    public sealed class EntityCreatedResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCreatedResponse"/> class.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        public EntityCreatedResponse(Guid id) => Id = id;

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        public Guid Id { get; }
    }
}
