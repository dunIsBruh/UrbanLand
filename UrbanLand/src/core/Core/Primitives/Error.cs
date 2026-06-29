namespace Core.Primitives;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Forbidden(string message)
        => new("Forbidden", message);
    
    public static Error NotFound(string entityName, object id) 
        => new("NotFound", $"{entityName} with id {id} not found");
        
    public static Error Validation(string message) 
        => new("Validation", message);
        
    public static Error Unauthorized(string message = "Unauthorized") 
        => new("Unauthorized", message);
        
    public static Error Conflict(string message) 
        => new("Conflict", message);
        
    public static Error Internal(string message = "Internal server error") 
        => new("Internal", message);
}