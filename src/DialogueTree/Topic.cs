
using DialogueTree.Nodes;

namespace DialogueTree
{
  public class Topic
  {
    public Topic(TopicNode current)
    {
      Current = current;
      CurrentDepth = 1;
      CanEnd = false;
    }

    public TopicNode Current { get; set; }

    public int CurrentDepth { get; set; }

    public bool CanEnd { get; set; }

    public void Update(TopicNode current)
    {
      if (current == Current)
      {
        CurrentDepth++;
      }
    }

    public void EndTopic(bool canEnd = true)
    {
      CanEnd |= canEnd;
    }
  }
}
