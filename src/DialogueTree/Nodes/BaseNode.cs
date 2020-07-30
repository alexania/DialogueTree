using DialogueTree.Nodes.Conditions;
using DialogueTree.Nodes.Effect;
using System.Collections.Generic;

namespace DialogueTree.Nodes
{
  public abstract class BaseNode
  {
    protected BaseNode LastParent;

    public BaseNode(PersonStatus validStatus, PersonStatus changeToStatus)
    {
      ValidStatuses = new List<PersonStatus> { validStatus };
      ChangeToStatus = changeToStatus;

      Conditions = new List<ICondition>();
      Effects = new List<IEffect>();

      LastParent = null;
    }

    public IList<ICondition> Conditions { get; }

    public IList<IEffect> Effects { get; }

    public IList<PersonStatus> ValidStatuses { get; }

    public PersonStatus ChangeToStatus { get; }

    public virtual bool IsValid(State state)
    {
      var isValid = ValidStatuses.Contains(PersonStatus.Any) || ValidStatuses.Contains(state.Speaker.Status);
      foreach (var condition in Conditions)
      {
        isValid &= condition.Check(state);
      }
      return isValid;
    }

    public abstract string Activate(State state, int index = -1);

    public abstract ResponseNode GetValidResponse(BaseNode parent, State state);

    public abstract IList<ResponseNode> GetValidResponses(BaseNode parent, State state);
  }
}
