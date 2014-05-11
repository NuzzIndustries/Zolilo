using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zolilo.Data
{
    public class DR_Metrics : GraphNode, IGraphNodeHasDefinition<Metric2Fragment_Definition>
    {
        Metric metricObject;

        #region UniversalAbstractMethods
        public static DR_Metrics Get(long id)
        {
            return ZoliloCache.Instance.Metrics[id];
        }
        #endregion

        public DR_Fragments NodeDefinition
        {
            get { return base.GetDefinition<Metric2Fragment_Definition>(); }
        }

        public string _NAME
        {
            get { return (string)Cells["NAME"].Data; }
            set { Cells["NAME"].Data = value; }
        }

        private string _OBJDATA
        {
            get { return (string)Cells["OBJDATA"].Data; }
            set { Cells["OBJDATA"].Data = value; }
        }

        public Metric MetricObject
        {
            get
            {
                if (metricObject != null)
                    return metricObject;
                if (_OBJDATA == null)
                {
                    metricObject = new Metric();
                    return metricObject;
                }
                byte[] objBytes = Convert.FromBase64String(_OBJDATA);
                BinaryFormatter bf = new BinaryFormatter();
                metricObject = (Metric)bf.Deserialize(new MemoryStream(objBytes));
                return metricObject;
            }
        }

        public override void SaveChanges()
        {
            if (metricObject != null && metricObject.ChangesMade)
            {
                metricObject.ChangesMade = false;
            }
            base.SaveChanges();
        }

        public override NodeType NodeType
        {
            get { return NodeType.Metric; }
        }
    }
}
