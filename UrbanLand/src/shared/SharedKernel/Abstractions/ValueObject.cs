namespace SharedKernel.Abstractions;

public abstract record ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();
    
    public virtual bool Equals(ValueObject? other)
    {
        if (other is null || GetType() != other.GetType())
        {
            return false;
        }
            
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(17, (hash, obj) => hash * 31 ^ obj.GetHashCode());
    }
}


// public abstract class ValueObject : IEquatable<ValueObject>
// {
//     protected abstract IEnumerable<object> GetEqualityComponents();
//
//     public bool Equals(ValueObject? other)
//     {
//         if (other is null || GetType() != other.GetType())
//         {
//             return false;
//         }
//
//         return GetEqualityComponents()
//             .SequenceEqual(other.GetEqualityComponents());
//     }
//
//     public override bool Equals(object? obj)
//     {
//         if (obj is null || ReferenceEquals(this, obj))
//         {
//             return false;
//         }
//
//         return obj is ValueObject valueObject && Equals(valueObject);
//     }
//
//     public override int GetHashCode()
//     {
//         return GetEqualityComponents()
//             .Select(x => x.GetHashCode())
//             .Aggregate((x, y) => x ^ y);
//     }
//
//     public static bool operator ==(ValueObject? left, ValueObject? right)
//     {
//         return Equals(left, right);
//     }
//
//     public static bool operator !=(ValueObject? left, ValueObject? right)
//     {
//         return !Equals(left, right);
//     }
// }