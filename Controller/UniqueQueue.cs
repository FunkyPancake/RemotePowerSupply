using System.Collections;

namespace Scpi;

public class UniqueQueue<T> : IEnumerable<T>
{
    private readonly HashSet<T> _hashSet = new HashSet<T>();
    private readonly Queue<T> _queue = new Queue<T>();

    public void Enqueue(T element)
    {
        if (_hashSet.Add(element))
        {
            _queue.Enqueue(element);
        }
    }

    public T Dequeue()
    {
        var element = _queue.Dequeue();
        _hashSet.Remove(element);
        return element;
    }

    public int Count => _hashSet.Count;
    
    public IEnumerator<T> GetEnumerator()
    {
        return _queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}