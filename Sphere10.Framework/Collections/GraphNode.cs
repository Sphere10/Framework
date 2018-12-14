//-----------------------------------------------------------------------
// <copyright file="GraphNode.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sphere10.Framework {

	public interface ISimpleGraph<TNode>
		where TNode : ISimpleGraph<TNode> {
		IEnumerable<TNode> Connections { get; set; }
	}

	public class SimpleGraph<TNode> : ISimpleGraph<TNode>
		where TNode : ISimpleGraph<TNode> {
		public virtual IEnumerable<TNode> Connections { get; set; }
	}


	public interface ISimpleGraph : ISimpleGraph<ISimpleGraph> {
	}

	public class SimpleGraph : SimpleGraph<SimpleGraph> {
	}

	internal class SimpleGraphAdapter<TNode, TEdge, TWeight> : ISimpleGraph 
		where TNode : IGraph<TNode, TEdge, TWeight>
		where TEdge : IGraphEdge<TNode, TEdge, TWeight>, new() {

		private readonly TNode _complexGraph;
		private readonly Func<ISimpleGraph, TEdge> _edgeCreator;

		public SimpleGraphAdapter(TNode complexGraph, Func<ISimpleGraph, TEdge> edgeCreator) {
			_complexGraph = complexGraph;
			_edgeCreator = edgeCreator;
		}

		public IEnumerable<ISimpleGraph> Connections {
			get {
				return (IEnumerable<ISimpleGraph>)_complexGraph.Edges.Select(edge => new SimpleGraphAdapter<TNode, TEdge, TWeight>(edge.Destination, _edgeCreator) );
			}
			set {
				if (_edgeCreator != null)
					_complexGraph.Edges = (value ?? Enumerable.Empty<ISimpleGraph>()).Select(node => _edgeCreator(node));
			}
		}
	}

	// specialization
	public interface ISimpleGraph<TEntity, TNode> : ISimpleGraph<TNode>
		where TNode : ISimpleGraph<TEntity, TNode> {

		 TEntity Entity { get; set; }
	}

	public class SimpleGraph<TEntity, TNode> : SimpleGraph<TNode>, ISimpleGraph<TEntity, TNode>
		where TNode : ISimpleGraph<TEntity, TNode> {
		public TEntity Entity { get; set; }
	}
	

	public interface IGraph<TNode, TEdge, TWeight>
		where TNode : IGraph<TNode, TEdge, TWeight>
		where TEdge : IGraphEdge<TNode, TEdge, TWeight>, new() {

		IEnumerable<TEdge> Edges { get; set; }

		ISimpleGraph ToSimpleGraph(Func<ISimpleGraph, TEdge> edgeCreator = null);
	}

	public interface IGraph<TNode> : IGraph<TNode, GraphEdge<TNode>, int>
		where TNode : IGraph<TNode> {
	}

	public interface IGraph : IGraph<Graph> {
	}

	public interface IGraphEdge<TNode, TEdge, TWeight>
		where TNode : IGraph<TNode, TEdge, TWeight>
		where TEdge : IGraphEdge<TNode, TEdge, TWeight>, new() {
		TWeight Weight { get; set; }
		TNode Source { get; set; }
		TNode Destination { get; set; }
	}

	public interface IGraphEdge<TNode> : IGraphEdge<TNode, GraphEdge<TNode>, int>
		where TNode : IGraph<TNode> {
	}

	public interface IGraphEdge : IGraphEdge<Graph> {
	}

	public class Graph<TNode, TEdge, TWeight> : IGraph<TNode, TEdge, TWeight>
		where TNode : IGraph<TNode, TEdge, TWeight>
		where TEdge : IGraphEdge<TNode, TEdge, TWeight>, new() {


		public virtual IEnumerable<TEdge> Edges { get; set; }

		public ISimpleGraph ToSimpleGraph(Func<ISimpleGraph, TEdge> edgeCreator = null) {
			return new SimpleGraphAdapter<TNode,TEdge,TWeight>((TNode)(object)this, edgeCreator);
		}

	}

	public class Graph<TNode> : Graph<TNode, GraphEdge<TNode>, int>, IGraph<TNode>
		where TNode : IGraph<TNode> {
	}

	public class Graph : Graph<Graph>, IGraph {
	}

	public class GraphEdge<TNode, TEdge, TWeight> : IGraphEdge<TNode, TEdge, TWeight>
		where TNode : IGraph<TNode, TEdge, TWeight>
		where TEdge : IGraphEdge<TNode, TEdge, TWeight>, new() {

		public virtual TWeight Weight { get; set; }
		public virtual TNode Source { get; set; }
		public virtual TNode Destination { get; set; }
	}

	public class GraphEdge<TNode> : GraphEdge<TNode, GraphEdge<TNode>, int>, IGraphEdge<TNode>
		where TNode : IGraph<TNode> {
	}

	public class GraphEdge : GraphEdge<Graph>, IGraphEdge {
	}

}
