namespace Core.Abstractions;

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
