using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public interface IMapper<in T> where T : class
{ 
    static abstract Book ToBookEntity(T response);
    static abstract Patron ToPatronEntity(T response);
}