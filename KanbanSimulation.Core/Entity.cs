using KanbanSimulation.Core.Interfaces;
using System;

namespace KanbanSimulation.Core
{
	public abstract class Entity: IEquatable<Entity>, IEntity
	{
		public int Id { get; protected set; }

		protected Entity(int id)
		{
			/*
			if (object.Equals(id, default(TId)))
			{
				throw new ArgumentException("The ID cannot be the type's default value.", "id");
			}*/

			Id = id;
		}
		
		// For simple entities, this may suffice
		// As Evans notes earlier in the course, equality of Entities is frequently not a simple operation
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var entity = obj as Entity;
			if (entity != null)
			{
				return this.Equals(entity);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		public bool Equals(Entity other)
		{
			if (other == null)
			{
				return false;
			}
			return this.Id.Equals(other.Id);
		}
	}
}