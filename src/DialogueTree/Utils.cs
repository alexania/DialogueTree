using System;
using System.Collections.Generic;

namespace DialogueTree
{
  public static class Utils
  {
    private static Random _rng = new Random();

    public static T GetRandomFromList<T>(IList<T> list)
    {
      var n = _rng.Next(0, list.Count);
      return list[n];
    }
  }

  public enum ConditionOperation
  {
    Equals,
    NotEquals,
    LessThan,
    LessThanOrEqual,
    MoreThan,
    MoreThanOrEqual
  }
}
