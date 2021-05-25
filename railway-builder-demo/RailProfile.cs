using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New rail profile", menuName ="Railway/RailProfile")]
public class RailProfile : ScriptableObject
{
    public List<Vector2> points;
}
