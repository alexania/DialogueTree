using System.Collections.Generic;
using System.Linq;

namespace DialogueTree.Nodes
{
  public class TopicNode : BaseNode
  {
    private readonly IDictionary<int, IList<BaseNode>> _children;

    public TopicNode(string name, PersonStatus validStatus, PersonStatus changeToStatus) : base(validStatus, changeToStatus)
    {
      Name = name;
      CurrentDepth = 1;

      _children = new Dictionary<int, IList<BaseNode>> { { 0, new List<BaseNode>() } };
    }

    public string Name { get; }

    public int CurrentDepth { get; set; }

    public override bool IsValid(State state)
    {
      var isValid = base.IsValid(state);
      isValid &= state.CanEndCurrentTopic || state.CurrentTopic == Name;

      return isValid;
    }

    public override string Activate(State state, int index = -1)
    {
      state.UpdateSpeaker(ChangeToStatus);
      CurrentDepth++;

      if (!IsValid(state) || GetValidResponses(null, state).Count == 0)
      {
        state.CanEndCurrentTopic = true;
      }

      return null;
    }

    public virtual TopicNode AddChild(int topicDepth, BaseNode child)
    {
      if (!_children.ContainsKey(topicDepth))
      {
        _children[topicDepth] = new List<BaseNode>();
      }
      _children[topicDepth].Add(child);
      return this;
    }

    public override ResponseNode GetValidResponse(BaseNode parent, State state)
    {
      var responses = GetValidResponses(parent, state);
      if (responses.Count > 0)
      {
        return Utils.GetRandomFromList(responses);
      }
      return null;
    }

    public override IList<ResponseNode> GetValidResponses(BaseNode parent, State state)
    {
      if (state.CurrentTopic == Name || IsValid(state))
      {
        var children = _children[0].ToList();
        if (_children.ContainsKey(CurrentDepth))
        {
          children.AddRange(_children[CurrentDepth]);
        }

        if (children.Count > 0)
        {
          if (parent != null)
          {
            LastParent = parent;
          }

          var responses = new List<ResponseNode>();

          foreach (var child in children)
          {
            var node = child.GetValidResponses(this, state);
            if (node != null && node.Count > 0)
            {
              responses.AddRange(node);
            }
          }
          return responses;
        }
      }
      return new List<ResponseNode>();
    }
  }
}
