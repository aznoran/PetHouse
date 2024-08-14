namespace PetHouse.Domain;

public abstract record BaseId<Tid> where Tid: notnull
{
    protected BaseId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static Tid NewEntityId => (Tid)Activator.CreateInstance(typeof(Tid), Guid.NewGuid());

    public static Tid NewEmptyEntityId => (Tid)Activator.CreateInstance(typeof(Tid), Guid.Empty);

    public static Tid Create(Guid id) => (Tid)Activator.CreateInstance(typeof(Tid), id);
}