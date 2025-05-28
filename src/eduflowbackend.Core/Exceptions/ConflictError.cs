using FluentResults;

namespace eduflowbackend.Core.Exceptions;

public class ConflictError : Error
{
    public ConflictError(string entity, string id) : base($"{entity} with Id = {id} exists")
    {
        
    }
    public ConflictError(string entity, string key, string value) : base($"{entity} with {key} = {value} exist")
    {
        
    }
}