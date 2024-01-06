public abstract class BaseModule {

    public abstract ModuleType GetModuleType ();

    public virtual void Init () { }

    public virtual void Reset () { }

}
