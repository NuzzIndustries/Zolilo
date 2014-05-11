using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Data
{
    public interface IEdgeCollection
    {
        void AddEdge(DR_GraphEdges edge);
        List<DR_GraphEdges> GetInternalList();
    }

    public class EdgeCollection<T> : IEnumerable<T>, IEdgeCollection where T : DR_GraphEdges
    {
        List<DR_GraphEdges> internalList; //This list will be a reference to the primary indexed list
        public List<DR_GraphEdges> GetInternalList()
        {
            return internalList;
        }

        public EdgeCollection(List<DR_GraphEdges> list)
        {
            this.internalList = list;
        }

        public void AddEdge(DR_GraphEdges edge)
        {
            throw new InvalidOperationException("Cannot add edge through this method");
        }

        internal void DeleteAllEdgesPermanently()
        {
            while (internalList.Count > 0)
                internalList[0].DeletePermanently();
        }

        public EdgeCollection<R> Convert<R>() where R : DR_GraphEdges
        {
            return new EdgeCollection<R>(internalList);
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator ie = internalList.GetEnumerator(); //Update to use temporarycopy?
            while (ie.MoveNext())
                yield return (T)ie.Current;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator ie = internalList.GetEnumerator();
            while (ie.MoveNext())
                yield return ie.Current;
        }

        public int Count
        {
            get { return internalList.Count(); }
        }

        public T this[int index]
        {
            get { return (T)internalList[index]; }
        }

        public EdgeCollection<T> GetTemporaryCopy()
        {
            List<DR_GraphEdges> copyList = new List<DR_GraphEdges>(internalList);
            return new EdgeCollection<T>(copyList);
        }
    }
}