using System;
using System.Runtime.InteropServices;

namespace WeakReferencesSamples
{
    [Serializable]
    public struct WeakReference<T>
        where T : class
    {
        private readonly WeakReference wrapped;

        public WeakReference(T target)
        {
            wrapped = new WeakReference(target);
        }

        public WeakReference(T target, bool trackResurrection)
        {
            wrapped = new System.WeakReference(target, trackResurrection);
        }

        public bool IsAlive
        {
            get
            {
                return wrapped.IsAlive;
            }
        }

        public T Target
        {
            get
            {
                return (T)wrapped.Target;
            }
            set
            {
                wrapped.Target = value;
            }
        }

        public bool TrackResurrection
        {
            get
            {
                return wrapped.TrackResurrection;
            }
        }
    }

    /// <summary>
    /// From : http://damieng.com/blog/2006/08/28/equatable_weak_references
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EquatableWeakReference<T> : IEquatable<EquatableWeakReference<T>>, IDisposable
        where T : class
    {
        protected GCHandle handle;
        protected int hashCode;

        public EquatableWeakReference(T target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            hashCode = target.GetHashCode();
            InitializeHandle(target);
        }

        protected virtual void InitializeHandle(T target)
        {
            handle = GCHandle.Alloc(target, GCHandleType.Weak);
        }

        ~EquatableWeakReference()
        {
            Dispose();
        }

        public void Dispose()
        {
            handle.Free();
            GC.SuppressFinalize(this);
        }

        public virtual bool IsAlive
        {
            get { return (handle.Target != null); }
        }

        public virtual T Target
        {
            get
            {
                object o = handle.Target;
                if ((o == null) || (!(o is T)))
                    return null;
                else
                    return (T)o;
            }
        }

        public override bool Equals(object other)
        {
            if (other is EquatableWeakReference<T>)
                return Equals((EquatableWeakReference<T>)other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public bool Equals(EquatableWeakReference<T> other)
        {
            return ReferenceEquals(other.Target, this.Target);
        }
    }
}
