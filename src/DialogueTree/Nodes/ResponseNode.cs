using System.Collections.Generic;
using System.Linq;

namespace DialogueTree.Nodes
{
  public class ResponseNode : BaseNode
  {
    public ResponseNode(string[] responses, bool topicEnd = false, PersonStatus validStatus = PersonStatus.Any, PersonStatus changeToStatus = PersonStatus.Any) : base(validStatus, changeToStatus)
    {
      Responses = responses.ToList();
      TopicEnd = topicEnd;
    }

    public IList<string> Responses { get; }

    public bool TopicEnd { get; }

    public override string Activate(State state, int index = -1)
    {
      state.CanEndCurrentTopic = false;

      if (LastParent != null)
      {
        LastParent.Activate(state);
      }

      state.UpdateSpeaker(ChangeToStatus);
      state.CanEndCurrentTopic |= TopicEnd;
      foreach (var effect in Effects)
      {
        effect.Set(state);
      }

      if (index > -1 && index < Responses.Count)
      {
        return Responses[index];
      }

      return Utils.GetRandomFromList(Responses);
    }

    public override ResponseNode GetValidResponse(BaseNode parent, State state)
    {
      return IsValid(state) ? this : null;
    }

    public override IList<ResponseNode> GetValidResponses(BaseNode parent, State state)
    {
      if (IsValid(state))
      {
        if (parent != null)
        {
          LastParent = parent;
        }

        return new List<ResponseNode> { this };
      }

      return null;
    }
  }
}
