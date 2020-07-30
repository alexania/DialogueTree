namespace DialogueTree.Nodes.Conditions
{
  public interface ICondition
  {
    bool Check(State state);

  }
}
