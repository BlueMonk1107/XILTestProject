using System.Collections;

namespace LifeCycle
{
    public abstract class LifeCycleAddConfigBase : IInit
    {
        public ArrayList Objects { get; private set; }

        public void Init()
        {
            Objects = new ArrayList();
            Add();
        }

        protected abstract void Add();
    }
}