namespace DialogueTree.Nodes.Effect
{
  public class MoodEffect : IEffect
  {
    private Mood _mood;

    public MoodEffect(Mood mood)
    {
      _mood = mood;
    }

    public void Set(State state)
    {
      state.Speaker.Mood = _mood;
    }
  }

  public static partial class EffectExtension
  {
    public static T AddMoodEffect<T>(this T node, Mood mood) where T : BaseNode
    {
      node.Effects.Add(new MoodEffect(mood));
      return node;
    }
  }
}
