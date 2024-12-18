using CoopProject.entities;

namespace CoopProject.facility;

public interface ICreator <out T> where T:Entity
{
    public T CreateEntity(object information);

}