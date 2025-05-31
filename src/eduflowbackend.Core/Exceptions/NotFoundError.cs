using FluentResults;

namespace eduflowbackend.Core.Exceptions;

public class NotFoundError : Error
{
    public NotFoundError(string entity, string id) : base($"{entity} with Id = {id} not found")
    {
    }
    

    public NotFoundError(string entity, int id) : base($"{entity} with Id = {id} not found")
    {
    }

    public NotFoundError(string entity, string key, string value) : base($"{entity} with {key} = {value} not found")
    {
    }
}