using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

//later can be rewrite on static Pool<T> with dynamic control attachment the objects
public class Pool<T> : MonoBehaviour 
	where T : MonoBehaviour
{
	[SerializeField] private T _prototypeRef;
	[SerializeField] private Transform _container;

	Stack<T> _pool = new Stack<T>();

	public T Get()
	{
		T obj;
 
		if (!TryGet(out obj))
		{
			obj = Instantiate (_prototypeRef, _container, false);
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
			result.gameObject.SetActive (true);
			return true;
		}

		result = null;
		return false;
	}

}
