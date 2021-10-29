using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LimbsChooseProbability : MonoBehaviour
{
    [SerializeField] private List<LimbsChooseProb> _probabilities;
    public List<LimbsChooseProb> Properties { get=>_probabilities; }
    private void Start()
    {
        var sum = _probabilities.Sum(x => x.Probability);
        if (sum != 1)
            throw new ArgumentException("The sum of the probabilities must be exactly 1!");
        if (_probabilities == null)
            throw new NullReferenceException();
        if (_probabilities.Count == 0)
            throw new ArgumentException("Empty list");
        if (_probabilities.GroupBy(x => x.Type).Any(x => x.Count() > 1))
            throw new ArgumentException("One Type -- One Probability");
    }

    [Serializable]
    public class LimbsChooseProb
    {
        public float Probability;
        public LimbType Type;
    }
}