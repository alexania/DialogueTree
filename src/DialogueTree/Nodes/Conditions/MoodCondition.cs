using System.Linq;

namespace DialogueTree.Nodes.Conditions
{
  public class MoodCondition : ICondition
  {
    private Mood[] _speakerMoods;
    private Mood[] _listenerMoods;

    public MoodCondition(Mood[] speakerMoods, Mood[] listenerMoods)
    {
      _speakerMoods = speakerMoods;
      _listenerMoods = listenerMoods;
    }

    public bool Check(State state)
    {
      return (_speakerMoods == null || _speakerMoods.Contains(state.Speaker.Mood)) && (_listenerMoods == null || _listenerMoods.Contains(state.Listener.Mood));
    }
  }

  public static partial class ConditionExtension
  {
    public static T AddSpeakerMoodCondition<T>(this T node, Mood speakerMood) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(new[] { speakerMood }, null));
      return node;
    }

    public static T AddSpeakerMoodCondition<T>(this T node, Mood[] speakerMoods) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(speakerMoods, null));
      return node;
    }

    public static T AddListenerMoodCondition<T>(this T node, Mood listenerMood) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(null, new[] { listenerMood }));
      return node;
    }

    public static T AddListenerMoodCondition<T>(this T node, Mood[] listenerMoods) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(null, listenerMoods));
      return node;
    }

    public static T AddMoodCondition<T>(this T node, Mood speakerMood, Mood listenerMood) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(new[] { speakerMood }, new[] { listenerMood }));
      return node;
    }

    public static T AddMoodCondition<T>(this T node, Mood[] speakerMoods, Mood[] listenerMoods) where T : BaseNode
    {
      node.Conditions.Add(new MoodCondition(speakerMoods, listenerMoods));
      return node;
    }
  }
}
