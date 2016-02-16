using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;

namespace Dolstagis.Tests.Objects.Fakes
{
    public class FakeIoCContainer : IIoCContainer
    {
        public FakeIoCContainer Parent { get; private set; }

        public IDictionary<Type, IList<IBinding>> _bindingDict
            = new Dictionary<Type, IList<IBinding>>();

        public void Add(IBinding binding)
        {
            IList<IBinding> bindings;
            if (!_bindingDict.TryGetValue(binding.SourceType, out bindings)) {
                bindings = new List<IBinding>();
                _bindingDict.Add(binding.SourceType, bindings);
            }

            if (!binding.Multiple) bindings.Clear();
            bindings.Add(binding);
        }

        public IList<IBinding> GetBindings(Type t, bool includeParents)
        {
            IList<IBinding> bindings;
            var result = _bindingDict.TryGetValue(t, out bindings)
                ? bindings
                : new List<IBinding>();

            if (includeParents && Parent != null && !bindings.Any(x => !x.Multiple)) {
                result = Parent.GetBindings(t, includeParents).Concat(result).ToList();
            }

            return result;
        }

        public void Dispose()
        {
        }

        public object Get(Type t)
        {
            return null;
        }

        public IEnumerable GetAll(Type t)
        {
            return Enumerable.Empty<object>();
        }

        public IIoCContainer GetChildContainer()
        {
            return new FakeIoCContainer() { Parent = this };
        }

        public void Validate()
        {
        }
    }

    public class FakeIoCContainer<TDummy> : FakeIoCContainer
    {
    }
}
