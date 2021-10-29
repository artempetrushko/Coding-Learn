using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Choser
{
    public static int Choose(float[] probs)
    {
        float total = 0;

        foreach (var elem in probs) total += elem;

        var randomPoint = Random.value * total;

        for (var i = 0; i < probs.Length; i++)
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        return probs.Length - 1;
    }
}

public enum LimbType
{
    Head,
    Body,
    Hand,
    Leg,
    Other
}

public class LimbsChooseScript : MonoBehaviour
{
    [SerializeField] private List<Limb> Limbs;

    public Transform GetTargetLimb(List<LimbsChooseProbability.LimbsChooseProb> hitProbabilities)
    {
        if (Limbs != null && Limbs.Count != 0&&hitProbabilities!=null&&hitProbabilities.Count!=0)
        {
            var choose = Choser.Choose(hitProbabilities.Select(x => x.Probability).ToArray());
            var selectedType = hitProbabilities[choose].Type;
            var selectedTypeArray = Limbs.Where(x => x.Type==selectedType).ToArray();
            return selectedTypeArray[Random.Range(0,selectedTypeArray.Length)].transform;
        }

        return null;
    }

    [Serializable]
    public class Limb
    {
        public string Name;
        public Transform transform;
        public LimbType Type;
    }
}