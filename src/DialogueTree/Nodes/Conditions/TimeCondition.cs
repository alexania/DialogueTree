using System;

namespace DialogueTree.Nodes.Conditions
{
  public class TimeCondition : ICondition
  {
    private TimeSpan _minTime = TimeSpan.Zero;
    private TimeSpan _maxTime = TimeSpan.Zero;

    public TimeCondition(string minTime = null, string maxTime = null)
    {
      if (minTime != null)
      {
        if (!TimeSpan.TryParse(minTime, out _minTime))
        {
          throw new Exception($"TimeCondition: MinTime - '{minTime}' is not a valid time.");
        }
      }
      if (maxTime != null)
      {
        if (!TimeSpan.TryParse(maxTime, out _maxTime))
        {
          throw new Exception($"TimeCondition: MaxTime - '{maxTime}' is not a valid time.");
        }
      }

      if (_minTime != TimeSpan.Zero && _maxTime != TimeSpan.Zero && _maxTime < _minTime)
      {
        throw new Exception($"TimeCondition: Unable to create condition as MaxTime '{_maxTime}' is less than MinTime '{_minTime}'.");
      }
    }

    public bool Check(State state)
    {
      var current = DateTime.Now.TimeOfDay;
      return (_minTime == TimeSpan.Zero || current >= _minTime) && (_maxTime == TimeSpan.Zero || current <= _maxTime);
    }
  }

  public static partial class ConditionExtension
  {
    public static T AddTimeCondition<T>(this T node, string minTime = null, string maxTime = null) where T : BaseNode
    {
      node.Conditions.Add(new TimeCondition(minTime, maxTime));
      return node;
    }

    public static T AddTimeCondition<T>(this T node, int minHour = -1, int maxHour = -1) where T : BaseNode
    {
      var minTime = minHour == -1 ? null : $"{minHour}:00";
      var maxTime = maxHour == -1 ? null : $"{maxHour}:00";
      node.Conditions.Add(new TimeCondition(minTime, maxTime));
      return node;
    }
  }
}
