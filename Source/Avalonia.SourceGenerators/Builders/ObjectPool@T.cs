using System.Runtime.CompilerServices;

namespace AvaloniaPropertySourceGenerator.Builders;
internal sealed class ObjectPool<T>
    where T : class
{
    /// <summary>
    /// The factory is stored for the lifetime of the pool. We will call this only when pool needs to
    /// expand. compared to "new T()", Func gives more flexibility to implementers and faster than "new T()".
    /// </summary>
    private readonly Func<T> factory;

    /// <summary>
    /// The array of cached items.
    /// </summary>
    private readonly Element[] items;

    /// <summary>
    /// Storage for the pool objects. The first item is stored in a dedicated field
    /// because we expect to be able to satisfy most requests from it.
    /// </summary>
    private T? firstItem;

    /// <summary>
    /// Creates a new <see cref="ObjectPool{T}"/> instance with the specified parameters.
    /// </summary>
    /// <param name="factory">The input factory to produce <typeparamref name="T"/> items.</param>
    public ObjectPool(Func<T> factory)
        : this(factory, Environment.ProcessorCount * 2)
    {
    }

    /// <summary>
    /// Creates a new <see cref="ObjectPool{T}"/> instance with the specified parameters.
    /// </summary>
    /// <param name="factory">The input factory to produce <typeparamref name="T"/> items.</param>
    /// <param name="size">The pool size to use.</param>
    public ObjectPool(Func<T> factory, int size)
    {
        this.factory = factory;
        this.items = new Element[size - 1];
    }

    /// <summary>
    /// Produces a <typeparamref name="T"/> instance.
    /// </summary>
    /// <returns>The returned <typeparamref name="T"/> item to use.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Allocate()
    {
        T? item = this.firstItem;

        if (item is null || item != Interlocked.CompareExchange(ref this.firstItem, null, item))
        {
            item = AllocateSlow();
        }

        return item;
    }

    /// <summary>
    /// Returns a given <typeparamref name="T"/> instance to the pool.
    /// </summary>
    /// <param name="obj">The <typeparamref name="T"/> instance to return.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Free(T obj)
    {
        if (this.firstItem is null)
        {
            this.firstItem = obj;
        }
        else
        {
            FreeSlow(obj);
        }
    }

    /// <summary>
    /// Allocates a new <typeparamref name="T"/> item.
    /// </summary>
    /// <returns>The returned <typeparamref name="T"/> item to use.</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private T AllocateSlow()
    {
        foreach (ref Element element in this.items.AsSpan())
        {
            T? instance = element.Value;

            if (instance is not null)
            {
                if (instance == Interlocked.CompareExchange(ref element.Value, null, instance))
                {
                    return instance;
                }
            }
        }

        return this.factory();
    }

    /// <summary>
    /// Frees a given <typeparamref name="T"/> item.
    /// </summary>
    /// <param name="obj">The <typeparamref name="T"/> item to return to the pool.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void FreeSlow(T obj)
    {
        foreach (ref Element element in this.items.AsSpan())
        {
            if (element.Value is null)
            {
                element.Value = obj;

                break;
            }
        }
    }

    /// <summary>
    /// A container for a produced item (using a <see langword="struct"/> wrapper to avoid covariance checks).
    /// </summary>
    private struct Element
    {
        /// <summary>
        /// The value held at the current element.
        /// </summary>
        internal T? Value;
    }
}