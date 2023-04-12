using Scarab.RiddleLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Scarab.RiddleVisual
{
    internal class ConnectionVisualSpawner : MonoBehaviour
    {
        [SerializeField] private ConnectionVisualController connectionVisualPrefab;
        [SerializeField] private int spawnAmount;
        private ObjectPool<ConnectionVisualController> pool;
        private List<ConnectionVisualController> activatedConnections = new List<ConnectionVisualController>();

        private void Awake()
        {
            pool = new ObjectPool<ConnectionVisualController>(
                () => { return Instantiate(connectionVisualPrefab); },
                connection => { connection.gameObject.SetActive(true); },
                connection => { connection.HideConnection(); },
                connection => { Destroy(connection.gameObject); },
                false, 45);
        }

        internal void SpawnConnection(ScarabController firstScarab, ScarabController secondScarab)
        {
            ConnectionVisualController connection = pool.Get();
            connection.transform.SetParent(transform, false);
            connection.SetConnection(secondScarab.transform.localPosition, firstScarab.transform.localPosition);
            activatedConnections.Add(connection);
        }

        internal void DespawnConnection(ScarabController firstScarab, ScarabController secondScarab)
        {
            ConnectionVisualController connection = activatedConnections.Find(connectionVisual => Array.Exists(connectionVisual.ScarabPositions, scarab => scarab == firstScarab.transform.localPosition)
                                                                                     && Array.Exists(connectionVisual.ScarabPositions, scarab => scarab == secondScarab.transform.localPosition));
            if (connection != null)
            {
                pool.Release(connection);
                activatedConnections.Remove(connection);
            }
        }

        internal void Reset()
        {
            foreach(ConnectionVisualController connection in activatedConnections)
            {
                pool.Release(connection);
            }
            activatedConnections.Clear();
        }
    }
}