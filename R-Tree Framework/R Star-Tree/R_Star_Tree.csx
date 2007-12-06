protected override void AdjustTree(Node node1, Node node2)
{
	AdjustTree(node1, node2, CalculateHeight(node1));
}
protected override void AdjustTree(Node node)
{
	AdjustTree(node, null, CalculateHeight(node));
}
protected virtual void AdjustTree(Node node1, Node node2, Int32 level)
{
	if (node1.Address.Equals(Root))
	{
		Cache.WritePageData(node1);
		return;
	}
	if (Root.Equals(Address.Empty))
	{
		Node rootNode = new Node(MaximumNodeOccupancy, Address.Empty, typeof(Node));
		Root = rootNode.Address;
		node1.Parent = Root;
		node2.Parent = Root;
		Cache.WritePageData(rootNode);
		TreeHeight++;
		level++;
	}
	Node parent = Cache.LookupNode(node1.Parent);
	NodeEntry entryToUpdate = null;
	foreach (NodeEntry entry in parent.NodeEntries)
		if (entry.Child.Equals(node1.Address))
			entryToUpdate = entry;
	if (entryToUpdate == null)
		parent.AddNodeEntry(new NodeEntry(node1.CalculateMinimumBoundingBox(), node1.Address));
	else
		entryToUpdate.MinimumBoundingBox = node1.CalculateMinimumBoundingBox();
	Cache.WritePageData(node1);
	if (node2 != null)
	{
		parent.AddNodeEntry(new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address));
		Cache.WritePageData(node2);
		if (parent.NodeEntries.Count > MaximumNodeOccupancy)
		{
			if (OverflowMarkers.Contains(level))
			{
				List<Node> splitNodes = Split(parent);
				if (parent.Address.Equals(Root))
					Root = Address.Empty;
				RemoveFromParent(parent);
				AdjustTree(splitNodes[0], splitNodes[1], level - 1);
				return;
			}
			else
			{
				OverflowMarkers.Add(level);
				ReInsert(parent, level - 1);
				AdjustTree(parent, level - 1);
				return;
			}
		}
	}
	AdjustTree(parent, level - 1);
}
protected virtual void AdjustTree(Node node, Int32 level)
{
	AdjustTree(node, null, level);
}