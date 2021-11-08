using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCH
{
    public class GameObjectPool : MonoBehaviour
    {
        /// <summary> Method for Generate Object Pool </summary>
        /// <param name="_object"> Object to pool </param>
        /// <param name="count"> Object Pool's Scale </param>
        /// <param name="isActive"> Initial Activation setting of pooled object </param>
        /// <returns></returns>
        public static GameObject[] GeneratePool(GameObject _object, int count, bool isActive = false)
        {
            GameObject[] returnObjs = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                returnObjs[i] = Instantiate(_object);
                returnObjs[i].name = _object.name + "_" + (i + 1);
                returnObjs[i].SetActive(isActive);
            }
            return returnObjs;
        }

        /// <summary> Method for Generate Object Pool </summary>
        /// <param name="_object"> Object to pool </param>
        /// <param name="count"> Object Pool's Scale </param>
        /// <param name="parent"> Objects's parent folder </param>
        /// <param name="isActive"> Initial Activation setting of pooled object </param>
        /// <returns></returns>
        public static GameObject[] GeneratePool(GameObject _object, int count, Transform parent, bool isActive = false)
        {
            GameObject[] returnObjs = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                returnObjs[i] = Instantiate(_object);
                returnObjs[i].name = _object.name + "_" + (i + 1);
                returnObjs[i].transform.SetParent(parent);
                returnObjs[i].SetActive(isActive);
            }
            return returnObjs;
        }

        /// <summary>   </summary>
        /// <param name="_objectPool"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static GameObject PopObjectFromPool(GameObject[] _objectPool, bool isActive = true)
        {
            for (int i = 0; i < _objectPool.Length; i++)
            {
                if (_objectPool[i].activeInHierarchy) continue;
                _objectPool[i].SetActive(isActive);
                return _objectPool[i];
            }
            return null;
        }

        /// <summary>  </summary>
        /// <param name="_objectPool"></param>
        /// <param name="initPos"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static GameObject PopObjectFromPool(GameObject[] _objectPool, Vector3 initPos, bool isActive = true)
        {
            for (int i = 0; i < _objectPool.Length; i++)
            {
                if (_objectPool[i].activeInHierarchy) continue;
                _objectPool[i].SetActive(isActive);
                _objectPool[i].transform.position = initPos;
                return _objectPool[i];
            }
            return null;
        }

        /// <summary>  </summary>
        /// <param name="_objectPool"></param>
        /// <param name="count"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static GameObject[] PopObjectsFromPool(GameObject[] _objectPool, int count, bool isActive = true)
        {
            GameObject[] returnObjs = new GameObject[count];
            int idx = 0;

            for (int i = 0; i < _objectPool.Length; i++)
            {
                if (_objectPool[i].activeInHierarchy) continue;
                _objectPool[i].SetActive(isActive);
                returnObjs[idx++] = _objectPool[i];
                if (idx == count)
                    break;
            }
            return returnObjs;
        }

        /// <summary>  </summary>
        /// <param name="_objectPool"></param>
        /// <param name="count"></param>
        /// <param name="initPos"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static GameObject[] PopObjectsFromPool(GameObject[] _objectPool, int count, Vector3[] initPos, bool isActive = true)
        {
            GameObject[] returnObjs = new GameObject[count];
            int idx = 0;

            for (int i = 0; i < _objectPool.Length; i++)
            {
                if (_objectPool[i].activeInHierarchy) continue;
                _objectPool[i].SetActive(isActive);
                _objectPool[i].transform.position = initPos[idx];
                returnObjs[idx++] = _objectPool[i];
                if (idx == count)
                    break;
            }
            return returnObjs;
        }
    }
}
