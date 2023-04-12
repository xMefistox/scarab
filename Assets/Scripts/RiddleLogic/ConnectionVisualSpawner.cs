using Scarab.RiddleLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Scarab.RiddleVisual
{
    internal class ConnectionVisualSpawner : MonoBehaviour
    {
        [SerializeField] private ConnectionVisual connectionVisualPrefab;
        [SerializeField] private int spawnAmount;
        private ObjectPool<ConnectionVisual> pool;
        private List<ConnectionVisual> activatedConnections = new List<ConnectionVisual>();

        private void Awake()
        {
            pool = new ObjectPool<ConnectionVisual>(
                () => { return Instantiate(connectionVisualPrefab); },
                connection => { connection.gameObject.SetActive(true); },
                connection => { connection.gameObject.SetActive(false); },
                connection => { Destroy(connection.gameObject); },
                false, 45);
        }

        internal void SpawnConnection(ScarabController firstScarab, ScarabController secondScarab)
        {
            ConnectionVisual connection = pool.Get();
            connection.transform.SetParent(transform, false);
            connection.SetConnection(secondScarab.transform.localPosition, firstScarab.transform.localPosition);
            activatedConnections.Add(connection);
        }

        internal void DespawnConnection(ScarabController firstScarab, ScarabController secondScarab)
        {
            ConnectionVisual connection = activatedConnections.Find(connectionVisual => Array.Exists(connectionVisual.ScarabPositions, scarab => scarab == firstScarab.transform.localPosition)
                                                                                     && Array.Exists(connectionVisual.ScarabPositions, scarab => scarab == secondScarab.transform.localPosition));
            if (connection != null)
            {
                pool.Release(connection);
                activatedConnections.Remove(connection);
            }
        }

        internal void Reset()
        {
            foreach(ConnectionVisual connection in activatedConnections)
            {
                pool.Release(connection);
            }
            activatedConnections.Clear();
        }
    }
}