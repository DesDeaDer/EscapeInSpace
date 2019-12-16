using System.Collections.Generic;
using UnityEngine;

//later can be rewrite on static Pool<T> with dynamic control attachment the objects
public class Pool<T> : MonoBehaviour
    where T : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private T _prototypeRef;
    [SerializeField] private Transform _container;

#pragma warning restore 0649
    #endregion

    Stack<T> _pool = new Stack<T>();

    public T Get()
    {
        if (!TryGet(out var obj))
        {
            obj = Instantiate(_prototypeRef, _container, false);
        }

        return obj;
    }

    public IEnumerable<T> GetAll()
    {
        var result = new T[_pool.Count];
        var index = 0;
        while (_pool.Count > 0)
        {
            TryGet(out result[index]);

            ++index;
        }

        return result;
    }

    public void Set(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }

    private bool TryGet(out T result)
    {
        if (_pool.Count > 0)
        {
            result = _pool.Pop();
            result.gameObject.SetActive(true);
            return true;
        }

        result = null;
        return false;
    }
}
